using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Silver.Common;
using Silver.DTO;
using System.Drawing;

namespace Silver.UI.Web.Presentation.Pages
{
    public partial class HistoricoRequisicao : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.NomeAplicacao = "Histórico de Requisição";
            Master.DescricaoAplicacao = "Histórico de solicitação de processamento";

            var usuarioSession = (DTO.Usuario)Session["Usuario"];

            lbNome.Text = usuarioSession.Nome.ToString();
            lbRamal.Text = usuarioSession.Ramal.ToString();
            CarregarGrid();
        }

        private void CarregarGrid()
        {
            grid_historico.DataSource = Silver.BLL.ControleSistema.ListarTodosControles((Session["Usuario"] as DTO.Usuario).Id).OrderByDescending(c => c.Id).ToList();
            grid_historico.DataBind();
        }

        protected void btBuscar_Click(object sender, ImageClickEventArgs e)
        {

        }


        protected void imb_logout_Click(object sender, ImageClickEventArgs e)
        {
            this.Logout();
        }

        protected void lnk_logout_Click(object sender, EventArgs e)
        {
            this.Logout();
        }

        protected void grid_historico_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grid_historico.PageIndex = e.NewPageIndex;
            CarregarGrid();
        }

        protected void grid_historico_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            e.Row.Cells[3].Text = Enum.GetName(typeof(SitucaoEventoControleSistema), e.Row.Cells[3].Text.ToInt32()).Replace('_', ' ');
            if (e.Row.Cells[3].Text == "Executando")
                e.Row.BackColor = Color.LightGreen;
        }

    }
}