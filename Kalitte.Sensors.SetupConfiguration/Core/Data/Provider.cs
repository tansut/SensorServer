using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Kalitte.Sensors.SetupConfiguration.Core.Data
{

    public enum AuthenticationType
    {
        Windows, SqlServer
    }
    public enum ProviderType
    {
        MSSQLServer, MySql, FileSystem
    }
    [Serializable]
    public class Provider
    {
        public ProviderType Type { get; set; }
        public AuthenticationType Authentication { get; set; }
        public string Path { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public Provider()
        {
            Server = "localhost";
        }

        public Provider(SqlConnectionStringBuilder sqlResult)
        {
            this.Type = ProviderType.MSSQLServer;
            if (sqlResult.IntegratedSecurity)
            {
                this.Authentication = AuthenticationType.Windows;
            }
            else
            {
                this.Authentication = AuthenticationType.SqlServer;
                this.UserName = sqlResult.UserID;
                this.Password = sqlResult.Password;
            }
            this.Server = sqlResult.DataSource;
            this.Database = sqlResult.InitialCatalog;
            this.Path = sqlResult.ConnectionString;
        }
    }
}
