using System;
using System.Collections.Generic;
using System.Linq;
using Silver.UI.Web.Presentation.Requests;
using Silver.AsteriskClient;
using System.Web.UI;
using Silver.DTO;
//using Silver.Discador;

namespace Silver.UI.Web.Presentation.Pages.Operator
{
    public partial class Operador : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
                Response.Redirect("~/Login.aspx");

            if (Session["ProximoCliente"] != null)
                lbl_proximo_cpf.Text = Session["ProximoCliente"].ToString();

            if (!IsPostBack)
            {
                var usuarioSession = (DTO.Usuario)Session["Usuario"];
                lbNome.Text = string.Format("Nome: {0}", usuarioSession.Nome);
                lbRamal.Text = string.Format("Ramal: {0}", usuarioSession.Ramal);
                lbTitulo.Text = "Projeto Silver";
                lstPausas.DataSource = new BLL.Pausa().Listar();
                lstPausas.DataTextField = "Descricao";
                lstPausas.DataValueField = "Id";
                lstPausas.DataBind();
            }
        }

        protected void btIniciarPausa_Click(object sender, EventArgs e)
        {
            var usuarioSession = (DTO.Usuario)Session["Usuario"];

            if (string.IsNullOrEmpty(lstPausas.SelectedItem.Value))
            {
                PanelMensagem.Visible = true;
                lbMensagem.Text = "Favor informe um tipo de pausa";
            }
            else
            {
                var idPausa = Convert.ToInt64(lstPausas.SelectedItem.Value);
                if (new BLL.UsuarioPausa().IniciarPausa(usuarioSession, idPausa))
                {
                    BLL.ControleSistema.IncluirEvento(EventoControleSistema.Iniciar_Pausa, new DTO.ControleSistema
                    {
                        Valor = (Session["Usuario"] as DTO.Usuario).Ramal.ToString() + "|" + idPausa.ToString(),
                        Campanha = (Session["Usuario"] as DTO.Usuario).IdCampanha,
                        Situacao = (int)SitucaoEventoControleSistema.Aguardando,
                        Solicitante = (Session["Usuario"] as DTO.Usuario).Id
                    });

                    btIniciarPausa.Enabled = false;
                    btRetornarPausa.Enabled = true;

                    lbl_mensagem.Text = "Pausa registrada com sucesso!";
                }
                else
                {
                    PanelMensagem.Visible = true;
                    lbMensagem.Text = "A pausa não foi iniciada, favor tentar novamente";
                }
            }

            Silver.BLL.MensagemSistema.Cadastrar(new MensagemSistema()
            {
                DataHora = DateTime.Now,
                IdCampanha = (Session["Usuario"] as DTO.Usuario).IdCampanha,
                Mensagem = String.Format("Início de Pausa:<br>{0}", (Session["Usuario"] as DTO.Usuario).Nome),
                Visualizada = false
            });
        }

        protected void btRetornarPausa_Click(object sender, EventArgs e)
        {
            var usuarioSession = (DTO.Usuario)Session["Usuario"];
            if (new BLL.UsuarioPausa().RetornarPausa(usuarioSession.Id))
            {
                //using (AsteriskCommand asterisk_comando = new AsteriskCommand())
                //    asterisk_comando.FinalizarPausaNaFila(usuarioSession.Ramal, new BLL.Campanha().Obter(usuarioSession.IdCampanha).Nome.Trim().Replace(' ', '_'));

                BLL.ControleSistema.IncluirEvento(EventoControleSistema.Finalizar_Pausa, new DTO.ControleSistema
                {
                    Valor = (Session["Usuario"] as DTO.Usuario).Ramal.ToString(),
                    Campanha = (Session["Usuario"] as DTO.Usuario).IdCampanha,
                    Situacao = (int)SitucaoEventoControleSistema.Aguardando,
                    Solicitante = (Session["Usuario"] as DTO.Usuario).Id
                });

                btRetornarPausa.Enabled = false;
                btIniciarPausa.Enabled = true;

                lbl_mensagem.Text = "Retorno de pausa registrada com sucesso!";
            }
            else
            {
                PanelMensagem.Visible = true;
                lbMensagem.Text = "Seu retorno não foi registrado, favor tentar novamente";
            }

            Silver.BLL.MensagemSistema.Cadastrar(new MensagemSistema()
            {
                DataHora = DateTime.Now,
                IdCampanha = (Session["Usuario"] as DTO.Usuario).IdCampanha,
                Mensagem = String.Format("Fim de Pausa:<br><b>{0}</b>", (Session["Usuario"] as DTO.Usuario).Nome),
                Visualizada = false
            });
        }

        protected void btSair_Click(object sender, EventArgs e)
        {
            var usuarioSession = (DTO.Usuario)Session["Usuario"];

            new BLL.UsuarioLogin().RegistrarSaida(usuarioSession.Id);
            Silver.BLL.MensagemSistema.Cadastrar(new MensagemSistema()
            {
                DataHora = DateTime.Now,
                IdCampanha = (Session["Usuario"] as DTO.Usuario).IdCampanha,
                Mensagem = String.Format("Logoff do Sistema:<br><b>{0}</b>", (Session["Usuario"] as DTO.Usuario).Nome),
                Visualizada = false
            });

            Session.RemoveAll();
            Response.Redirect("~/Login.aspx");
        }

        protected void btOkMensagem_Click(object sender, EventArgs e)
        {
            PanelMensagem.Visible = false;
            lbMensagem.Text = string.Empty;
        }

        protected void imb_logout_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            var usuarioSession = (DTO.Usuario)Session["Usuario"];
            using (AsteriskCommand asterisk_comando = new AsteriskCommand())
                asterisk_comando.IniciarPausaNaFila(usuarioSession.Ramal, new BLL.Campanha().Obter(usuarioSession.IdCampanha).Nome.Trim().Replace(' ', '_'));

            this.Logout();
        }

        protected void timer_refresh_Tick(object sender, EventArgs e)
        {
            var usuarioSession = (DTO.Usuario)Session["Usuario"];
            if (!string.IsNullOrEmpty(UsuarioOnLine.usuarioOnLine[usuarioSession.Ramal]))
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), string.Format("abrir_popup({0},'Atendimento',800,600)", UsuarioOnLine.usuarioOnLine[usuarioSession.Ramal]), false);
        }

        protected void timer_refresh0_Tick(object sender, EventArgs e)
        {
            var usuarioSession = (DTO.Usuario)Session["Usuario"];
            UsuarioOnLine.usuarioOnLine_checking[usuarioSession.Ramal] = DateTime.Now;
            new BLL.UsuarioLogin().RegistrarCheckin(usuarioSession.Id);
        }

        protected void btn_atualizar_requisicao_Click(object sender, EventArgs e)
        {

        }

        protected void lnk_atender_Click(object sender, EventArgs e)
        {
            if (UsuarioOnLine.ProximoCliente.ContainsKey((Session["Usuario"] as DTO.Usuario).Ramal))
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), string.Format("abrir_popup('{0}','CobNet',800,600)", UsuarioOnLine.ProximoCliente[(Session["Usuario"] as DTO.Usuario).Ramal]));
        }
    }
}
