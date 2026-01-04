using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Configuration;

namespace Silver.ProxyDiscagem
{
    public partial class ProxyDiscagemServico : ServiceBase
    {
        ProxyDiscagem servico_proxy_discagem;

        public ProxyDiscagemServico()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                MensagemLog("Iniciando serviço do Proxy de Discagem Silver.", Common.LoggerType.INFO);
                
                servico_proxy_discagem = new ProxyDiscagem();
                servico_proxy_discagem.IniciarServico();

                MensagemLog("Serviço do Proxy de Discagem Silver iniciado com sucesso.", Common.LoggerType.INFO);
            }
            catch (Exception ex)
            {
                MensagemLog("Houve uma falha no serviço. A mensagem do erro foi: " + ex.Message + Environment.NewLine + ex.StackTrace, Common.LoggerType.ERRO);
            }
            finally
            {

            }
        }

        protected override void OnStop()
        {
            try
            {
                MensagemLog("Parando serviço do Proxy de Discagem Silver", Common.LoggerType.INFO);
                MensagemLog("Serviço do Proxy de Discagem Silver parado com sucesso", Common.LoggerType.INFO);
            }
            catch (Exception ex)
            {
                MensagemLog("Houve uma falha no serviço. A mensagem do erro foi: " + ex.Message + Environment.NewLine + ex.StackTrace, Common.LoggerType.ERRO);
            }
            finally
            {

            }
        }

        protected override void OnShutdown()
        {
            try
            {
                base.OnShutdown();
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

        }

        protected override void OnContinue()
        {
            try
            {
                base.OnContinue();
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

        }

        protected override void OnPause()
        {
            try
            {
                base.OnPause();
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        protected override void OnCustomCommand(int command)
        {
            try
            {
                base.OnCustomCommand(command);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected void MensagemLog(string mensagem, Common.LoggerType tipo_log = Common.LoggerType.INFO, ConsoleColor color = ConsoleColor.White)
        {
            string path_logs = Path.Combine(ConfigurationManager.AppSettings["application.path.logs"], string.Format("Logs-ServicoProxyDiscagem-{0}.silver", DateTime.Now.ToString("ddMMyyyyHH")));
            Silver.Common.Logger.LoggerAsync.pathLog = path_logs;
            Silver.Common.Logger.LoggerAsync.WriteLog(new Common.Logger.LoggerMessage { MessageLogger = mensagem, TypeLogger = tipo_log }, color);
        }
    }
}
