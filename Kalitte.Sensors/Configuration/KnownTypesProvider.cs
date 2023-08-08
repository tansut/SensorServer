using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Provider;
using System.Configuration;

namespace Kalitte.Sensors.Configuration
{
    public enum AppDomainUsage
    {
        UseCurrent,
        CreateNew
    }

    public abstract class KnownTypesProvider : ProviderBase
    {
        private string typeQ;
        private AppDomainUsage appDomainUsage;

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);
            if (config["type"] != null)
            {
                if (ConfigurationManager.ConnectionStrings[config["type"]] != null)
                    typeQ = ConfigurationManager.ConnectionStrings[config["connectionString"]].ConnectionString;
                else typeQ = "";
            }

            if (config["appDomainUsage"] != null)
            {
                appDomainUsage = (AppDomainUsage)Enum.Parse(typeof(AppDomainUsage), config["appDomainUsage"]);
            }
            else appDomainUsage = AppDomainUsage.UseCurrent;
        }


        public AppDomainUsage DomainUsage
        {
            get
            {
                return appDomainUsage;
            }
        }


        public string Type
        {
            get
            {
                return typeQ;
            }
        }
    }


}
