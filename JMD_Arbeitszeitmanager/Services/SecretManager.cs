using JMD_Arbeitszeitmanager.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace JMD_Arbeitszeitmanager.Services
{
    class SecretManager : ISecretManager
    {
        private readonly string fileName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/JMD_Arbeitszeitmanager_data.dat";
        private readonly string staticEntropy = "secretEntropy123";


        public string getDecryptedPasswordToDB()    
        {
            Debug.WriteLine("Reading data from disk and decrypting...");

            FileStream fStream;

            // Open the file.
            try
            {
                fStream = new FileStream(fileName, FileMode.Open);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return "";
            }

            try
            {
                Debug.WriteLine(fStream.Name);

                // Create some random entropy.
                byte[] entropy = UnicodeEncoding.ASCII.GetBytes(staticEntropy);

                int bytesWritten = int.Parse(SettingsService.ReadSetting(Properties.Resources.ByteLengthPassword));

                // Read from the stream and decrypt the data.
                byte[] decryptData = MemoryProtection.DecryptDataFromStream(entropy, DataProtectionScope.CurrentUser, fStream, bytesWritten);

                fStream.Close();

                //Debug.WriteLine("Decrypted data: " + UnicodeEncoding.ASCII.GetString(decryptData));
                return UnicodeEncoding.ASCII.GetString(decryptData);
            } catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return "";
            }
            
        }

        public void saveDatabaseConnectionParameters(string user, string port, string pw, string url, string dbName, string sslCaPath, string sslKeyPath, string sslCertPath)
        {
            SettingsService.AddUpdateAppSettings(Properties.Resources.DBUser, user);
            SettingsService.AddUpdateAppSettings(Properties.Resources.ServerAddress, url);
            SettingsService.AddUpdateAppSettings(Properties.Resources.Port, port);
            SettingsService.AddUpdateAppSettings(Properties.Resources.DatabaseName, dbName);
            SettingsService.AddUpdateAppSettings(Properties.Resources.SSLCaPath, sslCaPath);
            SettingsService.AddUpdateAppSettings(Properties.Resources.SSLKeyPath, sslKeyPath);
            SettingsService.AddUpdateAppSettings(Properties.Resources.SSLCertPath, sslCertPath);

            saveDBPasswordSecurely(pw);

        }

        public void saveDBPasswordSecurely(string pw)
        {
            try { 

            // Create the original data to be encrypted
            byte[] toEncrypt = UnicodeEncoding.ASCII.GetBytes(pw);

            // Create a file.
            FileStream fStream = new FileStream(fileName, FileMode.OpenOrCreate);

            // Create some random entropy.
            //byte[] entropy = MemoryProtection.CreateRandomEntropy();
            byte[] entropy = UnicodeEncoding.ASCII.GetBytes(staticEntropy);


            //Debug.WriteLine("Original data: " + UnicodeEncoding.ASCII.GetString(toEncrypt));
            Debug.WriteLine("Encrypting and writing to disk...");

            // Encrypt a copy of the data to the stream.
            int bytesWritten = MemoryProtection.EncryptDataToStream(toEncrypt, entropy, DataProtectionScope.CurrentUser, fStream);

            SettingsService.AddUpdateAppSettings(Properties.Resources.ByteLengthPassword, bytesWritten.ToString());

            fStream.Close();
            } catch (Exception e)
            {
                Debug.WriteLine("Something went wron during saving the db password");
            } 
        }
    }
}
