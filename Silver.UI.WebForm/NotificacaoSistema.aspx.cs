using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Silver.Common;

namespace Silver.UI.Web.Presentation
{
    public partial class NotificacaoSistema1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
                Response.Redirect("~/Login.aspx");

            if (!string.IsNullOrEmpty(Request.QueryString.Get("id")))
            {
                var query_string = Request.QueryString.Get("id").ToInt64();
                var id_usuario = (Session["Usuario"] as DTO.Usuario).Id;
                Silver.BLL.MensagemSistema.MarcarComoVisualizada(query_string);
                Silver.BLL.MensagemSistemaVisualizado.Cadastrar(new DTO.MensagemSistemaVisualizado() { DataHora = new DateTime(), IdMensagem = query_string, IdUsuario = id_usuario });
                Page.ClientScript.RegisterClientScriptInclude(typeof(Page), "closePage", "window.close();");

            }
        }
    }
}