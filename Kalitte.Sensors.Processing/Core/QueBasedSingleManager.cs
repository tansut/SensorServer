using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Utilities;
using System.Messaging;
using System.Threading;
using System.Collections;

using Kalitte.Sensors.Exceptions;
using Kalitte.Sensors.Events;
using System.Collections.ObjectModel;
using Kalitte.Sensors.Processing.Core.Process;
using Kalitte.Sensors.Processing.ServerAnalyse;
using Kalitte.Sensors.Processing.ServerAnalyse.Context;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Processing.Core
{
    internal abstract class QueBasedSingleManager<E, R> : SingleModule<E>, INotificationErrorHandler
        where E : PersistEntityBase, IEntityPropertyProvider
        where R : VirtualRunnableBase
    {
        #region fields
        protected R VirtualRunnable;

        private static readonly int MaxQueError = 20;

        private string fullQueName, queName;
        private MessageQueue mq;
        Dictionary<string, Thread> watcherThreadList;
        private volatile bool noMoreProcessing;
        private object noMoreProcessingLock;
        private WaitCallback processEventCallBack;
        ArrayList workerThreads = ArrayList.Synchronized(new ArrayList());
        ArrayList abortedThreads = ArrayList.Synchronized(new ArrayList());
        internal AutoResetEvent threadOnQueEvent;
        TimeSpan receiveWaitsOnQFor = new TimeSpan(0, 0, 0, 0, ServerConfiguration.Current.QueTimeout);
        private long queExceptionCount;

        #endregion

        internal QueBasedSingleManager(E entity)
            : base(entity)
        {
            watcherThreadList = new Dictionary<string, Thread>();
            noMoreProcessing = false;
            this.workerThreads = ArrayList.Synchronized(new ArrayList());
            this.abortedThreads = ArrayList.Synchronized(new ArrayList());
            queName = string.Format("{0}_{1}_{2}", "KalitteSensorServer", QuePrefix, SensorCommon.RemoveNonCharsAndDigits(Entity.Name));
            fullQueName = @".\Private$\" + queName;
            mq = new MessageQueue(fullQueName);
            mq.Formatter = new BinaryMessageFormatter();
            mq.DefaultPropertiesToSend.AttachSenderId = false;
            processEventCallBack = new WaitCallback(ProcessEvent);
            threadOnQueEvent = new AutoResetEvent(true);
            noMoreProcessingLock = new object();
            queExceptionCount = 0;
            CreateAndEnsureQue();
        }

        internal bool SendEventToQue(string source, Events.SensorEventBase evt)
        {
            Send(new KeyValuePair<string, SensorEventBase>(source, evt));
            return true;
        }



        private void CreateAndEnsureQue()
        {
            if (!MessageQueue.Exists(fullQueName))
                MessageQueue.Create(fullQueName);
        }

        protected override void CleanupBeforeDeleting()
        {
            try
            {
                if (MessageQueue.Exists(fullQueName))
                    MessageQueue.Delete(fullQueName);
            }
            catch (System.Exception exc)
            {
                Logger.LogException("Unable to delete que {0}", exc, fullQueName);
            }
        }

        protected void Notify(string source, SensorEventBase evt)
        {
            if (VirtualRunnable != null && !noMoreProcessing)
            {
                VirtualRunnable.Notify(source, evt);
            }
        }


        public override void DelayedStartup()
        {
            ResetRelationStates();
            base.DelayedStartup();
        }

        protected virtual void ResetRelationStates()
        {
            foreach (var item in RelatedModules)
            {
                item.GetProperties().ResetState();
            }
        }


        protected abstract void ProcessMessageFromQue(string messageId, Message msgFromQue, AnalyseContext context);


        private void ProcessEvent(object state)
        {

            this.workerThreads.Add(Thread.CurrentThread);
            Thread.CurrentThread.Name = (string)state;
            //this.IncrementNumActiveEvents();
            string messageId = string.Empty;
            Message msgFromQue = null;
            try
            {
                var durationContext = ServerAnalyseManager.CreateContext<DurationAnalyseContext>();
                bool receivedMsg = this.RetrieveMessageFromQ(ref messageId, ref msgFromQue);
                if (receivedMsg)
                {
                    ProcessMessageFromQue(messageId, msgFromQue, durationContext);
                }

            }
            catch (System.Exception exc)
            {
                if (!(exc is ThreadAbortException))
                {
                    Logger.LogException("Eror ProcessEvent of QueManager", exc);
                }
            }
            finally
            {
                this.workerThreads.Remove(Thread.CurrentThread);
                //this.DecrementNumActiveEvents();
            }

        }


        protected bool RetrieveMessageFromQ(ref string messageId, ref Message message)
        {
            try
            {
                if (queExceptionCount > MaxQueError)
                {
                    queExceptionCount = Interlocked.Read(ref queExceptionCount);
                    return false;
                }
                message = mq.Receive(this.receiveWaitsOnQFor);
                messageId = message.Id;
            }
            catch (System.Messaging.MessageQueueException exception)
            {
                if (exception.MessageQueueErrorCode != MessageQueueErrorCode.MessageNotFound &&
                    exception.MessageQueueErrorCode != MessageQueueErrorCode.IOTimeout)
                {
                    Interlocked.Increment(ref queExceptionCount);
                    Logger.LogException("MessageQue Exception", exception);
                }
                return false;
            }
            catch (System.Exception exception2)
            {
                if (!(exception2 is ThreadAbortException))
                {
                    Logger.LogException("MessageQue Unknown Exception", exception2);
                }
                return false;
            }
            finally
            {
                threadOnQueEvent.Set();
            }
            return true;
        }


        private void StartWatchingQue(object state)
        {
            try
            {
                //this.DecrementNumActiveEvents();
                bool flag = false;
                //Console.WriteLine("StartWatchingQue started" + noMoreProcessing.ToString());

                while (!noMoreProcessing)
                {
                    //Console.WriteLine("ThreadWaitingOnQEvent.WaitOne");
                    if (threadOnQueEvent.WaitOne(0x3e8, false))
                    {
                        //Console.WriteLine("ThreadWaitingOnQEvent.WaitOne done");
                        flag = false;
                        if (!noMoreProcessing)
                        {

                            string threadName = string.Format("{0}_MqRead", Entity.Name);
                            ThreadPool.QueueUserWorkItem(ProcessEvent, threadName);
                            flag = true;
                            //if ((DateTime.Now.Ticks - this.m_timeOfLastNumActiveEventsLog) > 0x23c34600L)
                            //{
                            //    PEUtilities.PELogHelper(this.logger, Level.Verbose, "NumActiveEvents " + this.NumberOfActiveEvents);
                            //    this.m_timeOfLastNumActiveEventsLog = DateTime.Now.Ticks;
                            //}
                        }
                    }                    
                }
                if (flag)
                {
                    bool flag3 = threadOnQueEvent.WaitOne(0x2710, false);
                    Logger.Info("Joined thread waiting on que {0}. Signal:{1}", Entity.Name, flag3);
                }
            }
            finally
            {
                //this.IncrementNumActiveEvents();
                lock (noMoreProcessingLock)
                {
                    Logger.Info("Watch que {0} ending ... NoMoreProcess: {1}", Entity.Name, noMoreProcessing);
                }
            }

        }

        protected abstract string QuePrefix { get; }



        protected virtual void SetRelationStates(R virtualRunnable, ItemState itemState)
        {
            foreach (var item in RelatedModules)
            {
                if (item.GetProperties().StateInfo.State != itemState)
                    item.GetProperties().StateInfo = new ItemStateInfo(itemState);
            }
        }

        protected virtual Collection<IEntityPropertyProvider> RelatedModules
        {
            get
            {
                return new Collection<IEntityPropertyProvider>();
            }
        }


        private void setModuleException(System.Exception ex)
        {
            if (ex is ProcessorException)
            {
                ProcessorException exc = ex as ProcessorException;
                var module = RelatedModules.SingleOrDefault(p => p.Name == exc.RelatedModule);
                if (module != null)
                {
                    module.GetProperties().StateInfo = new ItemStateInfo(exc);
                }
            }
        }

        private void SetModuleExceptions(System.Exception ex)
        {

            itemlock.EnterWriteLock();
            try
            {
                if (ex is ProcessorException)
                    setModuleException(ex);
                else if (ex is DispatcherException)
                    setModuleException(ex);
                else if (ex is MultipleInnerException)
                {
                    var exc = (MultipleInnerException)ex;
                    foreach (var itemEx in exc.DetailedErrors)
                        setModuleException(itemEx);
                }
            }
            finally
            {
                itemlock.ExitWriteLock();
            }

        }

        protected abstract R CreateVirtualRunnable();

        protected void Send(object obj)
        {
            if (queExceptionCount > MaxQueError)
                queExceptionCount = Interlocked.Read(ref queExceptionCount);
            else
                lock (this.mq)
                {
                    try
                    {
                        this.mq.Send(obj);
                    }
                    catch (System.Exception exc)
                    {
                        Interlocked.Increment(ref queExceptionCount);
                        this.Logger.LogException("Unable to write que.", exc);
                    }
                }
        }

        protected virtual SensorException StopVirtual()
        {
            SensorException result = null;
            try
            {
                if (VirtualRunnable != null)
                {
                    result = VirtualRunnable.Stop();
                }
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
            return result;

        }

        protected void DestroyVirtual()
        {
            try
            {
                if (VirtualRunnable != null)
                {
                    VirtualRunnable.Dispose();
                }
                VirtualRunnable = null;
            }
            catch (ThreadAbortException)
            {
                VirtualRunnable = null;
                Thread.ResetAbort();
            }
            catch (System.Exception exc)
            {
                throw new Exception("Unable to stop appdomain successfully and it may be in an unstable state. Please try to stop again.", exc);
            }
        }

        protected override ItemStateInfo Run()
        {
            if (VirtualRunnable != null)
                throw new InvalidOperationException("It seems there is a pending stop. Try to stop again.");
            VirtualRunnable = CreateVirtualRunnable();

            try
            {
                VirtualRunnable.Start();
                SetRelationStates(VirtualRunnable, ItemState.Running);
            }
            catch (System.Exception exc)
            {
                SetModuleExceptions(exc);
                DestroyVirtual();
                throw;
            }

            queExceptionCount = 0;

            lock (noMoreProcessingLock)
            {
                noMoreProcessing = false;
            }
            
            threadOnQueEvent.Reset();
            threadOnQueEvent.Set();
            Thread queThread = new Thread(StartWatchingQue);
            queThread.Name = "WatchingQueThread";
            queThread.Start();
            return ItemStateInfo.Running;
        }

        protected override ItemStateInfo Stop()
        {
            Logger.Info("Stopping {0}", Entity.Name);
            lock (noMoreProcessingLock)
            {
                noMoreProcessing = true;
            }
            bool queDone = threadOnQueEvent.WaitOne(5000, false);
            queExceptionCount = 0;

            Exception excFromStop = null;
            try
            {
                excFromStop = StopVirtual();
                if (excFromStop != null)
                {
                    Logger.Error("Stopped with exception {0}", excFromStop);
                    SetModuleExceptions(excFromStop);
                }
                SetRelationStates(VirtualRunnable, ItemState.Stopped);
                DestroyVirtual();
            }
            catch (ThreadAbortException)
            {
                Manager.Logger.Error("Threadabort while trying to stop {0}. {1}", Entity.Name, excFromStop);
                SetRelationStates(VirtualRunnable, ItemState.Stopped);
                Thread.ResetAbort();
            }
            catch (Exception exc)
            {
                SetModuleExceptions(exc);
                throw;
            }

            return excFromStop == null ? ItemStateInfo.Stopped : new ItemStateInfo(excFromStop);
        }

        protected override ItemStateInfo GetEntityStateUsingException(ItemState newState, Exception exc)
        {
            SetModuleExceptions(exc);
            var state = base.GetEntityStateUsingException(newState, exc);
            state.State = newState;
            return state;
        }


        public void HandleError(object sender, ExceptionEventArgs exc)
        {
            Logger.Info("Error received from pipe {0} {1}", Entity.Name, exc.Exception);
            ChangeState(ItemState.Stopped, false, exc.Exception);
        }


        protected virtual void ModulePropertiesUpdated(IEntityPropertyProvider module, PropertyList profile)
        {

        }

        internal virtual void SetModuleProperties(string module, PropertyList properties)
        {
            ValidateState(ItemState.Running);
            itemlock.EnterReadLock();
            try
            {
                var current = ValidateRelationAsModule(module);
                if (current.GetProperties().StateInfo.State != ItemState.Running)
                    throw new InvalidOperationException(string.Format("Item {0} state should be running to set profile", module));
                if (VirtualRunnable != null)
                {
                    VirtualRunnable.SetModuleProperties(module, properties);
                    ModulePropertiesUpdated(current, properties);
                }
            }
            finally
            {
                itemlock.ExitReadLock();
            }
        }

        protected virtual IEntityPropertyProvider ValidateRelationAsModule(string module)
        {
            var current = RelatedModules.SingleOrDefault(p => p.Name == module);
            if (current == null)
                throw new ArgumentException("Module name is invalid");
            return current;
        }
    }
}
