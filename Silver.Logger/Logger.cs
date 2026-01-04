using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;

namespace Silver.Logger
{
    public static class Logger
    {
        private static object lock_thread = new object();

        public static void WriteLog(string message, LoggerType typelog)
        {
            lock (lock_thread)
            {
                using (FileStream fs = new FileStream(ConfigurationManager.AppSettings["Application.Paths.Log"], FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(string.Format("[{0}] - [{1}] - {2}", DateTime.Now, typelog.ToString(), message));
                        sw.Close();
                    }
                    fs.Close();
                }
            }
        }
    }
}
