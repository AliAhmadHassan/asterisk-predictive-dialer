using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Silver.UI.Web.Presentation.Pages;
using Silver.DTO;
using Silver.Common;

namespace Silver.UI.Web.Presentation.Pages.Registers
{
    public partial class Grupo : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.NomeAplicacao = "Grupo";
            Master.DescricaoAplicacao = "Configuração e manutenção de Grupo";
            if (!IsPostBack)
            {
                Master.NomeAplicacao = "Grupo";
                Master.DescricaoAplicacao = "Configuração e manutenção de Grupo";

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
                Cadastrar(int.Parse(GridViewObjeto.Rows[int.Parse(e.CommandArgument.ToString())].Cells[3].Text.Replace("&nbsp;", string.Empty)));
            }
            if (e.CommandName == "Excluir")
            {
                Excluir(int.Parse(GridViewObjeto.Rows[int.Parse(e.CommandArgument.ToString())].Cells[3].Text.Replace("&nbsp;", string.Empty)));
            }
        }

        protected void GridViewObjeto_Preencher(int intPageIndex, string filtro = "")
        {
            GridViewObjeto.PageIndex = intPageIndex;
            var Grupo = new BLL.Grupo();

            if (string.IsNullOrEmpty(filtro))
                GridViewObjeto.DataSource = Grupo.Listar();
            else
                GridViewObjeto.DataSource = Grupo.Buscar(filtro);

            GridViewObjeto.DataBind();
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            var Grupo = new BLL.Grupo();
            GridViewObjeto.DataSource = Grupo.Buscar(tbBuscar.Text);
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
            var Grupo = new BLL.Grupo().Obter(intID);

            IdCadastrar.Value = Grupo.Id.ToString();
            tbNome.Text = Grupo.Nome.ToString();
            tbDescricao.Text = Grupo.Descricao.ToString();
            PopularCampos(int.Parse(Grupo.IdGrupo.ToString()), int.Parse(Grupo.Id.ToString()));

            if (Grupo.Ativo)
                cbAtivo.Checked = true;

            Panel_Cadastrar_ModalPopupExtender.Show();
        }

        protected void LimparCampos()
        {
            IdCadastrar.Value = string.Empty;
            tbNome.Text = string.Empty;
            tbDescricao.Text = string.Empty;
            lstUsuario.ClearSelection();
            lstMenu.ClearSelection();
            cbAtivo.Checked = false;
        }

        protected void PopularCampos(int IdGrupo, int Id)
        {
            var grupo = new BLL.Grupo().Obter(Id);

            CarregarGrupos();
            CarregarUsuarios();
            CarregarMenu();

            var usuarios_grupo = new BLL.Usuario().ObterLst(Id, TipoConsulta.PeloGrupo);
            foreach (ListItem item in lstUsuario.Items)
                if (usuarios_grupo.Where(u => u.Id.Equals(item.Value.ToInt64())).FirstOrDefault() != null)
                    item.Selected = true;

            var menus_grupo = new BLL.GrupoMenu().Obter(grupo.Id, TipoConsulta.PeloGrupo);
            foreach (ListItem item in lstMenu.Items)
                if (menus_grupo.Where(m => m.IdMenu.Equals(item.Value.ToInt64())).FirstOrDefault() != null)
                    item.Selected = true;

            if (grupo.IdGrupo > 0)
                lstGrupoAcima.SelectedValue = grupo.IdGrupo.ToString(); ;
        }

        private void CarregarMenu()
        {
            lstMenu.DataSource = new BLL.Menu().Obter(true);
            lstMenu.DataTextField = "Descricao";
            lstMenu.DataValueField = "Id";
            lstMenu.DataBind();
        }

        private void CarregarUsuarios()
        {
            lstUsuario.DataSource = new BLL.Usuario().Obter(true);
            lstUsuario.DataTextField = "Nome";
            lstUsuario.DataValueField = "Id";
            lstUsuario.DataBind();
        }

        private void CarregarGrupos()
        {
            lstGrupoAcima.DataSource = new BLL.Grupo().Obter(true);
            lstGrupoAcima.DataTextField = "Nome";
            lstGrupoAcima.DataValueField = "Id";
            lstGrupoAcima.DataBind();
        }

        protected void btCadastrar_Sim_Click(object sender, EventArgs e)
        {
            var DTOCadastrar = new DTO.Grupo();
            var BLLCadastrar = new BLL.Grupo();
            Lb_Mensagem.Text = string.Empty;

            if (IdCadastrar.Value != string.Empty)
                DTOCadastrar = BLLCadastrar.Obter(int.Parse(IdCadastrar.Value));

            DTOCadastrar.IdGrupo = lstGrupoAcima.SelectedItem != null ? lstGrupoAcima.SelectedValue.ToInt32() : 0;
            DTOCadastrar.Nome = tbNome.Text;
            DTOCadastrar.Descricao = tbDescricao.Text;
            DTOCadastrar.Ativo = cbAtivo.Checked;

            BLLCadastrar.Cadastrar(DTOCadastrar);


            var lst = new List<long>();
            foreach (ListItem itemMenu in lstMenu.Items)
            {
                if (itemMenu.Selected)
                {
                    lst.Add(long.Parse(itemMenu.Value));
                }
            }

            new BLL.GrupoMenu().Cadastrar(DTOCadastrar.Id, lst, TipoConsulta.PeloGrupo);

            var DTOUsuario = new DTO.Usuario();
            foreach (ListItem itemUsuario in lstUsuario.Items)
            {
                if (itemUsuario.Selected)
                {
                    DTOUsuario = new BLL.Usuario().ObterObj(long.Parse(itemUsuario.Value), TipoConsulta.PelaPK);
                    DTOUsuario.IdGrupo = DTOCadastrar.Id;
                    new BLL.Usuario().Cadastrar(DTOUsuario);
                }
            }
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
            var Grupo = new BLL.Grupo();
            Grupo.Ativar(int.Parse(IdExcluir.Value), false);
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
            if (e.Row.Cells[7].Text.Trim().Equals("False"))
            {
                e.Row.Cells[7].Text = "Não";
            }
            else
            {
                e.Row.Cells[7].Text = "Sim";
            }
            if (!string.IsNullOrEmpty(e.Row.Cells[4].Text))
            {
                e.Row.Cells[4].Text = new BLL.Grupo().Obter(e.Row.Cells[4].Text.ToInt32()).Descricao;
            }
        }

        protected void GridViewObjeto_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            var grupo_selecionado = GridViewObjeto.Rows[e.NewSelectedIndex].Cells[3].Text.ToInt64();
            Session.Add("GrupoSelecionado", grupo_selecionado);
            lbl_grupo_selecionado.Text = GridViewObjeto.Rows[e.NewSelectedIndex].Cells[5].Text;

            var usuarios = new BLL.Usuario().ObterLst(grupo_selecionado, TipoConsulta.PeloGrupo);
            GridViewUsuario.DataSource = usuarios;
            GridViewUsuario.DataBind();
            lbl_total_operadores.Text = usuarios.Count.ToString("00000");

            var menus = new BLL.GrupoMenu().Obter(grupo_selecionado, TipoConsulta.PeloGrupo);
            GridViewObjetoMenu.DataSource = menus;
            GridViewObjetoMenu.DataBind();
            lbl_permissao_grupo.Text = menus.Count.ToString("00000");

        }

        protected void GridViewUsuario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }
            if (e.Row.Cells[5].Text.Trim().Equals("False"))
            {
                e.Row.Cells[5].Text = "Não";
            }
            else
            {
                e.Row.Cells[5].Text = "Sim";
            }
            if (e.Row.Cells[4].Text.Trim().Equals("False"))
            {
                e.Row.Cells[4].Text = "Não";
            }
            else
            {
                e.Row.Cells[4].Text = "Sim";
            }
            e.Row.Cells[3].Text = new BLL.Campanha().Obter(e.Row.Cells[3].Text.ToInt32()).Descricao;
        }

        protected void GridViewObjetoMenu_RowDataBound(object sender, GridViewRowEventArgs e)
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
            if (!string.IsNullOrEmpty(e.Row.Cells[1].Text))
            {
                e.Row.Cells[1].Text = new BLL.Menu().Obter(e.Row.Cells[1].Text.ToInt32()).Descricao;
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

        protected void GridViewUsuario_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var usuarios = new BLL.Usuario().ObterLst(Session["GrupoSelecionado"].ToInt64(), TipoConsulta.PeloGrupo);
            GridViewUsuario.PageIndex = e.NewPageIndex;
            GridViewUsuario.DataSource = usuarios;
            GridViewUsuario.DataBind();
        }

        protected void GridViewObjetoMenu_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var menus = new BLL.GrupoMenu().Obter(Session["GrupoSelecionado"].ToInt64(), TipoConsulta.PeloGrupo);
            GridViewObjetoMenu.PageIndex = e.NewPageIndex;
            GridViewObjetoMenu.DataSource = menus;
            GridViewObjetoMenu.DataBind();
        }
    }
}
