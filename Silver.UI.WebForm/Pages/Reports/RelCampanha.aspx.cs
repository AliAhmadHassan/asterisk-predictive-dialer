using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Silver.Common;
using Silver.DTO;
using System.IO;

namespace Silver.UI.Web.Presentation.Pages.Reports
{
    public partial class RelCampanha : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.NomeAplicacao = "Acompanha Campanha";
                Master.DescricaoAplicacao = "Acompanhamento da Campanha e os Atendentes";

                var usuarioSession = (DTO.Usuario)Session["Usuario"];
                lbNome.Text = usuarioSession.Nome.ToString();
                lbRamal.Text = usuarioSession.Ramal.ToString();
                CarregarCampanhas();
                CarregarGrid();
            }
        }

        protected void btn_iniciar_campanha_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlCampanhas.SelectedItem == null) return;
            BLL.ControleSistema.IncluirEvento(EventoControleSistema.Iniciar_Campanha, new DTO.ControleSistema { Valor = ddlCampanhas.SelectedItem.Value, Campanha = ddlCampanhas.SelectedItem.Value.ToInt64(), Situacao = (int)SitucaoEventoControleSistema.Aguardando, Solicitante = (Session["Usuario"] as DTO.Usuario).Id });
        }

        protected void btn_parar_campanha_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlCampanhas.SelectedItem == null) return;
            BLL.ControleSistema.IncluirEvento(EventoControleSistema.Parar_Campanha, new DTO.ControleSistema { Valor = ddlCampanhas.SelectedItem.Value, Campanha = ddlCampanhas.SelectedItem.Value.ToInt64(), Situacao = (int)SitucaoEventoControleSistema.Aguardando, Solicitante = (Session["Usuario"] as DTO.Usuario).Id });
        }

        protected void btn_continuar_campanha_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlCampanhas.SelectedItem == null) return;
            //BLL.ControleSistema.IncluirEvento(EventoControleSistema.Continuar_Campanha, ddlCampanhas.SelectedItem.Value.ToString(), (Session["Usuario"] as DTO.Usuario).Id);
        }

        protected void btn_reiniciar_Click(object sender, ImageClickEventArgs e)
        {
            BLL.ControleSistema.IncluirEvento(EventoControleSistema.Recarregar_Campanha, new DTO.ControleSistema { Valor = ddlCampanhas.SelectedItem.Value, Campanha = ddlCampanhas.SelectedItem.Value.ToInt64(), Situacao = (int)SitucaoEventoControleSistema.Aguardando, Solicitante = (Session["Usuario"] as DTO.Usuario).Id });
        }

        private void MontaMapaOperadores(int IdCampanha)
        {
            BLL.ControleSistema.IncluirEvento(new ControleSistema()
            {
                Campanha = ddlCampanhas.SelectedItem.Value.ToInt64(),
                Valor = ddlCampanhas.SelectedItem.Value.ToString(),
                Evento = Enum.GetName(typeof(DTO.EventoControleSistema), DTO.EventoControleSistema.Situacao_Fila),
                Porcentagem = 0,
                Situacao = (int)DTO.SitucaoEventoControleSistema.Aguardando,
                Solicitante = (Session["Usuario"] as DTO.Usuario).Id
            });

            DTO.RelCampanha RelCampanha = new BLL.RelCampanha().SelectQueueShow(IdCampanha);
            List<DTO.RelCampanhaQueueStatus> relCampanhaQueueStatus = new BLL.RelCampanha().SelectQueueStatus(IdCampanha);

            if (RelCampanha == null)
            {
                Lb_Mensagem.Text = "Nenhum dado encontrado para esta campanha.";
                Panel_Mensagem.Visible = true;

                ltlMapa.Text = "";
                return;
            }

            lblLigEntregue.Text = RelCampanha.dadoC.ToString();
            lblAbandono.Text = RelCampanha.dadoA.ToString();

            if (RelCampanha.dadoA != 0)
                lblTaxaSLA.Text = Math.Round(100 - (Convert.ToDecimal(RelCampanha.dadoA) / Convert.ToDecimal(RelCampanha.dadoC + RelCampanha.dadoA)) * 100, 2).ToString() + " %";
            else
                lblTaxaSLA.Text = "100%";

            #region MapaAtendentes
            StringBuilder SBMapaAtendentes = new StringBuilder();

            int total_operadores_conectados = 0;
            int total_operadores_desconectados = 0;
            int total_operadores_empausa = 0;
            int total_operadores_aguardando = 0;
            int total_operadores_ocupados = 0;

            string ramais_conectados = string.Empty;
            string ramais_desconectados = string.Empty;
            string ramais_empausa = string.Empty;
            string ramais_aguardando = string.Empty;
            string ramais_ocupados = string.Empty;

            foreach (DTO.RelCampanhaDados item in RelCampanha.lstDados.OrderBy(c => c.Ramal))
            {
                string Icone = string.Empty;
                string titulo = string.Empty;

                switch (item.Status)
                {
                    case "Ocupado":
                        Icone = "Msn-Buddy-mobile-icon-48.png";
                        titulo = "Ocupado";
                        total_operadores_ocupados++;
                        ramais_ocupados += item.Ramal + Environment.NewLine;
                        break;

                    case "Em pausa":
                        Icone = "Msn-Buddy-Busy-icon-48.png";
                        titulo = "Em Pausa";
                        total_operadores_empausa++;
                        ramais_empausa += item.Ramal + Environment.NewLine;
                        break;

                    case "Inacessível":
                        Icone = "Msn-Buddy-Offline-icon-48.png";
                        titulo = "Inacessível";
                        total_operadores_desconectados++;
                        ramais_desconectados += item.Ramal + Environment.NewLine;
                        break;

                    case "Aguardando":
                        Icone = "Msn-Buddy-icon-48.png";
                        titulo = "Aguardando";
                        total_operadores_aguardando++;
                        ramais_aguardando += item.Ramal + Environment.NewLine;
                        break;
                }

                SBMapaAtendentes.AppendLine(string.Format(@"
                                            <div class='DivSip' title='{4}'>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <img src='../../Imagens/{1}' >
                                                        </td>
                                                        <td>
                                                            {0} - {3}<br />
                                                            {2}
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>",
                                            item.Ramal,
                                            Icone,
                                            DateTime.Today.AddSeconds(item.Duracao).ToString("HH:mm:ss"),
                                            new BLL.Usuario().ObterObj(item.Ramal, DTO.TipoConsulta.PeloRamal).Nome, titulo));
            }

            ltlMapa.Text = SBMapaAtendentes.ToString();
            lbl_nomecampanha.Text = ddlCampanhas.SelectedItem.Text;

            #endregion

            total_operadores_conectados = total_operadores_aguardando + total_operadores_empausa + total_operadores_ocupados;

            lbl_total_aguardando.Text = total_operadores_aguardando.ToString("000");
            lbl_total_aguardando.ToolTip = ramais_aguardando;

            lbl_total_atendendo.Text = total_operadores_ocupados.ToString("000");
            lbl_total_atendendo.ToolTip = ramais_ocupados;

            lbl_total_conectados.Text = total_operadores_conectados.ToString("000");
            lbl_total_conectados.ToolTip = ramais_conectados;

            lbl_total_empausa.Text = total_operadores_empausa.ToString("000");
            lbl_total_empausa.ToolTip = ramais_empausa;

            lbl_total_desconectado.Text = total_operadores_desconectados.ToString("000");
            lbl_total_desconectado.ToolTip = ramais_desconectados;

            lbl_id_campanha.Text = ddlCampanhas.SelectedItem.Value;
            lbl_nome_campanha.Text = ddlCampanhas.SelectedItem.Text;

            var carga = Silver.BLL.ControleSistema.ObterUltimaCarga(ddlCampanhas.SelectedItem.Value.ToInt64());
            if (carga != null)
            {
                var historico_carga = new BLL.HistoricoCarga().Obter(Path.GetFileName(carga.Valor).Split('_')[1].Split('.')[0].ToInt32());
                lbl_id_carga.Text = historico_carga.Descricao;

                lbl_nome_carga.Text = Enum.GetName(typeof(DTO.SitucaoEventoControleSistema), carga.Situacao).ToUpper().Replace('_', ' ');
                lbl_porcentagem_carga.Text = carga.Porcentagem.ToString() + "%";
                try
                {
                    var TempoEspera = (Int64)new BLL.Campanha().Obter(ddlCampanhas.SelectedValue.ToInt64()).TempoEspera;
                    var idle = (int)new Silver.BLL.Idle().ObterIdle(historico_carga.Id) - TempoEspera;
                    lbl_idle.Text = TimeSpan.FromSeconds(idle).ToString();
                    lbl_TempoEspera.Text = TimeSpan.FromSeconds(TempoEspera).ToString();
                }
                catch
                {
                    lbl_idle.Text = "Indefinido";
                }
            }

            var mailing = Silver.BLL.ControleSistema.ObterUltimoMailing(ddlCampanhas.SelectedItem.Value.ToInt64());
            if (mailing != null)
            {
                lbl_id_mailing.Text = mailing.Id.ToString("0000");
                lbl_inicio_mailing.Text = mailing.DtHrExecucao.ToString();
                lbl_situacao_mailing.Text = Enum.GetName(typeof(DTO.SitucaoEventoControleSistema), mailing.Situacao).ToUpper().Replace('_', ' ');
                lbl_porcentagem_mailing.Text = mailing.Porcentagem.ToString() + "%";
            }

            #region MapaEmEspera
            StringBuilder SBMapaEmEspera = new StringBuilder();
            foreach (DTO.RelCampanhaQueueStatus item in relCampanhaQueueStatus.OrderByDescending(c => c.Wait))
            {
                if (!item.Channel.Contains("DGV/"))
                    return;

                SBMapaEmEspera.AppendLine(string.Format(@"
                                                            <div class='DivSip'>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <img src='../../Imagens/{1}'>
                                                                        </td>
                                                                        <td>
                                                                            Telefone: {0}<br />
                                                                            Espera: {2}
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>", item.CallerIDNum, "Wait-icon-48.png", DateTime.Today.AddSeconds(item.Wait).ToString("HH:mm:ss")));
            }

            ltlAguardando.Text = SBMapaEmEspera.ToString();
            #endregion
        }

        private void CarregarCampanhas()
        {
            var campanhas = new BLL.Campanha().Obter(true);
            ddlCampanhas.SelectedIndexChanged -= ddlCampanhas_SelectedIndexChanged;

            ddlCampanhas.DataSource = campanhas;
            ddlCampanhas.DataTextField = "Nome";
            ddlCampanhas.DataValueField = "Id";
            ddlCampanhas.DataBind();
            ddlCampanhas.SelectedIndexChanged += ddlCampanhas_SelectedIndexChanged;
            ddlCampanhas.Items.Insert(0, string.Empty);
            ddlCampanhas.SelectedIndex = 0;

            ddlCampanhas.DataSource = campanhas;
            ddlCampanhas.DataTextField = "Nome";
            ddlCampanhas.DataValueField = "Id";
            ddlCampanhas.DataBind();
        }

        protected void imb_logout_Click(object sender, ImageClickEventArgs e)
        {
            this.Logout();
        }

        protected void ddlCampanhas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCampanhas.SelectedItem == null) return;

            lbl_opcao_campanha.Text = ddlCampanhas.SelectedItem.Text;

            CarregarGrid();
            MontaMapaOperadores(ddlCampanhas.SelectedValue.ToInt32());
        }

        protected void btMensagem_Click(object sender, EventArgs e)
        {
            Panel_Mensagem.Visible = false;
        }

        protected void timer_update_Tick(object sender, EventArgs e)
        {
            if (ddlCampanhas.SelectedIndex > 0)
                ddlCampanhas_SelectedIndexChanged(ddlCampanhas, new EventArgs());
        }

        protected void btn_filtrar_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlCampanhas.SelectedValue != null)
                ddlCampanhas_SelectedIndexChanged(ddlCampanhas, new EventArgs());
        }

        private void CarregarGrid()
        {
            List<Silver.DTO.ControleSistema> fonte_dados = Silver.BLL.ControleSistema.ListarTodosControles((Session["Usuario"] as DTO.Usuario).Id).Where(c => c.Evento != "Situacao_Fila" && c.Evento != "Situacao_E1").OrderByDescending(c => c.Id).Take(5).ToList();
            if (ddlCampanhas.SelectedItem != null)
            {
                var log_discador = new Silver.BLL.LogDiscador().DiscadorEmExecucao(ddlCampanhas.SelectedValue.ToInt64());
                if (log_discador != null)
                {
                    if (log_discador.Evento.Equals(1))
                        lbl_status_discador.Text = "Em Execução";
                    else
                        lbl_status_discador.Text = "Parado";

                    lbl_hora_atualizacao.Text = log_discador.DataHora.ToString();
                }
                else
                {
                    lbl_status_discador.Text = string.Empty;
                    lbl_hora_atualizacao.Text = string.Empty;
                }
            }

            if (fonte_dados.Count < 5) return;

            if (fonte_dados[0].Evento.Equals("Iniciar_Campanha"))
            {
                lbl_evento_1.ToolTip = new Silver.BLL.Campanha().Obter(fonte_dados[0].Valor.ToInt64()).Nome;
                lbl_evento_1.Text = fonte_dados[0].Evento;
            }
            else lbl_evento_1.Text = fonte_dados[0].Evento;

            if (fonte_dados[1].Evento.Equals("Iniciar_Campanha"))
            {
                lbl_evento_2.Text = fonte_dados[1].Evento;
                lbl_evento_2.ToolTip = new Silver.BLL.Campanha().Obter(fonte_dados[1].Valor.ToInt64()).Nome;
            }
            else lbl_evento_2.Text = fonte_dados[1].Evento;

            if (fonte_dados[2].Evento.Equals("Iniciar_Campanha"))
            {
                lbl_evento_3.Text = fonte_dados[2].Evento;
                lbl_evento_3.ToolTip = new Silver.BLL.Campanha().Obter(fonte_dados[2].Valor.ToInt64()).Nome;
            }
            else lbl_evento_3.Text = fonte_dados[2].Evento;

            if (fonte_dados[3].Evento.Equals("Iniciar_Campanha"))
            {
                lbl_evento_4.Text = fonte_dados[3].Evento;
                lbl_evento_4.ToolTip = new Silver.BLL.Campanha().Obter(fonte_dados[3].Valor.ToInt64()).Nome;
            }
            else lbl_evento_4.Text = fonte_dados[3].Evento;

            if (fonte_dados[4].Evento.Equals("Iniciar_Campanha"))
            {
                lbl_evento_5.Text = fonte_dados[4].Evento;
                lbl_evento_5.ToolTip = new Silver.BLL.Campanha().Obter(fonte_dados[4].Valor.ToInt64()).Nome;
            }
            else lbl_evento_5.Text = fonte_dados[4].Evento;

            lbl_hora_1.Text = fonte_dados[0].DtHrExecucao.ToString();
            lbl_hora_2.Text = fonte_dados[1].DtHrExecucao.ToString();
            lbl_hora_3.Text = fonte_dados[2].DtHrExecucao.ToString();
            lbl_hora_4.Text = fonte_dados[3].DtHrExecucao.ToString();
            lbl_hora_5.Text = fonte_dados[4].DtHrExecucao.ToString();

            lbl_id_1.Text = fonte_dados[0].Id.ToString("000000");
            lbl_id_2.Text = fonte_dados[1].Id.ToString("000000");
            lbl_id_3.Text = fonte_dados[2].Id.ToString("000000");
            lbl_id_4.Text = fonte_dados[3].Id.ToString("000000");
            lbl_id_5.Text = fonte_dados[4].Id.ToString("000000");

            lbl_andamento_1.Text = fonte_dados[0].Porcentagem.ToString().Replace(',', '.');
            lbl_andamento_2.Text = fonte_dados[1].Porcentagem.ToString().Replace(',', '.');
            lbl_andamento_3.Text = fonte_dados[2].Porcentagem.ToString().Replace(',', '.');
            lbl_andamento_4.Text = fonte_dados[3].Porcentagem.ToString().Replace(',', '.');
            lbl_andamento_5.Text = fonte_dados[4].Porcentagem.ToString().Replace(',', '.');

            lbl_situacao_1.Text = Enum.GetName(typeof(DTO.SitucaoEventoControleSistema), fonte_dados[0].Situacao).Replace('_', ' ');
            lbl_situacao_2.Text = Enum.GetName(typeof(DTO.SitucaoEventoControleSistema), fonte_dados[1].Situacao).Replace('_', ' ');
            lbl_situacao_3.Text = Enum.GetName(typeof(DTO.SitucaoEventoControleSistema), fonte_dados[2].Situacao).Replace('_', ' ');
            lbl_situacao_4.Text = Enum.GetName(typeof(DTO.SitucaoEventoControleSistema), fonte_dados[3].Situacao).Replace('_', ' ');
            lbl_situacao_5.Text = Enum.GetName(typeof(DTO.SitucaoEventoControleSistema), fonte_dados[4].Situacao).Replace('_', ' ');

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script type='text/javascript'>");

            sb.AppendLine("$(function(){");
            sb.AppendLine("$('#div_progresso_1').progressbar({value:" + fonte_dados[0].Porcentagem.ToString().Replace(',', '.') + "});");
            sb.AppendLine("$('#div_progresso_2').progressbar({value:" + fonte_dados[1].Porcentagem.ToString().Replace(',', '.') + "});");
            sb.AppendLine("$('#div_progresso_3').progressbar({value:" + fonte_dados[2].Porcentagem.ToString().Replace(',', '.') + "});");
            sb.AppendLine("$('#div_progresso_4').progressbar({value:" + fonte_dados[3].Porcentagem.ToString().Replace(',', '.') + "});");
            sb.AppendLine("$('#div_progresso_5').progressbar({value:" + fonte_dados[4].Porcentagem.ToString().Replace(',', '.') + "});");

            sb.AppendLine("});");
            sb.AppendLine("</script>");

            lit_script_andamento.Text = sb.ToString();


            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), Guid.NewGuid().ToString(), string.Format("SetarValorAndamento(1,{0});", fonte_dados[0].Porcentagem.ToString().Replace(',', '.')), true);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), Guid.NewGuid().ToString(), string.Format("SetarValorAndamento(2,{0});", fonte_dados[1].Porcentagem.ToString().Replace(',', '.')), true);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), Guid.NewGuid().ToString(), string.Format("SetarValorAndamento(3,{0});", fonte_dados[2].Porcentagem.ToString().Replace(',', '.')), true);
        }

        protected void grid_historico_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

    }
}