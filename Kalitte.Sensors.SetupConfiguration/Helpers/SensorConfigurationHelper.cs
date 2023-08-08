using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.SetupConfiguration.Helpers
{
    public class SensorConfigurationHelper
    {

        private string  ServerConfigPath { get; set; }
        private string ManagementConfigPath { get; set; }
        public ConfigurationFileHelper ServerConfigurationFile { get; private set; }
        public ConfigurationFileHelper ManagementConfigurationFile { get; private set; }
        public SensorConfigurationHelper(string applicationInstallPath)
        {
            ServerConfigPath = string.Format(@"{0}\ServerWcf\Web.config", applicationInstallPath);
            ManagementConfigPath = string.Format(@"{0}\Management\Web.config", applicationInstallPath);
            ServerConfigurationFile = new ConfigurationFileHelper(ServerConfigPath);
            ManagementConfigurationFile = new ConfigurationFileHelper(ManagementConfigPath);
        }

        private void SetConnectionStrings(string conStr)
        {
            ServerConfigurationFile.SetAttributeValueOfInnerElement("/configuration/connectionStrings", "name", "SensorSqlConstr", "connectionString", conStr);
            ManagementConfigurationFile.SetAttributeValueOfInnerElement("/configuration/connectionStrings", "name", "SensorSqlConstr", "connectionString", conStr);
        }

        private void SetServerPort(string port)
        {
            ServerConfigurationFile.SetAttributeValue("/configuration/KalitteSensorServer/serverConfiguration/serviceConfiguration", "managementServicePort", port);
            ServerConfigurationFile.SetAttributeValue("/configuration/KalitteSensorServer/serverConfiguration/serviceConfiguration", "sensorCommandServicePort", port);
        }

        public void Configure(string conStr, string port)
        {
            SetConnectionStrings(conStr);
            SetServerPort(port);
            ServerConfigurationFile.Save();
            ManagementConfigurationFile.Save();
        }
    }
}
