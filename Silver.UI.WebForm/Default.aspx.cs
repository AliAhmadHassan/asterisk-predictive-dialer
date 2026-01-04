using System;
using Silver.UI.Web.Presentation.Pages;
using System.Configuration;

namespace Silver.UI.Web.Presentation.Comum
{
    public partial class Default1 : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.NomeAplicacao = "Pagina Inicial";
            Master.DescricaoAplicacao = "Selecione uma opção no menu";

            lbl_nome_aplicacao.Text = ConfigurationManager.AppSettings["Application.Name"];
            lbl_versao_aplicacao.Text = ConfigurationManager.AppSettings["Application.Version"];
            img_logotipo_aplicacao.ImageUrl = ConfigurationManager.AppSettings["Application.Logotipo.Max.Path"];
            lbl_copyright_aplicacao.Text = String.Format(ConfigurationManager.AppSettings["Application.Copyright"], DateTime.Now.Year);
            
            if (Server.GetLastError() != null)
                lblMensagem.Text = Server.GetLastError().Message;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }
    }
}
