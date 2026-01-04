using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silver.UI.Web.Presentation.Pages.Dashboard
{
    public partial class Default : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.NomeAplicacao = "Dashboard Discador";
            Master.DescricaoAplicacao = string.Empty;
        }
    }
}