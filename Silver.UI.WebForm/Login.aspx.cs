using System;
using System.Collections.Generic;
using System.Linq;
using Silver.UI.Web.Presentation.Requests;
using Silver.DTO;
using Silver.Common;
using Silver.AsteriskClient;
using System.Configuration;
//using Silver.Discador;

namespace Silver.UI.Web.Presentation
{
    public partial class Login2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lbl_versao.Text = ConfigurationManager.AppSettings["application.version"];
            lbl_copyright.Text = DateTime.Now.Year.ToString();
        }

        protected void btLogin_Click(object sender, EventArgs e)
        {
            var usuario = new BLL.Usuario();
            DTO.Usuario usuarioObj = null;

            if (usuario.ValidarUsuario(tbRamal.Text.Trim(), tbSenha.Text))
            {
                usuarioObj = usuario.ObterObj(tbRamal.Text.Trim().ToInt64(), TipoConsulta.PeloRamal);

                if (usuario.ValidarExpiracaoSenha((int)usuarioObj.Id))
                {
                    Session.Add("Usuario", usuarioObj);

                    if (usuarioObj.Operador)
                    {
                        if (usuarioObj.IdCampanha <= 0)
                            lbMensagem.Text = "Campanha não definida";

                        var servico_loggon = new BLL.UsuarioLogado();
                        var logado = servico_loggon.Obter(usuarioObj.Ramal);

                        if (logado == null)
                        {
                            logado = new UsuarioLogado();
                            logado.Ramal = usuarioObj.Ramal;
                            logado.Url = string.Empty;
                            servico_loggon.Cadastrar(logado);
                        }

                        new BLL.UsuarioLogin().RegistrarEntrada(usuarioObj);

                        BLL.ControleSistema.IncluirEvento(EventoControleSistema.Finalizar_Pausa, new DTO.ControleSistema
                        {
                            Valor = (Session["Usuario"] as DTO.Usuario).Ramal.ToString(),
                            Campanha = (Session["Usuario"] as DTO.Usuario).IdCampanha,
                            Situacao = (int)SitucaoEventoControleSistema.Aguardando,
                            Solicitante = (Session["Usuario"] as DTO.Usuario).Id
                        });

                        Silver.BLL.MensagemSistema.Cadastrar(new MensagemSistema()
                        {
                            DataHora = DateTime.Now,
                            IdCampanha = (Session["Usuario"] as DTO.Usuario).IdCampanha,
                            Mensagem = String.Format("Operador logado no sistema: {0}", (Session["Usuario"] as DTO.Usuario).Nome),
                            Visualizada = false
                        });

                        Response.Redirect(@"\Pages\Operador.aspx");
                    }
                    else
                    {
                        new BLL.UsuarioLogin().RegistrarEntrada(usuarioObj);
                        Silver.BLL.MensagemSistema.Cadastrar(new MensagemSistema()
                        {
                            DataHora = DateTime.Now,
                            IdCampanha = (Session["Usuario"] as DTO.Usuario).IdCampanha,
                            Mensagem = String.Format("Operador logado no sistema: {0}", (Session["Usuario"] as DTO.Usuario).Nome),
                            Visualizada = false
                        });
                        Response.Redirect("~/Default.aspx");
                    }

                    
                }
                else
                {
                    lbMensagem.Text = "A senha expirou, favor avisar o administrador";
                }
            }
            else
            {
                lbMensagem.Text = "Usuário ou senha inválida";
            }
        }
    }
}
