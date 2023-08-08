using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.SetupConfiguration.Core.Exception;
using System.Data.SqlClient;
using System.Data;

namespace Kalitte.Sensors.SetupConfiguration.Helpers
{
    public static class SqlHelper
    {
        private static readonly string WindowsAuthenticationConnectionString = "Data Source={0};Database=master;Integrated Security=true;";
        private static readonly string SqlServerAuthenticationConnectionString = "Data Source={0};Initial Catalog=master;User Id={1};Password={2};";

        public static List<string> GetDatabases(string conStr)
        {
            List<string> result = new List<string>();
            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "sp_databases";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(reader["DATABASE_NAME"].ToString());
                }
                reader.Close();
                con.Close();
            }
            catch (Exception ee)
            {
                throw new UserException(ee.Message, ee);
            }
            return result;
        }
        public static string CreateConnectionString(string server, string database = null)
        {
            if (string.IsNullOrWhiteSpace(database)) database = "master";
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
            scsb.DataSource = server;
            scsb.IntegratedSecurity = true;
            scsb.InitialCatalog = database;
            return scsb.ToString();
        }

        public static string CreateConnectionString(string server, string userName, string passWord, string database = null)
        {
            if (string.IsNullOrWhiteSpace(database)) database = "master";
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
            scsb.DataSource = server;
            scsb.UserID = userName;
            scsb.Password = passWord;
            scsb.InitialCatalog = database;
            return scsb.ToString();
        }

        public static void ExecuteBatchSqlCommand(string connectionString, string command)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                string[] scr = command.Split(new string[] { "~GO~" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string s in scr)
                {
                    SqlCommand cmd = db.CreateCommand();
                    cmd.CommandText = s;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                db.Close();
            }

        }

    }
}
