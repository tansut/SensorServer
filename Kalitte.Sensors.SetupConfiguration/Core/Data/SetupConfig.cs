using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.SetupConfiguration.Helpers;

namespace Kalitte.Sensors.SetupConfiguration.Core.Data
{
    [Serializable]
    public class SetupConfig
    {
        private int managementPort = 8746;
        public int ManagementPort
        {
            get
            {
                return managementPort;
            }
            set
            {
                managementPort = value;
            }
        }
        private Provider dataProvider;
        public Provider DataProvider
        {
            get
            {
                if (dataProvider == null) dataProvider = new Provider();
                return dataProvider;
            }
            set
            {
                dataProvider = value;
            }
        }
        public bool InstallAsService { get; set; }
        public bool CreateIISApplication { get; set; }
        public string ApplicationInstallPath { get; set; }
        public MSMQState MsmqState { get; set; }
        public IISState IisState { get; set; }
        public string WebSiteName { get; set; }
        public string ApplicationPool { get; set; }
    }
}
