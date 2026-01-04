using System;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Silver.UI.Web.Presentation.Pages
{
    public enum TipoMensagem
    {
        Alerta,
        Atencao,
        Confirmacao,
        Cuidado,
        Default,
        Erro
    }

    /// <summary>
    /// Desenvolvido por: Francisco Silva
    /// Data: 25/04/2013
    /// Centraliza todos os comportamentos comuns a todas as páginas
    /// </summary>
    public class PaginaBase : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            if (Session["Usuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }

            base.OnLoad(e);

            var header = (HtmlHead)Page.Header;

            ClientScript.RegisterClientScriptInclude(Guid.NewGuid().ToString(), Page.ResolveClientUrl("~/Js/jquery.js"));
            ClientScript.RegisterClientScriptInclude(Guid.NewGuid().ToString(), Page.ResolveClientUrl("~/Js/modernizr-2.6.2.js"));
            ClientScript.RegisterClientScriptInclude(Guid.NewGuid().ToString(), Page.ResolveClientUrl("~/Js/jquery.unobtrusive-ajax.js"));
            ClientScript.RegisterClientScriptInclude(Guid.NewGuid().ToString(), Page.ResolveClientUrl("~/Js/jquery.validate.js"));
            ClientScript.RegisterClientScriptInclude(Guid.NewGuid().ToString(), Page.ResolveClientUrl("~/Js/Plugs/jquery.ui/jquery-ui.js"));
            ClientScript.RegisterClientScriptInclude(Guid.NewGuid().ToString(), Page.ResolveClientUrl("~/Js/Silver.Common.js"));
            ClientScript.RegisterClientScriptInclude(Guid.NewGuid().ToString(), Page.ResolveClientUrl("~/Js/Silver.Dock.js"));
            ClientScript.RegisterClientScriptInclude(Guid.NewGuid().ToString(), Page.ResolveClientUrl("~/Js/Silver.Popup.js"));
            ClientScript.RegisterClientScriptInclude(Guid.NewGuid().ToString(), Page.ResolveClientUrl("~/Js/Silver.MensagemSistema.js"));
            ClientScript.RegisterClientScriptInclude(Guid.NewGuid().ToString(), Page.ResolveClientUrl("~/Js/jglowl/jquery.jgrowl.min.js"));
            ClientScript.RegisterClientScriptInclude(Guid.NewGuid().ToString(), Page.ResolveClientUrl("~/Js/jquery.maskedinput.js"));

            var btn = (ImageButton)Page.FindControl("imb_logout");
            if (btn != null)
            {
                btn.Attributes.Add("title", "Logout");
            }
            var lnk = (LinkButton)Page.FindControl("lnk_logout");
            if (lnk != null)
            {
                lnk.Attributes.Add("title", "Logout");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            var btn = (ImageButton)Page.FindControl("imb_logout");
            if (btn != null)
            {
                btn.Attributes.Add("title", "Logout");
            }
            var lnk = (LinkButton)Page.FindControl("lnk_logout");
            if (lnk != null)
            {
                lnk.Attributes.Add("title", "Logout");
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            var btn = (ImageButton)Page.FindControl("imb_logout");
            if (btn != null)
            {
                btn.Attributes.Add("title", "Logout");
            }
            var lnk = (LinkButton)Page.FindControl("lnk_logout");
            if (lnk != null)
            {
                lnk.Attributes.Add("title", "Logout");
            }
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            var btn = (ImageButton)Page.FindControl("imb_logout");
            if (btn != null)
            {
                btn.Attributes.Add("title", "Logout");
            }
            var lnk = (LinkButton)Page.FindControl("lnk_logout");
            if (lnk != null)
            {
                lnk.Attributes.Add("title", "Logout");
            }
        }
        
        protected override void OnError(EventArgs e)
        {
            Session["ExceptionSistema"] = Server.GetLastError();
            new Silver.Common.Logger.Logger(ConfigurationManager.AppSettings["Application.Paths.Log"]).WriteLog(Server.GetLastError().Message, Common.LoggerType.ERRO);
            Response.Redirect(Page.ResolveClientUrl("~/Error.aspx"));
            base.OnError(e);
        }
    }

    public static class ExtendsPage
    {
        public static void Logout(this Page value)
        {


            var usuarioSession = (DTO.Usuario)value.Session["Usuario"];
            new BLL.UsuarioLogin().RegistrarSaida(usuarioSession.Id);
            value.Session.RemoveAll();
            value.Response.Redirect(@"~/Login.aspx");
        }
    }
}
