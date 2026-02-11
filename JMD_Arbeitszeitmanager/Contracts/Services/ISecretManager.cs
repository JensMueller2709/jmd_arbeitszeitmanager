using System;
using System.Collections.Generic;
using System.Text;

namespace JMD_Arbeitszeitmanager.Contracts.Services
{
    public interface ISecretManager
    {

        string getDecryptedPasswordToDB();

        void saveDBPasswordSecurely(string pw);

        void saveDatabaseConnectionParameters(string user, string port, string pw, string url,string dbName, string sslCaPath, string sslKeyPath, string sslCertPath);

    }
}
