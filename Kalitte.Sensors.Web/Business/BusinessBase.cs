using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Client;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Processing;
using Kalitte.Sensors.Security;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Events;
using System.ServiceModel.Description;
using System.Threading;
using System.Security;
using System.Configuration;

namespace Kalitte.Sensors.Web.Business
{
    public abstract class BusinessBase
    {
        private SensorClient client = null;

        static BusinessBase() {
            if (ConfigurationManager.AppSettings["useDefaultWcfSettings"] != "true")
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) =>
                {
                    return true;
                };
        }

        public abstract System.Collections.IList GetItems();

        public SensorClient SensorProxy
        {
            get
            {
                if (client == null) {
                    Logindata login = AuthenticationBusiness.GetLoginData();
                    var credentials = new ClientCredentials();
                    credentials.UserName.UserName = Thread.CurrentPrincipal.Identity.Name;
                    credentials.UserName.Password = login.Password;
                    var serviceConfiguration = new ServiceConfiguration();
                    serviceConfiguration.UseDefaultWcfSettings = ConfigurationManager.AppSettings["useDefaultWcfSettings"] == "true";
                    serviceConfiguration.ManagementServicePort = login.Port;
                    serviceConfiguration.SensorCommandServicePort = login.Port;
                    client = new SensorClient(login.ServerHost, login.ServerHost, serviceConfiguration, credentials);
                }
                return client;
            }
        }

        public ExtendedMetadata GetItemExtendedMetadata(ProcessingItem itemType)
        {
            return SensorProxy.GetItemExtendedMetadata(itemType);
        }

        public Dictionary<PropertyKey, EntityMetadata> GetItemDefaultMetadata(ProcessingItem itemType, string itemName = null)
        {
            return SensorProxy.GetItemDefaultMetadata(itemType, itemName);
        }

        public LogQueryResult GetItemLog(ProcessingItem itemType, string itemName, LogQuery query)
        {
            return this.SensorProxy.GetItemLog(itemType, itemName, query);
        }

        public Dictionary<ProcessingItem, IEnumerable<string>> GetLogSources()
        {
            return SensorProxy.GetLogSources();
        }


        public IList<LastEvent> GetLastEvents(ProcessingItem itemType, string itemName)
        {
            return SensorProxy.GetLastEvents(itemType, itemName);

        }
    }
}
