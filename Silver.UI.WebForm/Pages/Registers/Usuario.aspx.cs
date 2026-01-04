using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Silver.UI.Web.Presentation.Pages;
using Silver.Common;
using Silver.DTO;

namespace Silver.UI.Web.Presentation
{
    public partial class Usuario : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.NomeAplicacao = "Usuários";
                Master.DescricaoAplicacao = "Configuração e manutenção de usuários";

                tbSenha.Attributes["value"] = tbSenha.Text;
                tbRamal.Attributes.Add("onkeypress", "if (event.keyCode < 48 || event.keyCode > 58) {event.keyCode = 0;}");
                tbSenha.Attributes.Add("onkeypress", "if (event.keyCode < 48 || event.keyCode > 58) {event.keyCode = 0;}");

                var usuarioSession = (DTO.Usuario)Session["Usuario"];
                lbNome.Text = usuarioSession.Nome;
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
            var Usuario = new BLL.Usuario();

            if (string.IsNullOrEmpty(filtro))
                GridViewObjeto.DataSource = Usuario.Listar();
            else
                GridViewObjeto.DataSource = Usuario.Buscar(filtro);
            GridViewObjeto.DataBind();
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            var Usuario = new BLL.Usuario();
            GridViewObjeto.DataSource = Usuario.Buscar(tbBuscar.Text);
            GridViewObjeto.DataBind();
        }

        protected void btNovo_Click(object sender, EventArgs e)
        {
            LimparCampos();
            PopularCampos(0, 0);
            Panel_Cadastrar.Visible = true;
        }

        protected void Cadastrar(int intID)
        {
            LimparCampos();
            var Usuario = new BLL.Usuario().ObterObj(intID, TipoConsulta.PelaPK);
            IdCadastrar.Value = Usuario.Id.ToString();
            tbRamal.Text = Usuario.Ramal.ToString();
            tbNome.Text = Usuario.Nome.ToString();
            PopularCampos(int.Parse(Usuario.IdGrupo.ToString()), int.Parse(Usuario.IdCampanha.ToString()));
            tbSenha.Text = Usuario.Senha.ToString();
            tbConfirma.Text = Usuario.Senha.ToString();
            if (Usuario.Operador)
            {
                cbOperador.Checked = true;
            }
            if (Usuario.Ativo)
            {
                cbAtivo.Checked = true;
            }
            Panel_Cadastrar.Visible = true;
            Panel_Cadastrar_ModalPopupExtender.Show();
        }

        protected void LimparCampos()
        {
            IdCadastrar.Value = string.Empty;
            tbRamal.Text = string.Empty;
            tbNome.Text = string.Empty;
            lstGrupo.ClearSelection();
            lstCampanha.ClearSelection();
            tbSenha.Text = string.Empty;
            tbConfirma.Text = string.Empty;
            cbOperador.Checked = false;
            cbAtivo.Checked = false;
        }

        protected void PopularCampos(int IdGrupo, int IdCampanha)
        {
            lstGrupo.DataSource = new BLL.Grupo().Obter(true);
            lstGrupo.DataTextField = "Nome";
            lstGrupo.DataValueField = "Id";
            lstGrupo.DataBind();
            if (IdGrupo > 0)
            {
                if ((lstGrupo.Items.FindByValue(IdGrupo.ToString()) != null))
                {
                    lstGrupo.SelectedValue = IdGrupo.ToString();
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
            var DTOCadastrar = new DTO.Usuario();
            var BLLCadastrar = new BLL.Usuario();
            Lb_Mensagem.Text = string.Empty;
            var Cadastro = false;

            if (IdCadastrar.Value != string.Empty)
            {
                DTOCadastrar = BLLCadastrar.ObterObj(int.Parse(IdCadastrar.Value), TipoConsulta.PelaPK);
                if (DTOCadastrar.Senha == tbSenha.Text)
                {
                    Cadastro = true;
                }
                else
                {
                    if (DTOCadastrar.UltimaSenha == tbSenha.Text || DTOCadastrar.PenultimaSenha == tbSenha.Text)
                    {
                        Lb_Mensagem.Text = "A nova Senha não pode ser igual às 3 últimas.";
                        Cadastro = false;
                    }
                    else
                    {
                        DTOCadastrar.PenultimaSenha = DTOCadastrar.UltimaSenha;
                        DTOCadastrar.UltimaSenha = DTOCadastrar.Senha;
                        Cadastro = true;
                    }
                }
            }
            else
            {
                Cadastro = true;
            }

            if (Cadastro)
            {
                DTOCadastrar.Ramal = Convert.ToInt64(tbRamal.Text);
                DTOCadastrar.Nome = tbNome.Text;
                DTOCadastrar.IdGrupo = Convert.ToInt64(lstGrupo.SelectedValue);
                if (!string.IsNullOrEmpty(lstCampanha.SelectedValue))
                    DTOCadastrar.IdCampanha = Convert.ToInt64(lstCampanha.SelectedValue);
                
                DTOCadastrar.Senha = tbSenha.Text;
                DTOCadastrar.SenhaExpiracao = DateTime.Now.AddMonths(3);
                DTOCadastrar.Operador = cbOperador.Checked;
                DTOCadastrar.Ativo = cbAtivo.Checked;
                BLLCadastrar.Cadastrar(DTOCadastrar);

                LimparCampos();
                Panel_Cadastrar.Visible = false;
                Lb_Mensagem.Text = "Item cadastrado com sucesso";
            }
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
            var Usuario = new BLL.Usuario();
            Usuario.Ativar(int.Parse(IdExcluir.Value), false);
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
            if (e.Row.Cells[7].Text.Trim().Equals("False"))
            {
                e.Row.Cells[7].Text = "Não";
            }
            else
            {
                e.Row.Cells[7].Text = "Sim";
            }
            e.Row.Cells[5].Text = new BLL.Grupo().Obter(e.Row.Cells[5].Text.ToInt32()).Descricao;
            e.Row.Cells[6].Text = new BLL.Campanha().Obter(e.Row.Cells[6].Text.ToInt32()).Descricao;
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
