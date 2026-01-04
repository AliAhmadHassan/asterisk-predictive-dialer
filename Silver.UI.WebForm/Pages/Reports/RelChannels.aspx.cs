using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Silver.DTO;

namespace Silver.UI.Web.Presentation.Pages.Reports
{
    public partial class RelChannels : PaginaBase
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
                //AtualizarStatusCanais();
                MontaMapaOperadores();
            }
        }

        private void MontaMapaOperadores()
        {
            List<DTO.RelChannels> clannels = new BLL.RelChannels().UltimoStatus();

            int e1_ocupado = 0;
            int e1_livre = 0;

            #region MapaAtendentes

            StringBuilder SBMapaAtendentes = new StringBuilder();
            StringBuilder sb_grupos = new StringBuilder();
            StringBuilder sb_grupos_li = new StringBuilder();

            sb_grupos.Append("<div id='tabs_acompanhamento'>");
            sb_grupos.Append("<ul>");
            sb_grupos.Append("");

            var grupos = clannels.GroupBy(a => a.Grp).ToList();
            int contador_tabs = 1;

            foreach (var g in grupos)
            {
                var grupo_selecionado = clannels.Where(c => c.Grp.Equals(g.Key)).FirstOrDefault();
                var canais_conectados = clannels.Where(c => c.Grp.Equals(g.Key)).Where(d => d.Rsrvd.Equals(1) ||
                                                                                        d.Rsrvd.Equals(2) ||
                                                                                        d.Rsrvd.Equals(3) ||
                                                                                        d.Rsrvd.Equals(4)).Count();

                var canais_livres = clannels.Where(c => c.Grp.Equals(g.Key)).Where(d => d.Rsrvd.Equals(0)).Count();

                sb_grupos.Append("<li>");
                sb_grupos.Append(string.Format("<a href='#tabs-{0}'>", contador_tabs));
                sb_grupos.Append(string.Format("Grupo: {0}<br>Operadora: {1}<br>Conectados: {2}<br>Livres: {3}", grupo_selecionado.Grp, grupo_selecionado.Context, canais_conectados.ToString("00"), canais_livres.ToString("00")));
                sb_grupos.Append("</a>");
                sb_grupos.Append("</li>");
                contador_tabs++;
            }

            sb_grupos.Append("</ul>");

            contador_tabs = 1;
            foreach (var g in grupos)
            {
                sb_grupos.Append(string.Format("<div id='tabs-{0}' class='tabs-acompanhamento'>", contador_tabs));
                var grupos_e1 = clannels.Where(h => h.Grp.Equals(g.Key));
                SBMapaAtendentes.Clear();
                foreach (var e in grupos_e1)
                {
                    string Icone = string.Empty;
                    string titulo = string.Empty;
                    if (e.Alrmd != 0)
                    {
                        Icone = "Alarm.png";
                        titulo = "E1 Alarmado";
                    }

                    switch (e.Rsrvd)
                    {
                        case 0:
                            Icone = "shield_protect_off.png";
                            titulo = "Aguardando";
                            e1_livre++;
                            break;

                        default:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            Icone = "shield_protect_on.png";
                            titulo = "Conectado";
                            e1_ocupado++;
                            break;
                    }
                    SBMapaAtendentes.AppendLine(string.Format(@"
                                            <div class='DivSip' title='{2}'>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <img src='../../Imagens/{1}' >
                                                        </td>
                                                        <td>
                                                            {0} - {2}
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>", e.Chan.ToString().PadLeft(2, '0'), Icone, titulo));
                }

                sb_grupos.Append(SBMapaAtendentes.ToString());
                sb_grupos.Append("</div>");
                contador_tabs++;
            }

            sb_grupos.Append("</div>");
            #endregion

            lit_e1.Text = sb_grupos.ToString();
        }

        protected void imb_logout_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btMensagem_Click(object sender, EventArgs e)
        {
            Panel_Mensagem.Visible = false;
        }

        protected void timer_update_Tick(object sender, EventArgs e)
        {
            //AtualizarStatusCanais();
            MontaMapaOperadores();
        }

        protected void btn_filtrar_Click(object sender, ImageClickEventArgs e)
        {
            //AtualizarStatusCanais();
            BLL.ControleSistema.IncluirEvento(new ControleSistema()
            {
                Campanha = 0,
                Valor = "Situação E1",
                Evento = Enum.GetName(typeof(DTO.EventoControleSistema), DTO.EventoControleSistema.Situacao_E1),
                Porcentagem = 0,
                Situacao = (int)DTO.SitucaoEventoControleSistema.Aguardando,
                Solicitante = (Session["Usuario"] as DTO.Usuario).Id
            });

            MontaMapaOperadores();
        }

        private void AtualizarStatusCanais()
        {
            BLL.ControleSistema.IncluirEvento(EventoControleSistema.Situacao_E1, new DTO.ControleSistema { Valor = "", Campanha = 0, Situacao = (int)SitucaoEventoControleSistema.Aguardando, Solicitante = (Session["Usuario"] as DTO.Usuario).Id });
            System.Threading.Thread.Sleep(2000);
        }
    }
}