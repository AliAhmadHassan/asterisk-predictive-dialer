using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using Silver.BLL.Logs;
using Silver.DTO;
using System.Configuration;

namespace Silver.RoboDiscagem.Exceptions
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva  
    /// Data: 25/04/2013
    /// </summary>
    public static class GlobalException
    {
        public static event ListnerExceptionInForm onExceptionInForm = new ListnerExceptionInForm(GlobalException.GlobalExceptionListner);
        
        public static void GlobalExceptionListner(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                string msg = string.Format("[{0}] - {1}", DateTime.Now.ToString(), e.Exception.Message + Environment.NewLine + e.Exception.StackTrace);
                string path_logs = Path.Combine(ConfigurationManager.AppSettings["application.path.logs"], string.Format("Logs-{0}_Hora-{1}.log", "ServerEscuta", DateTime.Now.ToString("ddMMyyyyHH")));

                Silver.Common.Logger.LoggerAsync.pathLog = path_logs;
                Silver.Common.Logger.LoggerAsync.WriteLog(new Common.Logger.LoggerMessage { MessageLogger = msg, TypeLogger = Common.LoggerType.ERRO });
            }
            catch { }
            finally { SetException(sender, e); }
        }

        public static void SetException(object s, ThreadExceptionEventArgs e)
        {
            if (onExceptionInForm != null)
                onExceptionInForm(s, e);
        }
    }
}
