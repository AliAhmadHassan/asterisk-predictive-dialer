using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silver.UI.Web.Presentation.Pages.Registers
{
    public partial class DownloadGravacao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["IdBilhetagem"] == null) return;

            DTO.BilhetagemGravacao gravacao = new BLL.BilhetagemGravacao().SelectPelaBilhetagem(Convert.ToInt64(Request.QueryString["IdBilhetagem"]));
            if (gravacao == null) return;

            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}.WAV\"", Guid.NewGuid().ToString()));
            Response.AddHeader("Content-Length", gravacao.Gravacao.Length.ToString());
            Response.BinaryWrite(gravacao.Gravacao);
            Response.End();
            Response.Flush();
            Response.Close();
        }
    }
}