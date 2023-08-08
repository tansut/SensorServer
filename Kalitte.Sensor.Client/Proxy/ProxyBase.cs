using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Core;
using System.Net;
using System.ServiceModel;
using System.Diagnostics;
using System.Reflection;
using System.Net.Sockets;
using System.ServiceModel.Description;
using System.Security.Principal;
using Kalitte.Sensors.Utilities;
using System.Globalization;
using System.Xml;
using System.ServiceModel.Channels;
using Kalitte.Sensors.Exceptions;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Service;

namespace Kalitte.Sensors.Client.Proxy
{
    [Serializable]
    internal class ProxyBase
    {
        // Fields
        private static ILogger defaultLogger = null;
        private static Dictionary<string, bool> impersonationCache = new Dictionary<string, bool>();
        private static ILogger logger = defaultLogger;
        private string m_host;
        private int m_port;
        [NonSerialized]
        private object m_lockObject;
        private bool m_needsInitialization;
        private TimeSpan operationTimeout;
        [NonSerialized]
        private int retryCount;
        [NonSerialized]
        private object wcfProxy;

        private Type proxyType;
        private string endPoint;

        private ClientCredentials credentials;

        public ServiceConfiguration Configuration { get; private set; }


        internal ProxyBase(string host, int port, ServiceConfiguration configuration, Type proxyType, string endPoint, ClientCredentials credentials)
        {
            this.Configuration = configuration;
            //configuration.UseDefaultSettings = false;
            this.m_host = host;
            this.m_port = port;
            this.m_lockObject = new object();
            this.m_needsInitialization = true;
            this.operationTimeout = TimeSpan.Zero;
            this.m_needsInitialization = true;
            this.proxyType = proxyType;
            this.endPoint = endPoint;
            this.credentials = credentials;
        }




        protected internal object CallMethod(string methodName, params object[] args)
        {
            ICommunicationObject wcfProxy = null;
            object obj4;
            Stopwatch stopwatch = new Stopwatch();
            bool impersonate = false;// NeedsImpersonation(methodName, base.GetType());
            try
            {
                wcfProxy = (ICommunicationObject)this.GetWcfProxy(impersonate);
                if (IsProxyStateFaulted(wcfProxy.State))
                {
                    if (logger != null)
                        logger.Info("proxy is in Faulted Or Closed state. Disposing, clearing and getting a new one. state: {0}", new object[] { wcfProxy.State });
                    try
                    {
                        wcfProxy.Abort();
                        ((IDisposable)wcfProxy).Dispose();
                    }
                    catch (Exception exception)
                    {
                        if (logger != null)
                            logger.Error("Disposing proxy caused exception: ex : {0}", new object[] { exception });
                    }
                    this.ClearWcfProxy();
                    wcfProxy = (ICommunicationObject)this.GetWcfProxy(impersonate);
                }
                MethodInfo method = wcfProxy.GetType().GetMethod(methodName);
                if (logger != null)
                {
                    logger.Info("Invoking method {0} on type {1}", new object[] { methodName, wcfProxy.GetType().ToString() });
                }
                stopwatch.Start();
                object obj3 = method.Invoke(wcfProxy, args);
                if (logger != null)
                {
                    logger.Info("Finished Invoking method {0}", new object[] { methodName });
                    logger.Info("Call {0} returned {1}", new object[] { methodName, obj3 });
                }
                this.retryCount = 0;
                obj4 = obj3;
            }
            catch (TargetInvocationException exception2)
            {
                if (logger != null)
                {
                    logger.Error("Invoking method {0} failed, reason {1}", new object[] { methodName, exception2.InnerException });
                }
                Exception innerException = exception2.InnerException;
                if (innerException == null)
                {
                    throw;
                }
                FaultException<SensorFault> exception4 = innerException as FaultException<SensorFault>;

                if (exception4 != null)
                {
                    this.retryCount = 0;
                    throw new SensorClientException("RemoteException", exception4);
                }
                if (stopwatch.IsRunning)
                {
                    stopwatch.Stop();
                }
                if ((wcfProxy != null) && IsProxyStateFaulted(wcfProxy.State))
                {
                    if ((this.retryCount == 0) && (stopwatch.ElapsedMilliseconds < 0x7530L))
                    {
                        if (logger != null)
                            logger.Info("retryCount is 0. Retrying. method: {0}", new object[] { methodName });
                        this.retryCount++;
                        return this.CallMethod(methodName, args);
                    }
                }
                else if (wcfProxy != null)
                {
                    this.retryCount = 0;
                }
                CommunicationException exception5 = innerException as CommunicationException;
                if (exception5 != null)
                {
                    SocketException exception6 = exception5.InnerException as SocketException;
                    if ((exception6 != null) && ((exception6.SocketErrorCode == SocketError.ConnectionRefused) || (SocketError.TimedOut == exception6.SocketErrorCode)))
                    {
                        throw new SensorClientException("NoServer", exception5);
                    }
                }
                throw new SensorClientException("RemoteException", innerException);

            }
            return obj4;
        }

        private void ClearWcfProxy()
        {
            this.wcfProxy = null;
            this.m_needsInitialization = true;
        }

        private object createDefaultChannel(string hostName, int port, Type proxyType, string endPoint)
        {
            Binding binding = ServiceBindingManager.GetDefaultBinding();
            EndpointAddress address = new EndpointAddress(new Uri(string.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}/sensor/service/{3}", new object[] { binding.Scheme, new IdnMapping().GetAscii(hostName), port, endPoint })), new SpnEndpointIdentity(""), new AddressHeader[0]);
            object obj2 = Activator.CreateInstance(proxyType, new object[] { binding, address });
            ChannelFactory factory = (ChannelFactory)obj2.GetType().GetProperty("ChannelFactory").GetValue(obj2, null);

            TypesHelper.AddKnownTypes(factory.Endpoint);
            SetMaxFaultSize(obj2);
            return obj2;
        }

        private object CreateChannel(string hostName, int port, Type proxyType, string endPoint)
        {
            object obj;
            if (Configuration.UseDefaultWcfSettings)
                obj = createDefaultChannel(hostName, port, proxyType, endPoint);
            else
                obj = Activator.CreateInstance(proxyType);
            ChannelFactory factory = (ChannelFactory)obj.GetType().GetProperty("ChannelFactory").GetValue(obj, null);
            if (credentials != null)
            {
                //var existingCredentials = factory.Endpoint.Behaviors.Find<ClientCredentials>();
                //if (existingCredentials != null)
                //{
                //    existingCredentials.UserName.UserName = "admin";
                //    existingCredentials.UserName.Password = "change";
                //}
                factory.Endpoint.Behaviors.Remove<ClientCredentials>();
                factory.Endpoint.Behaviors.Add(credentials);
            }

            TypesHelper.AddKnownTypes(factory.Endpoint);
            return obj;
        }

        internal static void SetMaxFaultSize(object proxy)
        {
            ServiceEndpoint endpoint = (ServiceEndpoint)proxy.GetType().GetProperty("Endpoint").GetValue(proxy, null);
            endpoint.Behaviors.Add(new MaxFaultSizeBehavior());
        }


        private object GetWcfProxy(bool impersonate)
        {
            if (this.m_needsInitialization)
            {
                this.wcfProxy = null;
            }
            if ((this.wcfProxy != null) && !impersonate)
            {
                return this.wcfProxy;
            }
            object newWCFProxy = this.CreateChannel(this.m_host, this.m_port, this.proxyType, this.endPoint);
            if (impersonate)
            {
                this.SetImpersonationLevel(newWCFProxy);
            }
            if (this.operationTimeout != TimeSpan.Zero)
            {
                this.SetOperationTimeout(newWCFProxy);
            }
            else
            {
                OpenConnection(newWCFProxy);
            }
            this.m_needsInitialization = false;
            if (!impersonate)
            {
                this.wcfProxy = newWCFProxy;
            }
            return newWCFProxy;
        }


        [OnDeserialized]
        private void InitProxyOnDeserialization(StreamingContext context)
        {
            this.m_needsInitialization = true;
            this.wcfProxy = null;
        }

        //private static bool NeedsImpersonation(string callName, Type proxyType)
        //{
        //    lock (impersonationCache)
        //    {
        //        bool flag;
        //        if (impersonationCache.TryGetValue(callName, out flag))
        //        {
        //            return flag;
        //        }
        //        foreach (Type type in proxyType.GetInterfaces())
        //        {
        //            foreach (MethodInfo info in type.GetMethods())
        //            {
        //                object[] customAttributes = info.GetCustomAttributes(typeof(NeedsImpersonation), false);
        //                if ((customAttributes != null) && (customAttributes.Length > 0))
        //                {
        //                    impersonationCache[info.Name] = true;
        //                }
        //                else
        //                {
        //                    impersonationCache[info.Name] = false;
        //                }
        //            }
        //        }
        //        return (impersonationCache.TryGetValue(callName, out flag) && flag);
        //    }
        //}

        private static void OpenConnection(object proxy)
        {
            proxy.GetType().GetMethod("Open").Invoke(proxy, null);
        }

        private void SetImpersonationLevel(object newWCFProxy)
        {
            ClientCredentials credentials = (ClientCredentials)newWCFProxy.GetType().GetProperty("ClientCredentials").GetValue(newWCFProxy, null);
            credentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
        }

        internal static bool IsProxyStateFaulted(CommunicationState communicationState)
        {
            return (((communicationState != CommunicationState.Opened) && (communicationState != CommunicationState.Opening)) && (communicationState != CommunicationState.Created));
        }

        public static void SetLogger(ILogger value)
        {
            if (value == null)
            {
                logger = defaultLogger;
            }
            else
            {
                logger = value;
            }
        }

        private void SetOperationTimeout(object proxy)
        {
            IClientChannel channel = (IClientChannel)proxy.GetType().GetProperty("InnerChannel").GetValue(proxy, null);
            channel.OperationTimeout = this.operationTimeout;
        }

        public string Host
        {
            get
            {
                return this.m_host;
            }
            set
            {
                lock (this.m_lockObject)
                {
                    this.m_host = value;
                    this.m_needsInitialization = true;
                }
            }
        }





        public int Timeout
        {
            get
            {
                return (int)this.operationTimeout.TotalSeconds;
            }
            set
            {
                this.operationTimeout = TimeSpan.FromSeconds((double)value);
                this.ClearWcfProxy();
            }
        }
    }




}
