using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Silver.UI.Web.Presentation.UserControls
{
    public delegate void EventoControles(object sender, EventArgs e);

    public partial class ucUsuario : System.Web.UI.UserControl
    {
        public event EventoControles OnBtBusca_Click;

        public string txtBusca
        {
            get
            {
                return tbBuscar.Text.Trim();
            }
            set
            {
                tbBuscar.Text = value.Trim();
            }
        }

        public Button BotaoNovo
        {
            get
            {
                return btNovo;
            }
            set
            {
                btNovo = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            if (OnBtBusca_Click != null)
            {
                OnBtBusca_Click(sender, e);
            }
        }
    }
}
