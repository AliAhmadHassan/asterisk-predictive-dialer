using System;
using Silver.UI.Web.Presentation.Pages;

namespace Silver.UI.Web.Presentation
{
    public partial class PageNotFound : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.NomeAplicacao = "Não Encontrado";
            Master.DescricaoAplicacao = "Página de exibição de erro";

            var usuarioSession = (DTO.Usuario)Session["Usuario"];
            lbNome.Text = usuarioSession.Nome.ToString();
            lbRamal.Text = usuarioSession.Ramal.ToString();
        }
    }
}
