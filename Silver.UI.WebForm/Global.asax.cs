using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Configuration;
using Silver.Common;

namespace Silver.UI.Web.Presentation
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            MensagemLog("Aplicação iniciada com sucesso", LoggerType.INFO);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            MensagemLog(string.Format("Sessão Iniciada, ID: {0}", Session.SessionID), LoggerType.INFO);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (Server.GetLastError() != null)
                MensagemLog(string.Format("Houve uma falha na aplicação. A mensagem de erro foi: {0}", Server.GetLastError().Message), LoggerType.ERRO);
        }

        protected void Session_End(object sender, EventArgs e)
        {
            if (Session != null)
            {
                if (Session["Usuario"] != null)
                {
                    MensagemLog(string.Format("Sessão Finalizada, ID: {0}, OPERADOR: {0}", Session.SessionID, (Session["Usuario"] as DTO.Usuario).Nome), LoggerType.INFO);
                    new BLL.UsuarioLogado().Remover(new DTO.UsuarioLogado
                    {
                        Ramal = (Session["Usuario"] as DTO.Usuario).Ramal,
                        Url = ""
                    });
                }
                else
                {
                    MensagemLog("Falha no Session_End, Session[\"Usuario\"] = null", LoggerType.INFO);
                }
            }
            else
            {
                MensagemLog("Falha no Session_End, Session = null", LoggerType.INFO);
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            MensagemLog("Aplicação finalizada com sucesso", Common.LoggerType.INFO);
        }

        protected void MensagemLog(string mensagem, Common.LoggerType tipo_log = Common.LoggerType.INFO, ConsoleColor color = ConsoleColor.White)
        {
            string path_logs = Path.Combine(ConfigurationManager.AppSettings["application.path.logs"], string.Format("Logs-{0}-{1}.silver", "AplicacaoWEB", DateTime.Now.ToString("ddMMyyyyHH")));
            Silver.Common.Logger.LoggerAsync.pathLog = path_logs;
            Silver.Common.Logger.LoggerAsync.WriteLog(new Common.Logger.LoggerMessage { MessageLogger = mensagem, TypeLogger = tipo_log }, color);
        }
    }
}
