using System;
using System.IO;

namespace SimpleDNSUpdater
{
    static class Logfile
    {
        static public void Append(string s)
        {
            File.AppendAllText(Config.Instance.Directory + @"\log.txt",
                String.Format("{0}\t{1}", System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), s) + Environment.NewLine);
        }
    }
}
