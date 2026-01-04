using System;
using System.Collections.Generic;
using System.Linq;
using Silver.Common;

namespace Silver.UI.Web.Presentation.Requests
{
    public partial class Requests : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var ramal = Request.QueryString["ramal"].ToInt64();
            var cpf = Request.QueryString["cpf"];

            if (ramal > 0 && cpf != null)
            {
                try
                {
                    if (UsuarioOnLine.usuarioOnLine.ContainsKey(ramal))
                    {
                        var url = string.Format(@"http://192.168.20.201/Logados/Acionamento/Atendimento/Detalhes_do_Devedor.aspx?CPFCNPJ={0:d15}", Request.QueryString["cpf"]);

                        if (!UsuarioOnLine.ProximoCliente.ContainsKey(ramal))
                            UsuarioOnLine.ProximoCliente.Add(ramal, url);
                        else
                            UsuarioOnLine.ProximoCliente[ramal] = url;

                        UsuarioOnLine.usuarioOnLine[ramal] = url;
                    }
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
