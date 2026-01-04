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

namespace Silver.ServerEscuta
{
    public enum TipoLog
    {
        INFO = 100,
        ERRO = 200
    }

    public partial class SilverServerEscutaRamalDesligou : ServiceBase
    {
        private int qtderros_instancia = 0;
        private Silver.AsteriskClient.AsteriskListener escuta_asterisk;

        public SilverServerEscutaRamalDesligou()
        {
            InitializeComponent();
        }

        public void Inicializar()
        {
            qtderros_instancia = 0;
            if (escuta_asterisk != null)
                escuta_asterisk.Dispose();

            escuta_asterisk = new AsteriskClient.AsteriskListener();
            escuta_asterisk.SaidaPadrao = AsteriskClient.SaidaPadraoAsterisk.BancoDados;
            escuta_asterisk.IniciarEscuta();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Logs(Common.LoggerType.INFO, "Executando configurações iniciais da escuta com Asterisk");
                Inicializar();
                Logs(Common.LoggerType.INFO, "Serviço de Escuta iniciado com sucesso. Aguardando eventos");
            }
            catch (Exception ex)
            {
                Logs(Common.LoggerType.ERRO, ex.Message + Environment.NewLine + ex.StackTrace);
                if (qtderros_instancia == Convert.ToInt32(ConfigurationManager.AppSettings["Application.Erros.Max"]))
                {
                    Logs(Common.LoggerType.INFO, "O serviço de escuta foi parado por atingir seu limite de erros em tempo de execução");
                    OnStop();
                    return;
                }

                qtderros_instancia++;
                Inicializar();
            }
        }

        protected override void OnStop()
        {
            if (escuta_asterisk != null)
                escuta_asterisk.Dispose();

            Logs(Common.LoggerType.INFO, "O serviço de escuta com Asterisk foi parado");
        }

        public void Logs(Common.LoggerType tipolog, string msg)
        {
            string path_logs = Path.Combine(ConfigurationManager.AppSettings["application.path.logs"], string.Format("Logs-{0}_Hora-{1}.log", "ServerEscuta", DateTime.Now.ToString("ddMMyyyyHH")));
            Silver.Common.Logger.LoggerAsync.pathLog = path_logs;
            Silver.Common.Logger.LoggerAsync.WriteLog(new Common.Logger.LoggerMessage { MessageLogger = msg, TypeLogger = tipolog });

            Console.Out.WriteLine(msg);
        }
    }
}
