using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Silver.UI.Web.Presentation.Requests;

namespace Silver.UI.Web.Presentation.Pages
{
    public partial class AbrePopUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             var usuarioSession = (DTO.Usuario)Session["Usuario"];
             //if (!string.IsNullOrEmpty(UsuarioOnLine.usuarioOnLine[usuarioSession.Ramal]))
             {
                 Response.Write("<META HTTP-EQUIV=\"Refresh\" CONTENT=\"0; URL=Operador.aspx\">");
                 string script = string.Format("abrir_popup('{0}','CobNet',800,600)", UsuarioOnLine.usuarioOnLine[usuarioSession.Ramal]);
                 ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), script, true);
             }
        }

        protected void TimerRequest_Tick(object sender, EventArgs e)
        {
            //var usuarioSession = (DTO.Usuario)Session["Usuario"];
            //if (!string.IsNullOrEmpty(UsuarioOnLine.usuarioOnLine[usuarioSession.Ramal]))
            //{
            //    //Response.Write("<META HTTP-EQUIV=\"Refresh\" CONTENT=\"0; URL=Teste.aspx\">");
            //    //string script = string.Format("abrir_popup('{0}','CobNet',800,600)", UsuarioOnLine.usuarioOnLine[usuarioSession.Ramal]);
            //    //ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), script, true);

            //    //string script = string.Format("abrir_popup({0},'Cobnet',800,600)", UsuarioOnLine.usuarioOnLine[usuarioSession.Ramal]);
            //    //ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), script, false);
            //    //UsuarioOnLine.usuarioOnLine[usuarioSession.Ramal] = string.Empty;
            //}
        }

        protected void TimerCheckIn_Tick()
        {
            //var usuarioSession = (DTO.Usuario)Session["Usuario"];
            //UsuarioOnLine.usuarioOnLine_checking[usuarioSession.Ramal] = DateTime.Now;
            //new BLL.UsuarioLogin().RegistrarCheckin(usuarioSession.Id);
        }
    }
}