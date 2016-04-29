using System.Xml;

namespace SimpleDNSUpdater
{
    public class Config
    {
        private static Config instance;

        private string longName;
        private string shortName;
        private string dependencies;
        private string externalAddress;
        private string updateURL;
        private string host;
        private string domain;
        private string password;
        private string ipAddr;

        private Config()
        {
            longName = "Dynamic DNS Updater";
            shortName = "dyndnsupd";
            dependencies = "tcpip";

            var cfg = new XmlDocument();
            cfg.Load(CfgFile);

            externalAddress = cfg.DocumentElement.SelectSingleNode("/Configuration/Settings/ExternalAddress").InnerText;
            updateURL = cfg.DocumentElement.SelectSingleNode("/Configuration/Settings/UpdateURL").InnerText;
            host = cfg.DocumentElement.SelectSingleNode("/Configuration/Settings/Host").InnerText;
            domain = cfg.DocumentElement.SelectSingleNode("/Configuration/Settings/Domain").InnerText;
            password = cfg.DocumentElement.SelectSingleNode("/Configuration/Settings/Password").InnerText;
            ipAddr = cfg.DocumentElement.SelectSingleNode("/Configuration/Settings/IPAddr").InnerText;
        }

        public string ExternalAddress
        {
            get { return externalAddress; }
        }

        public string UpdateURL
        {
            get { return updateURL; }
        }

        public string Host
        {
            get { return host; }
        }

        public string Domain
        {
            get { return domain; }
        }

        public string Password
        {
            get { return password; }
        }

        public static Config Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Config();
                }
                return instance;
            }
        }

        public string LongName
        {
            get { return longName; }
        }

        public string ShortName
        {
            get { return shortName; }
        }

        public string Dependencies
        {
            get { return dependencies; }
        }

        public string IPAddr
        {
            get { return ipAddr; }
            set
            {
                if (value != ipAddr)
                {
                    ipAddr = value;
                    var cfg = new System.Xml.XmlDocument();
                    cfg.Load(CfgFile);
                    cfg.DocumentElement.SelectSingleNode("/Configuration/Settings/IPAddr").InnerText = ipAddr;
                    cfg.Save(CfgFile);
                }
            }
        }

        public string ThisAssembly
        {
            get { return System.Reflection.Assembly.GetExecutingAssembly().Location; }
        }

        public string Directory
        {
            get { return System.IO.Path.GetDirectoryName(ThisAssembly); }
        }

        public string CfgFile
        {
            get { return Directory + @"\SimpleDNSUpdater.xml"; }
        }
    }
}
