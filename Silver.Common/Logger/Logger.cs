using System;
using System.Collections.Generic;
using System.IO;

namespace Silver.Common.Logger
{
    public class Logger
    {
        private string pathLog;
        private string directoryday;
        private string directorydefault;

        public static Queue<string> Mensagens = new Queue<string>();

        public string DiretorioLog
        {
            get
            {
                return pathLog;
            }
        }

        /// <summary>
        /// Registra os logs em um arquivo texto
        /// </summary>
        public Logger()
        {
            directoryday = DateTime.Today.ToString("dd/MM/yyyy").Replace('/', '-');
            directorydefault = @"C:\Windows\temp\";

            if (!Directory.Exists(@"C:\Windows\temp\"))
            {
                Directory.CreateDirectory(@"C:\Windows\temp\");
            }
            if (!Directory.Exists(string.Format(@"C:\Windows\temp\{0}", directoryday)))
            {
                Directory.CreateDirectory(Path.Combine(directorydefault, directoryday));
            }
            pathLog = string.Format(@"C:\Windows\temp\{0}\CobNetDefaultLogRegister.log", directoryday);
        }

        /// <summary>
        /// Registra os logs em um arquivo texto
        /// </summary>
        /// <param name="pathLog"></param>
        public Logger(string pathLog)
        {
            this.pathLog = pathLog;
        }

        /// <summary>
        /// Registra os logs em uma base de dados
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="dataProvider"></param>
        public Logger(string connectionString, string dataProvider)
        {
        }

        /// <summary>
        /// Escreve mensagens de log
        /// </summary>
        /// <param name="message">Mensagem que se deseja registrar</param>
        /// <param name="typelog">Tipo do Log. 0 = INFO, 1 = ERROR</param>
        public void WriteLog(string message, LoggerType typelog)
        {
            lock (this)
            {
                using (var fs = new FileStream(pathLog, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (var sw = new StreamWriter(fs))
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
