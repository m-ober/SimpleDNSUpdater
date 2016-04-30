using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace SimpleDNSUpdater
{
    public static class SimpleDNSUpdater
    {
        private static string IpAddrRegex = @"(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}(1[0-9]{2}|2[0-4][0-9]|25[0-5]|[1-9][0-9]|[0-9])";
        private static System.Timers.Timer updateTimer;

        public static void Start()
        {
            updateTimer = new System.Timers.Timer();
            updateTimer.Elapsed += new System.Timers.ElapsedEventHandler(UpdateCheck);
            updateTimer.Interval = 120000;
            updateTimer.Enabled = true;
            Logfile.Append("Service started");
        }

        public static void Stop()
        {
            updateTimer.Enabled = false;
            Logfile.Append("Service stopped");
        }

        private static string GetExternalIpAddr()
        {
            string result = String.Empty;
            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Proxy = null;
                    string html = wc.DownloadString(Config.Instance.ExternalAddress);
                    MatchCollection matches = new Regex(IpAddrRegex).Matches(html);
                    if (matches.Count > 0)
                    {
                        result = matches[0].Value;
                    }
                    else
                    {
                        Logfile.Append("Could not find any valid IP address on " + Config.Instance.ExternalAddress);
                    }
                }
            }
            catch (WebException e)
            {
                Logfile.Append("Error while loading page " + Config.Instance.ExternalAddress + ": " + e.Message);
            }
            return result;
        }

        private static void UpdateDNS()
        {
            string newAddr = GetExternalIpAddr();

            if (newAddr != String.Empty && newAddr != Config.Instance.IPAddr) // detect ip address change
            {
                Logfile.Append("IP address changed to: " + newAddr);

                Config.Instance.IPAddr = newAddr;
                var args = new Dictionary<string, string>() {
                    {"[HOST]", Config.Instance.Host},
                    {"[DOMAIN]", Config.Instance.Domain},
                    {"[PASSWORD]", Config.Instance.Password},
                    {"[IPADDR]", Config.Instance.IPAddr}
                };
                string updateURL = args.Aggregate(Config.Instance.UpdateURL,
                    (current, value) => current.Replace(value.Key, value.Value));

                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.Proxy = null;
                        wc.DownloadString(updateURL);
                    }
                }
                catch (WebException e)
                {
                    Logfile.Append("Error while updating DNS with request " + updateURL + ": " + e.Message);
                }
            }
        }

        private static void UpdateCheck(object source, System.Timers.ElapsedEventArgs e)
        {
            UpdateDNS();
        }
    }
}
