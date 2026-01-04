using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Silver.Common;
using Silver.DTO;

namespace Silver.UI.Web.Presentation.Pages.Registers
{
    public partial class Menu : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.NomeAplicacao = "Menu";
            Master.DescricaoAplicacao = "Configuração e manutenção de Menu";

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
            var Menu = new BLL.Menu();

            if (string.IsNullOrEmpty(filtro))
                GridViewObjeto.DataSource = Menu.Listar();
            else
                GridViewObjeto.DataSource = Menu.Buscar(filtro);

            GridViewObjeto.DataBind();
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            var Menu = new BLL.Menu();
            GridViewObjeto.DataSource = Menu.Buscar(tbBuscar.Text);
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
            var Menu = new BLL.Menu().Obter(intID);
            IdCadastrar.Value = Menu.Id.ToString();
            PopularCampos(int.Parse(Menu.IdMenu.ToString()), int.Parse(Menu.Id.ToString()));
            tbDescricao.Text = Menu.Descricao.ToString();
            tbUrl.Text = Menu.Url.ToString();
            
            ddl_grupomenu.SelectedValue = Menu.GrupoMenu.ToString();

            if (Menu.Icone != null)
            {
                if (Menu.Icone.Length > 0)
                {
                    lbIcone.Text = "~/Imagens/Temp/menu_imgwrite_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                    var fs = new FileStream(Server.MapPath(lbIcone.Text), FileMode.OpenOrCreate, FileAccess.Write);
                    var ArraySize = new int();
                    ArraySize = Menu.Icone.GetUpperBound(0);
                    fs.Write(Menu.Icone, 0, ArraySize);
                    fs.Close();

                    imgIcone.ImageUrl = lbIcone.Text;
                    imgIcone.Visible = true;
                }
            }

            if (Menu.Ativo)
                cbAtivo.Checked = true;
            
            Panel_Cadastrar_ModalPopupExtender.Show();
        }

        protected void LimparCampos()
        {
            IdCadastrar.Value = string.Empty;
            lstMenuAcima.ClearSelection();
            tbDescricao.Text = string.Empty;
            tbUrl.Text = string.Empty;
            fuIcone.Attributes.Clear();
            ddl_grupomenu.SelectedIndex = -1;
            if (lbIcone.Text != string.Empty)
                if (File.Exists(lbIcone.Text))
                    File.Delete(lbIcone.Text);
            
            cbAtivo.Checked = false;
        }

        protected void PopularCampos(int IdMenu, int Id)
        {
            lstMenuAcima.Items.Clear();
            lstGrupo.Items.Clear();

            lstMenuAcima.DataSource = new BLL.Menu().Obter(true);
            lstMenuAcima.DataTextField = "Descricao";
            lstMenuAcima.DataValueField = "Id";
            lstMenuAcima.DataBind();

            if (IdMenu > 0)
                if ((lstMenuAcima.Items.FindByValue(IdMenu.ToString()) != null))
                    lstMenuAcima.SelectedValue = IdMenu.ToString();

            lstGrupo.DataSource = new BLL.Grupo().Obter(true);
            lstGrupo.DataTextField = "Nome";
            lstGrupo.DataValueField = "Id";
            lstGrupo.DataBind();

            var result = new BLL.GrupoMenu().Obter(Id, TipoConsulta.PeloMenu);
            if (Id > 0)
                foreach (DTO.GrupoMenu GrupoMenu in result)
                    if ((lstGrupo.Items.FindByValue(GrupoMenu.IdGrupo.ToString()) != null))
                        lstGrupo.Items.FindByValue(GrupoMenu.IdGrupo.ToString()).Selected = true;
        }

        protected void btCadastrar_Sim_Click(object sender, EventArgs e)
        {
            var Continua = false;
            
            if (fuIcone.HasFile)
                Continua = true;
            else
                Continua = true;

            if (Continua)
            {
                var DTOCadastrar = new DTO.Menu();
                var BLLCadastrar = new BLL.Menu();
                
                Lb_Mensagem.Text = string.Empty;

                if (IdCadastrar.Value != string.Empty)
                    DTOCadastrar = BLLCadastrar.Obter(int.Parse(IdCadastrar.Value));

                var grupo_menu_id = 0L;
                if (lstMenuAcima.SelectedItem != null)
                {
                    grupo_menu_id = lstMenuAcima.SelectedValue.ToInt64();
                    DTOCadastrar.IdMenu = Convert.ToInt64(grupo_menu_id);
                }

                DTOCadastrar.Descricao = tbDescricao.Text;
                DTOCadastrar.Url = tbUrl.Text;
                DTOCadastrar.GrupoMenu = ddl_grupomenu.SelectedValue.ToInt32();

                var filePath = Server.MapPath("~/Imagens/Temp/") + "menu_imgwrite_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                if (fuIcone.HasFile)
                {
                    fuIcone.SaveAs(filePath);
                    DTOCadastrar.Icone = new BinaryReader(fuIcone.PostedFile.InputStream).ReadBytes(fuIcone.PostedFile.ContentLength);// br.ReadBytes((int)fs.Length);
                }

                DTOCadastrar.Ativo = cbAtivo.Checked;
                BLLCadastrar.Cadastrar(DTOCadastrar);

                var lst = new List<long>();
                foreach (ListItem itemGrupo in lstGrupo.Items)
                    if (itemGrupo.Selected)
                        lst.Add(long.Parse(itemGrupo.Value));
                
                new BLL.GrupoMenu().Cadastrar(DTOCadastrar.Id, lst, TipoConsulta.PeloMenu);

                if (File.Exists(filePath))
                    File.Delete(filePath);
                
                LimparCampos();
                Lb_Mensagem.Text = "Item cadastrado com sucesso";
            }
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
            var Menu = new BLL.Menu();
            Menu.Ativar(int.Parse(IdExcluir.Value), false);
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
            LimparCampos();
        }

        protected void GridViewObjeto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            if (e.Row.Cells[7].Text.Trim().Equals("False"))
                e.Row.Cells[7].Text = "Não";
            else
                e.Row.Cells[7].Text = "Sim";

            var grupo_menu = e.Row.Cells[6].Text.Trim().ToInt32();
            var grupo_menu_texto = "Grupo Raiz";
            if (grupo_menu.Equals(0))
                e.Row.Cells[6].Text = grupo_menu_texto;
            else
            {
                switch (grupo_menu)
                {
                    case 1:
                        grupo_menu_texto = "Cadastros";
                        break;
                    case 2:
                        grupo_menu_texto = "Relatórios";
                        break;
                    case 3:
                        grupo_menu_texto = "Telefonia";
                        break;
                }
            }

            e.Row.Cells[6].Text = grupo_menu_texto;

            if (!string.IsNullOrEmpty(e.Row.Cells[3].Text))
                e.Row.Cells[3].Text = new BLL.Menu().Obter(e.Row.Cells[3].Text.ToInt32()).Descricao;

            var item_menu = (DTO.Menu)e.Row.DataItem;
            if (item_menu.Icone == null) return;

            var icone_menu = (Image)e.Row.FindControl("img_icone");
            var icone_nome_fisico = Server.MapPath(string.Format("~/Imagens/Temp/{0}.png", item_menu.Id.ToString()));
            var icone_nome_relativo = string.Format("~/Imagens/Temp/{0}.png", item_menu.Id.ToString());

            if (!File.Exists(icone_nome_fisico))
                if (item_menu.Icone != null)
                    File.WriteAllBytes(icone_nome_fisico, item_menu.Icone);

            if (icone_menu == null) return;
            icone_menu.ImageUrl = icone_nome_relativo;
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
