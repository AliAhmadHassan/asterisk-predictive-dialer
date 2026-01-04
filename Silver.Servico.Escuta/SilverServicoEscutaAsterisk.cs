using System;
using System.IO;
using System.ServiceProcess;
using System.Configuration;
using Silver.AsteriskClient;
using Silver.Common;
using Silver.Common.Logger;


namespace Silver.Servico.Escuta
{
    public partial class SilverServicoEscutaAsterisk : ServiceBase
    {
        SaidaPadraoAsterisk saida_padrao;
        public SilverServicoEscutaAsterisk()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                SaidaLogs("Iniciando serviço de escuta com o Asterisk", Common.LoggerType.INFO);

                saida_padrao = (SaidaPadraoAsterisk)Convert.ToInt32(ConfigurationManager.AppSettings["application.output.default"]);

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
                SaidaLogs("Serviço iniciado com sucesso.", Common.LoggerType.INFO);
            }
            catch (Exception ex)
            {
                SaidaLogs(ex.Message, Silver.Common.LoggerType.ERRO);
            }
        }

        protected override void OnStop()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                SaidaLogs(ex.Message, Silver.Common.LoggerType.ERRO);
            }
        }

        private void SaidaLogs(string mensagem, Silver.Common.LoggerType tipolog)
        {
            string msg = string.Format(mensagem);
            string path_logs = Path.Combine(ConfigurationManager.AppSettings["application.path.logs"], string.Format("Logs-{0}_Hora-{1}.log", "ServerEscutaConsole", DateTime.Now.ToString("ddMMyyyyHH")));

            LoggerAsync.pathLog = path_logs;
            LoggerAsync.WriteLog(new LoggerMessage() { MessageLogger = msg, TypeLogger = tipolog });

            System.Console.Out.WriteLine(mensagem);
        }
    }
}
