using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace JMD_Arbeitszeitmanager.Contracts.Services
{
    public interface IDatabaseConnector
    {

        MySqlConnection getDBConnection();

        bool checkDB_Conn();

        bool checkDB_Conn_With_Parameters(string user, string port, string pw, string url, string dbName, string sslCaPath, string sslKeyPath, string sslCertPath);

        void setNewConnectionString();

    }
}
