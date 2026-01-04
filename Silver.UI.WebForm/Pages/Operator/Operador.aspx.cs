using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silver.UI.Web.Presentation.Pages.Operator
{
    public partial class Operador : System.Web.UI.Page
    {
        private DTO.Usuario usuarioSession = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                usuarioSession = (DTO.Usuario)Session["Usuario"];
                lbNome.Text = usuarioSession.Ramal.ToString();
                lbRamal.Text = usuarioSession.Ramal.ToString();
                lbTitulo.Text = "Tela Operador";
                lstPausas.DataSource = TempPopularListBox();
                lstPausas.DataBind();
            }
        }

        protected void btIniciarPausa_Click(object sender, EventArgs e)
        {
            try
            {
                long idPausa = string.IsNullOrEmpty(lstPausas.SelectedValue) ? 0 : Convert.ToInt64(lstPausas.SelectedValue);
                new BLL.UsuarioPausa().IniciarPausa(usuarioSession.Id, idPausa);
                btIniciarPausa.Enabled = false;
                btRetornarPausa.Enabled = true;
            }
            catch (Exception)
            {   
                   
            }            
        }
        
        protected void btRetornarPausa_Click(object sender, EventArgs e)
        {
            btRetornarPausa.Enabled = false;
            btIniciarPausa.Enabled = true;
            new BLL.UsuarioPausa().RetornarPausa(usuarioSession.Id);
        }

        protected void btSair_Click(object sender, EventArgs e)
        {
            new BLL.UsuarioLogin().RegistrarSaida(usuarioSession.Id);
            Session["Usuario"] = null;
            Response.Redirect(@"Login.aspx");
        }

        protected List<ListItem> TempPopularListBox()
        {
            List<ListItem> retorno = new List<ListItem>();
            ListItem item = new ListItem();

            for (int i = 0; i < 10; i++)
            {
                item = new ListItem(string.Format("Pausa {0}", i), i.ToString());
                retorno.Add(item);
            }

            return retorno;
        }
    }
}