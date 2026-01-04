using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silver.UI.Web.Presentation.Pages.Registers
{
    public partial class Servidor : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.NomeAplicacao = "Servidor";
                Master.DescricaoAplicacao = "Configuração e manutenção de servidores";

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
            var Servidor = new BLL.Servidor();

            if (string.IsNullOrEmpty(filtro))
                GridViewObjeto.DataSource = Servidor.Listar();
            else
                GridViewObjeto.DataSource = Servidor.Buscar(filtro);

            GridViewObjeto.DataBind();
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            var Servidor = new BLL.Servidor();
            GridViewObjeto.DataSource = Servidor.Buscar(tbBuscar.Text);
            GridViewObjeto.DataBind();
        }

        protected void btNovo_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        protected void Cadastrar(int intID)
        {
            LimparCampos();
            var Servidor = new BLL.Servidor().Obter(intID);
            IdCadastrar.Value = Servidor.Id.ToString();
            tbDescricao.Text = Servidor.Descricao.ToString();
            tbIp.Text = Servidor.Ip.ToString();
            tbUrl.Text = Servidor.Url.ToString();
            if (Servidor.Ativo)
            {
                cbAtivo.Checked = true;
            }
            Panel_Cadastrar_ModalPopupExtender.Show();
        }

        protected void LimparCampos()
        {
            IdCadastrar.Value = string.Empty;
            tbDescricao.Text = string.Empty;
            tbIp.Text = string.Empty;
            tbUrl.Text = string.Empty;
            cbAtivo.Checked = false;
        }

        protected void btCadastrar_Sim_Click(object sender, EventArgs e)
        {
            var DTOCadastrar = new DTO.Servidor();
            var BLLCadastrar = new BLL.Servidor();
            Lb_Mensagem.Text = string.Empty;

            if (IdCadastrar.Value != string.Empty)
            {
                DTOCadastrar = BLLCadastrar.Obter(int.Parse(IdCadastrar.Value));
            }
            DTOCadastrar.Descricao = tbDescricao.Text;
            DTOCadastrar.Ip = tbIp.Text;
            DTOCadastrar.Url = tbUrl.Text;
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
            var Servidor = new BLL.Servidor();
            Servidor.Ativar(int.Parse(IdExcluir.Value), false);
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
            if (e.Row.Cells[6].Text.Trim().Equals("False"))
            {
                e.Row.Cells[6].Text = "Não";
            }
            else
            {
                e.Row.Cells[6].Text = "Sim";
            }
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
