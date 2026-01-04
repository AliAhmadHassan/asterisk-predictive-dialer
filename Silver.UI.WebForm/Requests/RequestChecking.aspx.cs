using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Silver.AsteriskClient;

namespace Silver.UI.Web.Presentation.Requests
{
    public partial class RequestChecking : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void timer_checking_Tick(object sender, EventArgs e)
        {
            //    var usuarios = UsuarioOnLine.usuarioOnLine_checking.Where(u => u.Value < DateTime.Now.AddSeconds(-10)).ToList();
            //    foreach (var u in usuarios)
            //    {
            //        new ClienteAsterisk().RemoverOperadorFila(u.Key, new BLL.Usuario().ObterObj(u.Key, DTO.TipoConsulta.PeloRamal).CampanhaDescricao);
            //        UsuarioOnLine.usuarioOnLine.Remove(u.Key);
            //        UsuarioOnLine.usuarioOnLine_checking.Remove(u.Key);
            //    }
            //}

            foreach (var item in UsuarioOnLine.usuarioOnLine)
            {
                Response.Write("Ramal: " + item.Key +  ", Url: " + item.Value);
            }
            
        }
    }
}