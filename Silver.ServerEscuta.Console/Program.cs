using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using Silver;
using Silver.DTO;
using System.Threading;
using Silver.AsteriskClient;
using Silver.Common.Logger;

namespace Silver.ServerEscuta.Console
{

    static class Program
    {
        static AsteriskClient.SaidaPadraoAsterisk saida_padrao = (AsteriskClient.SaidaPadraoAsterisk)Convert.ToInt32(ConfigurationManager.AppSettings["application.output.default"]);

        static void Main(string[] args)
        {
            try
            {
                //Mutex mu = null;
                //try
                //{
                //    mu = Mutex.OpenExisting("Silver.ServerEscuta.Console");
                //}
                //catch (WaitHandleCannotBeOpenedException) { }

                //if (mu == null)
                //    mu = new Mutex(true, "Silver.ServerEscuta.Console");
                //else
                //{
                //    mu.Close();
                //    return;
                //}

                Silver.BLL.SaidaAsterisk servicos_saida = new BLL.SaidaAsterisk();
                AsteriskListener escuta_asterisk = new AsteriskListener();

                if (args == null)
                    escuta_asterisk.SaidaPadrao = saida_padrao;
                else
                {
                    if (args.Length > 0)
                        escuta_asterisk.SaidaPadrao = (AsteriskClient.SaidaPadraoAsterisk)Convert.ToInt32(args[0]);
                    else
                        escuta_asterisk.SaidaPadrao = saida_padrao;
                }

                escuta_asterisk.GravaSaidasAsterisk = servicos_saida.Cadastrar;
                escuta_asterisk.IniciarEscuta();

                HeaderServer();
                int count_saida = 5;

                do
                {
                    System.Console.Out.WriteLine("[{0}] - [{1}] - Pressione qualquer tecla para finalizar o seviço de escuta", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff"), count_saida.ToString("000"));
                    System.Console.ReadKey();
                    count_saida -= 1;
                }
                while (count_saida > 0);
            }
            catch (Exception ex)
            {
                SaidaLogs(ex.Message, Silver.Common.LoggerType.ERRO);
            }
        }

        private static void HeaderServer()
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.Out.WriteLine("".PadRight(80, '*'));
            System.Console.Out.WriteLine("* Aplicação: Servidor de Escuta Silver®\tVersão: {0}\tBuild: {1}", "1.0.0", "1000");
            System.Console.Out.WriteLine("* Saida Padrão: {0}", Enum.GetName(typeof(AsteriskClient.SaidaPadraoAsterisk), saida_padrao));
            System.Console.Out.WriteLine("* Usuario Serviço: {0}\tInício: {1}\t", Environment.UserName, DateTime.Now.ToString("dd-MM-yyyy HH:mm"));
            System.Console.Out.WriteLine("".PadRight(80, '*'));
            System.Console.ForegroundColor = ConsoleColor.Gray;
            System.Console.Out.WriteLine("[{0}] - Servidor de escuta Asterisk Iniciado com sucesso...", DateTime.Now);
        }

        private static void SaidaLogs(string mensagem, Silver.Common.LoggerType tipolog)
        {
            string msg = string.Format(mensagem);
            string path_logs = Path.Combine(ConfigurationManager.AppSettings["application.path.logs"], string.Format("Logs-{0}_Hora-{1}.log", "ServerEscutaConsole", DateTime.Now.ToString("ddMMyyyyHH")));

            LoggerAsync.pathLog = path_logs;
            LoggerAsync.WriteLog(new LoggerMessage() { MessageLogger = msg, TypeLogger = tipolog });

            System.Console.Out.WriteLine(mensagem);
        }
    }
}
