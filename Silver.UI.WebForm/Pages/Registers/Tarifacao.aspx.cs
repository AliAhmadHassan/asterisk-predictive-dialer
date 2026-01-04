using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Silver.UI.Web.Presentation.Pages.Registers
{
    public partial class Tarifacao : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.NomeAplicacao = "Tarifação";
                Master.DescricaoAplicacao = "Configuração e manutenção de tarifações";

                var usuarioSession = (DTO.Usuario)Session["Usuario"];
                lbNome.Text = usuarioSession.Nome;
                lbRamal.Text = usuarioSession.Ramal.ToString();

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
            var Tarifacao = new BLL.Tarifacao();

            if (string.IsNullOrEmpty(filtro))
                GridViewObjeto.DataSource = Tarifacao.Listar();
            else
                GridViewObjeto.DataSource = Tarifacao.Buscar(filtro);

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
            var Tarifacao = new BLL.Tarifacao().Obter(intID);
            PopularCampos(int.Parse(Tarifacao.IdBilhetagem.ToString()), int.Parse(Tarifacao.IdTarifa.ToString()), int.Parse(Tarifacao.IdTarifaAlternativa.ToString()));
            if (Tarifacao.Ativo)
            {
                cbAtivo.Checked = true;
            }
            Panel_Cadastrar.Visible = true;
            Panel_Cadastrar_ModalPopupExtender.Show();
        }

        protected void LimparCampos()
        {
            IdCadastrar.Value = string.Empty;
            lstBilhetagem.ClearSelection();
            lstTarifa.ClearSelection();
            tbValor.Text = string.Empty;
            lstTarifaAlternativa.ClearSelection();
            tbValorAlternativo.Text = string.Empty;
            cbAtivo.Checked = false;
        }

        protected void PopularCampos(int IdBilhetagem, int IdTarifa, int IdTarifaAlternativa)
        {
            lstBilhetagem.DataSource = new BLL.Bilhetagem().Obter(true);
            lstBilhetagem.DataTextField = "Id";
            lstBilhetagem.DataValueField = "Id";
            lstBilhetagem.DataBind();
            if (IdBilhetagem > 0)
            {
                lstBilhetagem.SelectedValue = IdBilhetagem.ToString();
            }
            lstTarifa.DataSource = new BLL.Tarifa().Obter(true);
            lstTarifa.DataTextField = "Descricao";
            lstTarifa.DataValueField = "Id";
            lstTarifa.DataBind();
            if (IdTarifa > 0)
            {
                lstTarifa.SelectedValue = IdTarifa.ToString();
                tbValor.Text = new BLL.Tarifa().Obter(IdTarifa).Valor.ToString("0.00");
            }


            lstTarifaAlternativa.DataSource = new BLL.TarifaAlternativa().Obter(true);
            lstTarifaAlternativa.DataTextField = "Descricao";
            lstTarifaAlternativa.DataValueField = "Id";
            lstTarifaAlternativa.DataBind();
            if (IdTarifa > 0)
            {
                lstTarifaAlternativa.SelectedValue = IdTarifaAlternativa.ToString();
                tbValorAlternativo.Text = new BLL.TarifaAlternativa().Obter(IdTarifaAlternativa).Valor.ToString("0.00");
            }
        }

        protected void lstTarifa_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbValor.Text = new BLL.Tarifa().Obter(int.Parse(lstTarifa.SelectedValue)).Valor.ToString("0.00");
        }

        protected void lstTarifaAlternativa_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbValorAlternativo.Text = new BLL.TarifaAlternativa().Obter(int.Parse(lstTarifaAlternativa.SelectedValue)).Valor.ToString("0.00");
        }

        protected void btCadastrar_Sim_Click(object sender, EventArgs e)
        {
            var DTOCadastrar = new DTO.Tarifacao();
            var BLLCadastrar = new BLL.Tarifacao();
            Lb_Mensagem.Text = string.Empty;

            if (IdCadastrar.Value != string.Empty)
            {
                DTOCadastrar = BLLCadastrar.Obter(int.Parse(IdCadastrar.Value));
            }
            DTOCadastrar.IdBilhetagem = Convert.ToInt64(lstBilhetagem.SelectedValue);
            DTOCadastrar.IdTarifa = Convert.ToInt64(lstTarifa.SelectedValue);
            DTOCadastrar.Valor = Convert.ToDecimal(tbValor.Text);
            DTOCadastrar.IdTarifaAlternativa = Convert.ToInt64(lstTarifaAlternativa.SelectedValue);
            DTOCadastrar.ValorAlternativo = Convert.ToDecimal(tbValorAlternativo.Text);
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
            var Tarifacao = new BLL.Tarifacao();
            Tarifacao.Ativar(int.Parse(IdExcluir.Value), false);
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

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            var Tarifa = new BLL.Tarifa();
            GridViewObjeto.DataSource = Tarifa.Buscar(tbBuscar.Text);
            GridViewObjeto.DataBind();
        }

        protected void btBuscar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (string.IsNullOrEmpty(tbBuscar.Text)) return;
            GridViewObjeto_Preencher(0, tbBuscar.Text.Trim());
        }

        protected void imb_logout_Click(object sender, ImageClickEventArgs e)
        {
            this.Logout();
        }

        protected void btNovo_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}
