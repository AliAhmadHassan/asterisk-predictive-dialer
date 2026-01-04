using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Silver.UI.Web.Presentation.Requests;
using System.Text;

namespace Silver.UI.Web.Presentation
{
    //TODO - Ignorar
    /// <summary>
    /// Summary description for Helper
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class NotificacaoSistema : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string json = string.Empty;
            try
            {
                var usuarioSession = (DTO.Usuario)context.Session["Usuario"];
                if (usuarioSession == null)
                {
                    json = "{ \"responseUrl\": \"\",\"responseMsg\":\"" + "Sua sessão com o servidor foi perdida, favor logar novamente." + "\" }";
                    context.Response.Write(json);
                    return;
                }
                context.Response.ContentType = "application/json";
                if (usuarioSession != null)
                {
                    var mensagen_nao_visualizadas = Silver.BLL.MensagemSistema.ListarNaoVisualizadas(usuarioSession.IdCampanha);

                    StringBuilder mensagem_formatada = new StringBuilder();
                    foreach (var m in mensagen_nao_visualizadas)
                    {
                        if (!Silver.BLL.MensagemSistemaVisualizado.ExisteVisualizacao(m.Id, usuarioSession.Id))
                        {
                            var url = string.Format("http://localhost:60522/NotificacaoSistema.aspx?id={0}", m.Id);
                            var url_visualizacao = string.Format("<a href='{0}' target='_blank' style='font-size:10px; cursor:pointer' onclick='javascript:$(this).html('Visualizado');'>Marcar como lida</a>", url);
                            mensagem_formatada.Append(m.Mensagem + "<br><br>" + url_visualizacao + "|");
                        }
                    }

                    json = "{ \"responseUrl\": \"\",\"responseMsg\":\"" + mensagem_formatada.ToString() + "\" }";
                }
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