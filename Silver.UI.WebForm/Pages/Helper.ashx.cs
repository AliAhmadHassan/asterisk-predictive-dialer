using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Silver.UI.Web.Presentation.Requests;

namespace Silver.UI.Web.Presentation.Pages
{
    //TODO - Ignorar
    /// <summary>
    /// Summary description for Helper
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Helper : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string json = string.Empty;
            try
            {
                var usuarioSession = (DTO.Usuario)context.Session["Usuario"];
                //Todo -- Deu erro aqui -- usuarioSession = NULL
                if (usuarioSession == null)
                {
                    json = "{ \"responseUrl\": \"\",\"responseMsg\":\"" + "Sua sessão com o servidor foi perdida, favor logar novamente." + "\" }";
                    context.Response.Write(json);
                    return;
                }

                var url_usuario_logado = new BLL.UsuarioLogado().Obter(usuarioSession.Ramal);
                context.Response.ContentType = "application/json";
                if (!string.IsNullOrEmpty(url_usuario_logado.Url))
                {
                    json = "{ \"responseUrl\": \"" + url_usuario_logado.Url + "\", \"responseMsg\":\"" + url_usuario_logado.Url + "\", \"responseCliente\":\"" + url_usuario_logado.Contato + "\" }";
                    var aux1 = json.Split('?');
                    var aux2 = aux1[1].Split('&');
                    var aux_cpf = aux2[0];
                    new BLL.UsuarioLogado().Atualizar(new DTO.UsuarioLogado { Url = "", Ramal = usuarioSession.Ramal });
                }
                else
                    json = "{ \"responseUrl\": \"\",\"responseMsg\":\"" + "Nenhuma ligação disponível" + "\" }";

                context.Response.Write(json);
            }
            catch (Exception ex)
            {
                json = "{ \"responseUrl\": \"\",\"responseMsg\":\"" + ex.Message + "\" }";
                context.Response.Write(json);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}