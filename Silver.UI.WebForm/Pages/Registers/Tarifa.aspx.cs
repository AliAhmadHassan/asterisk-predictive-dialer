using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Silver.Common;

namespace Silver.UI.Web.Presentation.Pages.Registers
{
    public partial class Tarifa : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.NomeAplicacao = "Tarifa";
            Master.DescricaoAplicacao = "Configuração e manutenção de Tarifa";

            if (!IsPostBack)
            {
                tbValor.Attributes.Add("onkeypress", "if ((event.keyCode < 48 || event.keyCode > 58) && event.keyCode != 44) {event.keyCode = 0;}");

                if (Session["Usuario"] != null)
                {
                    var usuarioSession = (DTO.Usuario)Session["Usuario"];
                    lbNome.Text = usuarioSession.Nome.ToString();
                    lbRamal.Text = usuarioSession.Ramal.ToString();
                }
                else
                {
                    Response.Redirect("../Login.aspx");
                }
                PopularCampos(0, 0, 0);
                GridViewObjeto_Preencher(GridViewObjeto.PageIndex);
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
            var Tarifa = new BLL.Tarifa();

            if (string.IsNullOrEmpty(filtro))
                GridViewObjeto.DataSource = Tarifa.Listar();
            else
                GridViewObjeto.DataSource = Tarifa.Buscar(filtro);

            GridViewObjeto.DataBind();
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            var Tarifa = new BLL.Tarifa();
            GridViewObjeto.DataSource = Tarifa.Buscar(tbBuscar.Text);
            GridViewObjeto.DataBind();
        }

        protected void btNovo_Click(object sender, EventArgs e)
        {
            LimparCampos();
            PopularCampos(0, 0, 0);
            Panel_Cadastrar.Visible = true;
        }

        protected void Cadastrar(int intID)
        {
            LimparCampos();
            var Tarifa = new BLL.Tarifa().Obter(intID);
            IdCadastrar.Value = Tarifa.Id.ToString();
            tbDescricao.Text = Tarifa.Descricao;
            PopularCampos(int.Parse(Tarifa.IdTarifaTipo.ToString()), int.Parse(Tarifa.IdTarifaRegra.ToString()), int.Parse(Tarifa.IdOperadora.ToString()));
            tbValor.Text = Tarifa.Valor.ToString();
            if (Tarifa.Ativo)
            {
                cbAtivo.Checked = true;
            }
            Panel_Cadastrar.Visible = true;
            Panel_Cadastrar_ModalPopupExtender.Show();
        }

        protected void LimparCampos()
        {
            IdCadastrar.Value = string.Empty;
            tbDescricao.Text = string.Empty;
            lstTarifaTipo.ClearSelection();
            lstTarifaRegra.ClearSelection();
            lstOperadora.ClearSelection();
            tbValor.Text = string.Empty;
            cbAtivo.Checked = false;
        }

        protected void PopularCampos(int IdTarifaTipo, int IdTarifaRegra, int IdOperadora)
        {
            lstTarifaTipo.DataSource = new BLL.TarifaTipo().Obter(true);
            lstTarifaTipo.DataTextField = "Descricao";
            lstTarifaTipo.DataValueField = "Id";
            lstTarifaTipo.DataBind();
            if (IdTarifaTipo > 0)
            {
                lstTarifaTipo.SelectedValue = IdTarifaTipo.ToString();
            }
            lstTarifaRegra.DataSource = new BLL.TarifaRegra().Obter(true);
            lstTarifaRegra.DataTextField = "Descricao";
            lstTarifaRegra.DataValueField = "Id";
            lstTarifaRegra.DataBind();
            if (IdTarifaRegra > 0)
            {
                lstTarifaRegra.SelectedValue = IdTarifaRegra.ToString();
            }
            lstOperadora.DataSource = new BLL.Operadora().Obter(true);
            lstOperadora.DataTextField = "Descricao";
            lstOperadora.DataValueField = "Id";
            lstOperadora.DataBind();
            if (IdOperadora > 0)
            {
                lstOperadora.SelectedValue = IdOperadora.ToString();
            }
        }

        protected void btCadastrar_Sim_Click(object sender, EventArgs e)
        {
            var DTOCadastrar = new DTO.Tarifa();
            var BLLCadastrar = new BLL.Tarifa();
            Lb_Mensagem.Text = string.Empty;

            if (IdCadastrar.Value != string.Empty)
            {
                DTOCadastrar = BLLCadastrar.Obter(int.Parse(IdCadastrar.Value));
            }
            DTOCadastrar.Descricao = tbDescricao.Text;
            DTOCadastrar.IdTarifaTipo = Convert.ToInt64(lstTarifaTipo.SelectedValue);
            DTOCadastrar.IdTarifaRegra = Convert.ToInt64(lstTarifaRegra.SelectedValue);
            DTOCadastrar.IdOperadora = Convert.ToInt64(lstOperadora.SelectedValue);
            DTOCadastrar.Valor = Convert.ToDecimal(tbValor.Text);
            DTOCadastrar.Ativo = cbAtivo.Checked;

            BLLCadastrar.Cadastrar(DTOCadastrar);

            LimparCampos();
            Panel_Cadastrar.Visible = false;
            Lb_Mensagem.Text = "Item cadastrado com sucesso";
            Panel_Mensagem.Visible = true;
        }

        protected void btCadastrar_Nao_Click(object sender, EventArgs e)
        {
            LimparCampos();
            Panel_Cadastrar.Visible = false;
        }

        protected void Excluir(int intExcluir)
        {
            IdExcluir.Value = intExcluir.ToString();
            Panel_Excluir.Visible = true;
        }

        protected void btExcluir_Sim_Click(object sender, EventArgs e)
        {
            var Tarifa = new BLL.Tarifa();
            Tarifa.Ativar(int.Parse(IdExcluir.Value), false);
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

        protected void btBuscar_Click(object sender, ImageClickEventArgs e)
        {
            if (string.IsNullOrEmpty(tbBuscar.Text)) return;
            GridViewObjeto_Preencher(0, tbBuscar.Text.Trim());
        }

        protected void GridViewObjeto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }
            if (e.Row.Cells[8].Text.Trim().Equals("False"))
            {
                e.Row.Cells[8].Text = "Não";
            }
            else
            {
                e.Row.Cells[8].Text = "Sim";
            }
            e.Row.Cells[4].Text = new BLL.TarifaTipo().Obter(e.Row.Cells[4].Text.ToInt32()).Descricao;

            e.Row.Cells[5].Text = new BLL.TarifaRegra().Obter(e.Row.Cells[5].Text.ToInt32()).Descricao;

            e.Row.Cells[6].Text = new BLL.Operadora().Obter(e.Row.Cells[6].Text.ToInt32()).Descricao;
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
