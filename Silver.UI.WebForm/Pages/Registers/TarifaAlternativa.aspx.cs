using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Silver.Common;

namespace Silver.UI.Web.Presentation.Pages.Registers
{
    public partial class TarifaAlternativa : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.NomeAplicacao = "Tarifa Alternativa";
            Master.DescricaoAplicacao = "Configuração e manutenção de Tarifa Alternativa";

            if (!IsPostBack)
            {
                tbValor.Attributes.Add("onkeypress", "if ((event.keyCode < 48 || event.keyCode > 58) && event.keyCode != 44) {event.keyCode = 0;}");
                var usuarioSession = (DTO.Usuario)Session["Usuario"];
                lbNome.Text = usuarioSession.Nome.ToString();
                lbRamal.Text = usuarioSession.Ramal.ToString();
                GridViewObjeto_Preencher(GridViewObjeto.PageIndex);
                PopularCampos(0, 0, 0, 0);
            }
        }

        protected void GridViewObjeto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewObjeto_Preencher(e.NewPageIndex);
        }

        protected void GridViewObjeto_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Alterar")
            {
                Cadastrar(int.Parse(GridViewObjeto.Rows[int.Parse(e.CommandArgument.ToString())].Cells[2].Text.Replace("&nbsp;", string.Empty)));
            }
            if (e.CommandName == "Excluir")
            {
                Excluir(int.Parse(GridViewObjeto.Rows[int.Parse(e.CommandArgument.ToString())].Cells[2].Text.Replace("&nbsp;", string.Empty)));
            }
        }

        protected void GridViewObjeto_Preencher(int intPageIndex, string filtro = "")
        {
            GridViewObjeto.PageIndex = intPageIndex;
            var TarifaAlternativa = new BLL.TarifaAlternativa();
            
            if (string.IsNullOrEmpty(filtro))
                GridViewObjeto.DataSource = TarifaAlternativa.Listar();
            else
                GridViewObjeto.DataSource = TarifaAlternativa.Buscar(filtro);

            GridViewObjeto.DataBind();
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            var TarifaAlternativa = new BLL.TarifaAlternativa();
            GridViewObjeto.DataSource = TarifaAlternativa.Buscar(tbBuscar.Text);
            GridViewObjeto.DataBind();
        }

        protected void btNovo_Click(object sender, EventArgs e)
        {
            LimparCampos();
            PopularCampos(0, 0, 0, 0);
        }

        protected void Cadastrar(int intID)
        {
            LimparCampos();
            var TarifaAlternativa = new BLL.TarifaAlternativa().Obter(intID);
            IdCadastrar.Value = TarifaAlternativa.Id.ToString();
            tbDescricao.Text = TarifaAlternativa.Descricao;
            PopularCampos(int.Parse(TarifaAlternativa.IdTarifaTipo.ToString()), int.Parse(TarifaAlternativa.IdTarifaRegra.ToString()), int.Parse(TarifaAlternativa.IdOperadora.ToString()), int.Parse(TarifaAlternativa.IdCampanha.ToString()));
            tbValor.Text = TarifaAlternativa.Valor.ToString();

            if (TarifaAlternativa.Ativo)
            {
                cbAtivo.Checked = true;
            }
            Panel_Cadastrar_ModalPopupExtender.Show();
        }

        protected void LimparCampos()
        {
            IdCadastrar.Value = string.Empty;
            tbDescricao.Text = string.Empty;
            lstTarifaTipo.ClearSelection();
            lstTarifaRegra.ClearSelection();
            lstOperadora.ClearSelection();
            lstCampanha.ClearSelection();
            tbValor.Text = string.Empty;
            cbAtivo.Checked = false;
        }

        protected void PopularCampos(int IdTarifaTipo, int IdTarifaRegra, int IdOperadora, int IdCampanha)
        {
            lstTarifaTipo.DataSource = new BLL.TarifaTipo().Obter(true);
            lstTarifaTipo.DataTextField = "Descricao";
            lstTarifaTipo.DataValueField = "Id";
            lstTarifaTipo.DataBind();
            if (IdTarifaTipo > 0)
            {
                if ((lstTarifaTipo.Items.FindByValue(IdTarifaTipo.ToString()) != null))
                {
                    lstTarifaTipo.SelectedValue = IdTarifaTipo.ToString();
                }
            }
            lstTarifaRegra.DataSource = new BLL.TarifaRegra().Obter(true);
            lstTarifaRegra.DataTextField = "Descricao";
            lstTarifaRegra.DataValueField = "Id";
            lstTarifaRegra.DataBind();
            if (IdTarifaRegra > 0)
            {
                if ((lstTarifaRegra.Items.FindByValue(IdTarifaRegra.ToString()) != null))
                {
                    lstTarifaRegra.SelectedValue = IdTarifaRegra.ToString();
                }
            }
            lstOperadora.DataSource = new BLL.Operadora().Obter(true);
            lstOperadora.DataTextField = "Descricao";
            lstOperadora.DataValueField = "Id";
            lstOperadora.DataBind();
            if (IdOperadora > 0)
            {
                if ((lstOperadora.Items.FindByValue(IdOperadora.ToString()) != null))
                {
                    lstOperadora.SelectedValue = IdOperadora.ToString();
                }
            }
            lstCampanha.DataSource = new BLL.Campanha().Obter(true);
            lstCampanha.DataTextField = "Nome";
            lstCampanha.DataValueField = "Id";
            lstCampanha.DataBind();
            if (IdCampanha > 0)
            {
                if ((lstCampanha.Items.FindByValue(IdCampanha.ToString()) != null))
                {
                    lstCampanha.SelectedValue = IdCampanha.ToString();
                }
            }
        }

        protected void btCadastrar_Sim_Click(object sender, EventArgs e)
        {
            var DTOCadastrar = new DTO.TarifaAlternativa();
            var BLLCadastrar = new BLL.TarifaAlternativa();
            Lb_Mensagem.Text = string.Empty;

            if (IdCadastrar.Value != string.Empty)
            {
                DTOCadastrar = BLLCadastrar.Obter(int.Parse(IdCadastrar.Value));
            }
            DTOCadastrar.Descricao = tbDescricao.Text;
            DTOCadastrar.IdTarifaTipo = Convert.ToInt64(lstTarifaTipo.SelectedValue);
            DTOCadastrar.IdTarifaRegra = Convert.ToInt64(lstTarifaRegra.SelectedValue);
            DTOCadastrar.IdOperadora = Convert.ToInt64(lstOperadora.SelectedValue);
            DTOCadastrar.IdCampanha = Convert.ToInt64(lstCampanha.SelectedValue);
            DTOCadastrar.Valor = Convert.ToDecimal(tbValor.Text);
            DTOCadastrar.Ativo = cbAtivo.Checked;

            BLLCadastrar.Cadastrar(DTOCadastrar);

            LimparCampos();
            Lb_Mensagem.Text = "Item cadastrado com sucesso";
            Panel_Mensagem.Visible = true;
        }

        protected void btCadastrar_Nao_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        protected void Excluir(int intExcluir)
        {
            IdExcluir.Value = intExcluir.ToString();
            Panel_Excluir.Visible = true;
        }

        protected void btExcluir_Sim_Click(object sender, EventArgs e)
        {
            var TarifaAlternativa = new BLL.TarifaAlternativa();
            TarifaAlternativa.Ativar(int.Parse(IdExcluir.Value), false);
            Panel_Excluir.Visible = false;
            Lb_Mensagem.Text = "Item inativado com sucesso";
            Panel_Mensagem.Visible = true;
        }

        protected void btExcluir_Nao_Click(object sender, EventArgs e)
        {
            Panel_Excluir.Visible = false;
        }

        protected void btMensagem_Click(object sender, EventArgs e)
        {
            GridViewObjeto_Preencher(GridViewObjeto.PageIndex);
            Panel_Mensagem.Visible = false;
        }

        protected void btSair_Click(object sender, EventArgs e)
        {
            var usuarioSession = (DTO.Usuario)Session["Usuario"];
            new BLL.UsuarioLogin().RegistrarSaida(usuarioSession.Id);
            Session.RemoveAll();
            Response.Redirect("../Login.aspx");
        }

        protected void btNovo_Click(object sender, ImageClickEventArgs e)
        {
        }

        protected void GridViewObjeto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }
            if (e.Row.Cells[9].Text.Trim().Equals("False"))
            {
                e.Row.Cells[9].Text = "Não";
            }
            else
            {
                e.Row.Cells[9].Text = "Sim";
            }
            e.Row.Cells[4].Text = new BLL.TarifaTipo().Obter(e.Row.Cells[4].Text.ToInt32()).Descricao;

            e.Row.Cells[5].Text = new BLL.TarifaRegra().Obter(e.Row.Cells[5].Text.ToInt32()).Descricao;

            e.Row.Cells[6].Text = new BLL.Operadora().Obter(e.Row.Cells[6].Text.ToInt32()).Descricao;

            e.Row.Cells[7].Text = new BLL.Campanha().Obter(e.Row.Cells[7].Text.ToInt32()).Descricao;
        }

        protected void imb_logout_Click(object sender, ImageClickEventArgs e)
        {
            this.Logout();
        }

        protected void lnk_logout_Click(object sender, EventArgs e)
        {
            this.Logout();
        }

        protected void btBuscar_Click(object sender, ImageClickEventArgs e)
        {
            if (string.IsNullOrEmpty(tbBuscar.Text)) return;
            GridViewObjeto_Preencher(0, tbBuscar.Text.Trim());
        }
    }
}
