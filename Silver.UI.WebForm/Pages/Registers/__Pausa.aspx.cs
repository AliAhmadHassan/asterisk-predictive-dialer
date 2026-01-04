using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silver.UI.Web.Presentation.Pages.Registers
{
    public partial class Pausa : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.NomeAplicacao = "Pausa";
                Master.DescricaoAplicacao = "Configuração e manutenção de pausas";

                DTO.Usuario usuarioSession = (DTO.Usuario)Session["Usuario"];
                lbNome.Text = usuarioSession.Nome;
                lbRamal.Text = usuarioSession.Ramal.ToString();

                GridViewObjeto_Preencher(0);
                CarregarListViewCampanha();
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
                ExibirPanelCadastro(Convert.ToInt32(GridViewObjeto.Rows[int.Parse(e.CommandArgument.ToString())].Cells[2].Text));
            }
            if (e.CommandName == "Excluir")
            {
                hfExcluirId.Value = GridViewObjeto.Rows[int.Parse(e.CommandArgument.ToString())].Cells[2].Text;
                Panel_Excluir.Visible = true;
            }
        }

        protected void GridViewObjeto_Preencher(int intPageIndex)
        {
            GridViewObjeto.PageIndex = intPageIndex;
            GridViewObjeto.DataSource = new BLL.Pausa().Listar();
            GridViewObjeto.DataBind();
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbBuscar.Text))
            {
                GridViewObjeto.DataSource = new BLL.Pausa().Obter(tbBuscar.Text);
                GridViewObjeto.DataBind();
            }
            else
            {
                Panel_Mensagem.Visible = true;
            }
        }

        protected void LimparCampos()
        {
            tbDescricao.Text = string.Empty;
            cbAtivo.Checked = false;
        }

        protected void ExibirPanelCadastro(int codigoPausa)
        {
            DTO.Usuario usuarioSession = (DTO.Usuario)Session["Usuario"];

            if (codigoPausa > 0)
            {
                DTO.Pausa pausa = new BLL.Pausa().Obter(codigoPausa);
                tbDescricao.Text = pausa.Descricao;
                cbAtivo.Checked = pausa.Ativo;
                CarregarListViewCampanha();
                lstCampanha.SelectedValue = usuarioSession.IdCampanha.ToString();
                hfAtualizar.Value = codigoPausa.ToString();
            }
            else
            {
                tbDescricao.Text = string.Empty;
                cbAtivo.Checked = false;
                CarregarListViewCampanha();
                hfAtualizar.Value = "-1";
            }

            Panel_Cadastrar_ModalPopupExtender.Show();
        }

        private void CarregarListViewCampanha()
        {
            lstCampanha.DataSource = new BLL.Campanha().Listar();
            lstCampanha.DataTextField = "Descricao";
            lstCampanha.DataValueField = "Id";
            lstCampanha.DataBind();
        }

        protected void btSalvarCadastro_Click(object sender, EventArgs e)
        {
            DTO.Usuario usuarioSession = (DTO.Usuario)Session["Usuario"];
            List<long> lstIdCampanha = new List<long>();

            DTO.Pausa pausa = new DTO.Pausa();
            pausa.Id = Convert.ToInt64(hfAtualizar.Value);
            pausa.Descricao = tbDescricao.Text;
            pausa.Ativo = cbAtivo.Checked;

            foreach (ListItem li in lstCampanha.Items)
                if (li.Selected)
                    lstIdCampanha.Add(Convert.ToInt64(li.Value));

            long idPausa = new BLL.Pausa().Cadastro(pausa);
            new BLL.CampanhaPausa().Cadastrar(lstIdCampanha, idPausa);

            Panel_Mensagem.Visible = true;
            Lb_Mensagem.Text = string.Format("Pausa {0} com sucesso.", hfAtualizar.Value == "-1" ? "cadastrada" : "atualizada");
            LimparCampos();
        }

        protected void btCancelarCadastro_Click(object sender, EventArgs e)
        {
            Panel_Mensagem.Visible = false;
        }

        protected void btOkMensagem_Click(object sender, EventArgs e)
        {
            GridViewObjeto_Preencher(GridViewObjeto.PageIndex);
            Panel_Mensagem.Visible = false;
        }

        protected void btNaoExcluir_Click(object sender, EventArgs e)
        {
            Panel_Excluir.Visible = false;
            hfExcluirId.Value = string.Empty;
        }

        protected void btSimExcluir_Click(object sender, EventArgs e)
        {
            new BLL.Pausa().Ativar(Convert.ToInt32(hfExcluirId.Value), false);
        }

        protected void btSair_Click(object sender, EventArgs e)
        {
            DTO.Usuario usuarioSession = (DTO.Usuario)Session["Usuario"];
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
                return;

            if (e.Row.Cells[4].Text.Trim().Equals("False"))
                e.Row.Cells[4].Text = "Não";
            else
                e.Row.Cells[4].Text = "Sim";
        }

        protected void btMensagem_Click(object sender, EventArgs e)
        {
            Panel_Mensagem.Visible = false;
        }

        protected void btMensagem_Click1(object sender, EventArgs e)
        {

        }
    }
}