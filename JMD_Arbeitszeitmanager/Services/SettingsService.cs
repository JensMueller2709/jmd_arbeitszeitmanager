using System;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Specialized;

namespace JMD_Arbeitszeitmanager.Services
{
    public class SettingsService
    {

        public static void ReadAllSettings()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    Console.WriteLine("AppSettings is empty.");
                }
                else
                {
                    foreach (var key in appSettings.AllKeys)
                    {
                        Debug.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);
                    }
                }
            }
            catch (ConfigurationErrorsException)
            {
                Debug.WriteLine("Error reading app settings");
            }
        }

        public static string ReadSetting(string key)
        {
            
            if (App.Current.Properties.Contains(key))
            {
                string prop = App.Current.Properties[key].ToString();
                Debug.WriteLine("Readed Property: " + key + " - " + prop);
                return prop;
            } else
            {
                return "Not Found";
            }
            
            /*
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings.Get(key) ?? "Not Found";
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
                return "";
            }
            */
        }

       

        public static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                

                App.Current.Properties.Add(key, value);
                
                /*
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                */
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
            catch(Exception e)
            {
                Debug.WriteLine("Error writing app settings");
            }
        }

        
    }

}

