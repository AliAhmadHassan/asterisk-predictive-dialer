using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Silver.Common;

namespace Silver.UI.Web.Presentation.Pages.Registers
{
    public partial class Pausa : PaginaBase
    {
        protected void PreencherDataGrid(int page_index = 0, string filtro = "")
        {
            BLL.Pausa result = new BLL.Pausa();

            if (!string.IsNullOrEmpty(filtro))
                grid.DataSource = result.Obter(filtro);
            else
                grid.DataSource = result.Listar();
            
            grid.PageIndex = page_index;
            grid.DataBind();
        }

        protected void LimparCampos()
        {
            tbDescricao.Text = string.Empty;
            cbAtivo.Checked = false;
            tbDescricao.Focus();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btNovo.OnClientClick = string.Format("$find({0}).text('')", tbDescricao.ClientID);
            if (!IsPostBack)
            {
                Master.NomeAplicacao = "Pausa";
                Master.DescricaoAplicacao = "Configuração e manutenção de pausas";

                var usuarioSession = (DTO.Usuario)Session["Usuario"];
                lbNome.Text = usuarioSession.Nome;
                lbRamal.Text = usuarioSession.Ramal.ToString();

                PreencherDataGrid();
            }
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }
            if (e.Row.Cells[4].Text.Trim().Equals("False"))
            {
                e.Row.Cells[4].Text = "Não";
            }
            else
            {
                e.Row.Cells[4].Text = "Sim";
            }
            var delete = (ImageButton)e.Row.FindControl("img_excluir");
            delete.Attributes.Add("onclick", string.Format("javascript:return confirm('Deseja excluir a pausa {0}?')", e.Row.Cells[3].Text.ToUpper()));

            var atualizar = (ImageButton)e.Row.FindControl("img_alterar");
            atualizar.OnClientClick = string.Format("$find({0}).show()", Panel_Cadastrar_ModalPopupExtender.PopupControlID);
        }

        protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PreencherDataGrid(e.NewPageIndex);
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToLower())
            {
                case "alterar":

                    LimparCampos();
                    var pausa = new BLL.Pausa().Obter(e.CommandArgument.ToInt32());
                    tbDescricao.Text = pausa.Descricao;
                    cbAtivo.Checked = pausa.Ativo;
                    ViewState["CodigoPausa"] = e.CommandArgument;
                    Panel_Cadastrar_ModalPopupExtender.Show();

                    break;
                case "excluir":
                    new BLL.Pausa().Ativar(e.CommandArgument.ToInt64(), false);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "javascript:alert('O registro foi desativado com sucesso')", true);
                    PreencherDataGrid(grid.PageIndex);
                    break;
            }
        }

        protected void btNovo_Click(object sender, ImageClickEventArgs e)
        {
        }

        protected void btBuscar_Click(object sender, ImageClickEventArgs e)
        {
            if (string.IsNullOrEmpty(tbBuscar.Text)) return;
            PreencherDataGrid(0, tbBuscar.Text.Trim());
        }

        protected void btSalvarCadastro_Click(object sender, EventArgs e)
        {
            if (ViewState["CodigoPausa"] == null)
            {
                new BLL.Pausa().Cadastro(new DTO.Pausa() { Ativo = cbAtivo.Checked, Descricao = tbDescricao.Text.Trim().ToUpper() });
                ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "javascript:alert('O registro foi cadastrado com sucesso')", true);
            }
            else
            {
                new BLL.Pausa().Cadastro(new DTO.Pausa() { Ativo = cbAtivo.Checked, Descricao = tbDescricao.Text.Trim().ToUpper(), Id = ViewState["CodigoPausa"].ToInt64() });
                ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "javascript:alert('O registro foi atualizado com sucesso')", true);
            }
            LimparCampos();
            ViewState["CodigoPausa"] = null;
            PreencherDataGrid(grid.PageIndex);
        }

        protected void btCancelarCadastro_Click(object sender, EventArgs e)
        {
            ViewState["CodigoPausa"] = null;
        }

        protected void imb_logout_Click(object sender, ImageClickEventArgs e)
        {
            this.Logout();
        }

        protected void lnk_logout_Click(object sender, EventArgs e)
        {
            this.Logout();
        }
    }
}
