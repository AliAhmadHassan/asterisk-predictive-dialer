using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Silver.Common;

namespace Silver.UI.Web.Presentation.Pages.Registers
{
    public partial class E1 : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.NomeAplicacao = "E1";
            Master.DescricaoAplicacao = "Criação e manutenção de E1";

            if (!IsPostBack)
            {
                var usuarioSession = (DTO.Usuario)Session["Usuario"];
                lbNome.Text = usuarioSession.Nome.ToString();
                lbRamal.Text = usuarioSession.Ramal.ToString();
                GridViewObjeto_Preencher(GridViewObjeto.PageIndex);
                PopularCampos(0, 0);
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
            var E1 = new BLL.E1();

            if (string.IsNullOrEmpty(filtro))
                GridViewObjeto.DataSource = E1.Listar();
            else
                GridViewObjeto.DataSource = E1.Buscar(filtro);
            GridViewObjeto.DataBind();
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            var E1 = new BLL.E1();
            GridViewObjeto.DataSource = E1.Buscar(tbBuscar.Text);
            GridViewObjeto.DataBind();
        }

        protected void btNovo_Click(object sender, EventArgs e)
        {
            LimparCampos();
            PopularCampos(0, 0);
        }

        protected void Cadastrar(int intID)
        {
            LimparCampos();
            var E1 = new BLL.E1().Obter(intID);
            IdCadastrar.Value = E1.Id.ToString();
            tbDescricao.Text = E1.Descricao;
            PopularCampos(int.Parse(E1.IdServidor.ToString()), int.Parse(E1.IdOperadora.ToString()));
            tbPlaca.Text = E1.Placa.ToString();
            tbPosicao.Text = E1.Posicao.ToString();
            tbContrato.Text = E1.Contrato.ToString();
            if (E1.Ativo)
            {
                cbAtivo.Checked = true;
            }
            Panel_Cadastrar_ModalPopupExtender.Show();
        }

        protected void LimparCampos()
        {
            IdCadastrar.Value = string.Empty;
            tbDescricao.Text = string.Empty;
            lstServidor.ClearSelection();
            lstOperadora.ClearSelection();
            tbPlaca.Text = string.Empty;
            tbPosicao.Text = string.Empty;
            tbContrato.Text = string.Empty;
            cbAtivo.Checked = false;
        }

        protected void PopularCampos(int IdServidor, int IdOperadora)
        {
            lstServidor.DataSource = new BLL.Servidor().Obter(true);
            lstServidor.DataTextField = "Descricao";
            lstServidor.DataValueField = "Id";
            lstServidor.DataBind();
            if (IdServidor > 0)
            {
                lstServidor.SelectedValue = IdServidor.ToString();
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
            var DTOCadastrar = new DTO.E1();
            var BLLCadastrar = new BLL.E1();
            Lb_Mensagem.Text = string.Empty;

            if (IdCadastrar.Value != string.Empty)
            {
                DTOCadastrar = BLLCadastrar.Obter(int.Parse(IdCadastrar.Value));
            }
            DTOCadastrar.Descricao = tbDescricao.Text;
            DTOCadastrar.IdServidor = Convert.ToInt64(lstServidor.SelectedValue);
            DTOCadastrar.IdOperadora = Convert.ToInt64(lstOperadora.SelectedValue);
            DTOCadastrar.Placa = tbPlaca.Text;
            DTOCadastrar.Posicao = tbPosicao.Text;
            DTOCadastrar.Contrato = tbContrato.Text;
            DTOCadastrar.Ativo = cbAtivo.Checked;

            BLLCadastrar.Cadastrar(DTOCadastrar);
            GridViewObjeto_Preencher(GridViewObjeto.PageIndex);
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
            var E1 = new BLL.E1();
            E1.Ativar(int.Parse(IdExcluir.Value), false);
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
            e.Row.Cells[4].Text = new BLL.Servidor().Obter(e.Row.Cells[4].Text.ToInt32()).Descricao;

            e.Row.Cells[5].Text = new BLL.Operadora().Obter(e.Row.Cells[5].Text.ToInt32()).Descricao;
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

        protected void btBuscar_Click1(object sender, ImageClickEventArgs e)
        {

        }
    }
}
