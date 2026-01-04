using System;
using System.Collections.Generic;
using System.Linq;
using Silver.UI.Web.Presentation.Pages;

namespace Silver.UI.Web.Presentation
{
    public partial class Error : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.NomeAplicacao = "Erro";
            Master.DescricaoAplicacao = "Página de exibição de erro";

            var usuarioSession = (DTO.Usuario)Session["Usuario"];
            lbNome.Text = usuarioSession.Nome.ToString();
            lbRamal.Text = usuarioSession.Ramal.ToString();

            if (Session["ExceptionSistema"] != null)
            {
                lblMensagem.Text ="Mensagem:<hr>"+  (Session["ExceptionSistema"] as Exception).Message;
                lblMensagem.Text += "<br><br>Stack Trace:<hr>" + (Session["ExceptionSistema"] as Exception).StackTrace;
                Session.Remove("ExceptionSistema");
            }
        }
    }
}
