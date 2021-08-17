using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Xml;

namespace CSGOTraderBot.Helper
{
    public class Config
    {
        public static void Set(string key, string value)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;

            settings[key].Value = value;

            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }

        public static void Set(string key, string value, string sectionName)
        {
            var xmlDoc = new XmlDocument();

            xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            XmlElement xmlConfig = (XmlElement)xmlDoc.SelectSingleNode($"//{sectionName}/add[@key='{key}']");

            if (xmlConfig != null)
            {
                xmlConfig.SetAttribute("value", value);

                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection(sectionName);
            }
        }

        public static string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string Get(string key, string sectionName)
        {
            var section = ConfigurationManager
                .GetSection(sectionName) as NameValueCollection;

            return section.GetValues(key)
                .FirstOrDefault();
        }
    }
}
