using JMD_Arbeitszeitmanager.Contracts.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace JMD_Arbeitszeitmanager.Services.Database
{
    public class DatabaseConnector : IDatabaseConnector
    {
        private readonly ISecretManager _secretManager;

        private string connection_string=null;

        public DatabaseConnector(ISecretManager secretManager)
        {
            _secretManager = secretManager;
        }

        public MySqlConnection getDBConnection()
        {
            return getConnection();
        }

        public void setNewConnectionString()
        {
            connection_string = null;
            getDBConnectionString();
        }

        public bool checkDB_Conn()
        {
            var conn_info = getDBConnectionString();
            bool isConn = false;
            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection(conn_info);
                conn.Open();
                isConn = true;
            }
            catch (ArgumentException a_ex)
            {
                /*
                Console.WriteLine("Check the Connection String.");
                Console.WriteLine(a_ex.Message);
                Console.WriteLine(a_ex.ToString());
                */
            }
            catch (MySqlException ex)
            {
                string sqlErrorMessage = "Message: " + ex.Message + "\n" +
                "Source: " + ex.Source + "\n" +
                "Number: " + ex.Number;
                Debug.WriteLine(sqlErrorMessage);
                
                isConn = false;
                switch (ex.Number)
                {
                    //http://dev.mysql.com/doc/refman/5.0/en/error-messages-server.html
                    case 1042: // Unable to connect to any of the specified MySQL hosts (Check Server,Port)
                        break;
                    case 0: // Access denied (Check DB name,username,password)
                        break;
                    default:
                        break;
                }
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return isConn;
        }

        private string getDBConnectionString()
        {
            if (!String.IsNullOrEmpty(connection_string))
            {
                return connection_string;
            }

            string connectionString;
            
            string server, port, database, user, pw, sslCert, sslKey, sslCa;

            server = SettingsService.ReadSetting(Properties.Resources.ServerAddress);
            port = SettingsService.ReadSetting(Properties.Resources.Port);
            database = SettingsService.ReadSetting(Properties.Resources.DatabaseName);
            user = SettingsService.ReadSetting(Properties.Resources.DBUser);
            sslCert = SettingsService.ReadSetting(Properties.Resources.SSLCertPath);
            sslKey = SettingsService.ReadSetting(Properties.Resources.SSLKeyPath);
            sslCa = SettingsService.ReadSetting(Properties.Resources.SSLCaPath);

            pw = _secretManager.getDecryptedPasswordToDB();

            Debug.WriteLine("----------------PW-------------------");

            if (String.IsNullOrEmpty(pw))
            {
                Debug.WriteLine("Missing PW");
            }

            connectionString = String.Format(@"Server={0};Port={1};Database={2};Uid={3};Pwd={4};SSL Mode=Required;
                                          SslCert={5};
                                          SslKey={6};
                                          Ssl-Ca={7};
                                          pooling=True;", server, port, database, user, pw, sslCert, sslKey, sslCa);

            //Debug.WriteLine(connectionString);  

            connection_string = connectionString;

            return connectionString;
        }

        private MySqlConnection getConnection()
        {
            string connectionString;
            MySqlConnection cnn;
            connectionString = getDBConnectionString();

            //Debug.WriteLine(connectionString);

            try
            {
                cnn = new MySqlConnection(connectionString);

                return cnn;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return null;
            }
        }

        public bool checkDB_Conn_With_Parameters(string user, string port, string pw, string url, string dbName, string sslCaPath, string sslKeyPath, string sslCertPath)
        {
            throw new NotImplementedException();
        }
    }
}
