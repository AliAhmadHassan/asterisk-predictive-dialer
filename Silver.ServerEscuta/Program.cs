using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Diagnostics;
using System.Configuration;
using System.IO;

namespace Silver.ServerEscuta
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new SilverServerEscutaRamalDesligou() 
            };

            #region Testes

            SaidaLogs("Servidor iniciando em modo TESTE", Common.LoggerType.INFO);
            //ClienteAsterisk cliente_asterisk = new ClienteAsterisk();
            //cliente_asterisk.IniciarConexao();

            Silver.BLL.SaidaAsterisk servicos_saida = new BLL.SaidaAsterisk();
            Silver.AsteriskClient.AsteriskListener escuta_asterisk = new AsteriskClient.AsteriskListener();

            escuta_asterisk.SaidaPadrao = AsteriskClient.SaidaPadraoAsterisk.BancoDados;
            escuta_asterisk.GravaSaidasAsterisk = servicos_saida.Cadastrar;
            escuta_asterisk.IniciarEscuta();

            while (true)
                System.Threading.Thread.Sleep(10);
            
            #endregion

            ServiceBase.Run(ServicesToRun);
        }

        private static EventLog Log = new EventLog();

        private static void SaidaLogs(string mensagem, Common.LoggerType tipolog)
        {
            string msg = string.Format(mensagem);
            string path_logs = Path.Combine(ConfigurationManager.AppSettings["application.path.logs"], string.Format("Logs-{0}_Hora-{1}.log", "ServerEscuta", DateTime.Now.ToString("ddMMyyyyHH")));

            Silver.Common.Logger.LoggerAsync.pathLog = path_logs;
            Silver.Common.Logger.LoggerAsync.WriteLog(new Common.Logger.LoggerMessage { MessageLogger = msg, TypeLogger = tipolog });
        }
    }
}
