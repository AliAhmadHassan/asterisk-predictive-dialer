using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Silver.DTO;

namespace Silver.BLL
{

    public class RamalTempoMedio
    {
        public long Ramal { get; set; }

        public TimeSpan TMA { get; set; }

        public TimeSpan TME { get; set; }

        public TimeSpan IDLE { get; set; }
    }

    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 13/08/2013
    /// </summary>
    public class Dashboard
    {
        public Dashboard() { }

        public static decimal TMA(params long[] id_historico)
        {
            return new DAL.Dashboard().TMA(id_historico);
        }


        public static DataTable Cargas(params long[] ids_historico)
        {
            DataTable tabela_retorno = new DataTable("tabela_retorno");

            tabela_retorno.Columns.Add(new DataColumn("descricao", typeof(string)));
            tabela_retorno.Columns.Add(new DataColumn("total", typeof(long)));
            tabela_retorno.PrimaryKey = new DataColumn[] { tabela_retorno.Columns[0] };

            foreach (long hist in ids_historico)
            {
                var tabela = new Silver.DAL.Dashboard().Cargas(hist);
                foreach (DataRow r in tabela.Rows)
                {
                    var result = tabela_retorno.Select(string.Format("descricao = '{0}'", r[0]));
                    if (result.Length > 0) //somar valores
                        r[1] = Convert.ToInt64(result[0][1]) + Convert.ToInt64(r[1]);
                    else
                    {
                        DataRow row = tabela_retorno.NewRow();
                        row[0] = r[0];
                        row[1] = r[1];

                        tabela_retorno.Rows.Add(row);
                    }
                }

                tabela_retorno.Merge(tabela, true, MissingSchemaAction.AddWithKey);
            }
            return tabela_retorno;
        }

        public static DataTable Campanhas(params long[] ids_historico)
        {
            DataTable tabela_retorno = new DataTable("tabela_retorno");
            foreach (long hist in ids_historico)
                tabela_retorno.Merge(new Silver.DAL.Dashboard().Campanhas(hist));

            return tabela_retorno;
        }

        public static DataTable Resultados(params long[] ids_historico)
        {
            DataTable tabela_retorno = new DataTable("tabela_retorno");

            tabela_retorno.Columns.Add(new DataColumn("descricao", typeof(string)));
            tabela_retorno.Columns.Add(new DataColumn("total", typeof(long)));
            tabela_retorno.PrimaryKey = new DataColumn[] { tabela_retorno.Columns[0] };

            foreach (long hist in ids_historico)
            {
                var tabela = new Silver.DAL.Dashboard().Resultados(hist);
                foreach (DataRow r in tabela.Rows)
                {
                    var result = tabela_retorno.Select(string.Format("descricao = '{0}'", r[0]));
                    if (result.Length > 0) //somar valores
                        r[1] = Convert.ToInt64(result[0][1]) + Convert.ToInt64(r[1]);
                    else
                    {
                        DataRow row = tabela_retorno.NewRow();
                        row[0] = r[0];
                        row[1] = r[1];

                        tabela_retorno.Rows.Add(row);
                    }
                }

                tabela_retorno.Merge(tabela, true, MissingSchemaAction.AddWithKey);
            }
            return tabela_retorno;
        }

        public static DataTable Discagem(params long[] ids_historico)
        {
            DataTable tabela_retorno = new DataTable("tabela_retorno");

            tabela_retorno.Columns.Add(new DataColumn("Ramal", typeof(int)));
            tabela_retorno.Columns.Add(new DataColumn("Total", typeof(long)));
            tabela_retorno.PrimaryKey = new DataColumn[] { tabela_retorno.Columns[0] };

            foreach (long hist in ids_historico)
            {
                var tabela = new Silver.DAL.Dashboard().Discagem(hist);
                foreach (DataRow r in tabela.Rows)
                {
                    if (string.IsNullOrEmpty(r[0].ToString()))
                        continue;

                    if (r[0].ToString().Equals("0"))
                        continue;

                    var result = tabela_retorno.Select(string.Format("Ramal = '{0}'", Convert.ToInt32(r[0])));
                    if (result.Length > 0) //somar valores
                        r[1] = Convert.ToInt64(result[0][1]) + Convert.ToInt64(r[1]);
                    else
                    {
                        DataRow row = tabela_retorno.NewRow();
                        row[0] = r[0];
                        row[1] = r[1];

                        tabela_retorno.Rows.Add(row);
                    }
                }
            }
            return tabela_retorno;
        }

        public static RamalTempoMedio TempoMedio(int id_campanha, params long[] ids_historico)
        {
            List<RamalTempoMedio> ramais_tempomedio = new List<RamalTempoMedio>();

            DataTable tabela_retorno_tma = new DataTable("tabela_retorno_tma");
            DataTable tabela_retorno_tme = new DataTable("tabela_retorno_tme");

            tabela_retorno_tma.Columns.Add(new DataColumn("Ramal", typeof(int)));
            tabela_retorno_tma.Columns.Add(new DataColumn("Total", typeof(long)));
            tabela_retorno_tma.Columns.Add(new DataColumn("QtdLigacoes", typeof(long)));
            tabela_retorno_tma.PrimaryKey = new DataColumn[] { tabela_retorno_tma.Columns[0] };

            tabela_retorno_tme.Columns.Add(new DataColumn("Ramal", typeof(int)));
            tabela_retorno_tme.Columns.Add(new DataColumn("Total", typeof(long)));
            tabela_retorno_tme.Columns.Add(new DataColumn("QtdLigacoes", typeof(long)));
            tabela_retorno_tme.PrimaryKey = new DataColumn[] { tabela_retorno_tme.Columns[0] };

            List<DTO.LogRamal> tabela_completa_historico = new List<DTO.LogRamal>();

            TimeSpan tempo_tma = new TimeSpan();
            TimeSpan tempo_idle = new TimeSpan();
            TimeSpan tempo_tme = new TimeSpan();

            long contador_tma = 0;
            long contador_idle = 0;
            long contador_tme = 0;

            foreach (long hist in ids_historico)
            {
                tabela_completa_historico = new List<DTO.LogRamal>(new Silver.DAL.Dashboard().LogRamal(hist));

                var ramais = (from r in tabela_completa_historico
                              select new { Ramal = r.Ramal }).GroupBy(s => s.Ramal).OrderBy(c => c.Key);

                //Evento 6 - Ramal atendeu - Filtro
                List<DTO.LogRamal> ramal_atendeu = new List<DTO.LogRamal>(tabela_completa_historico.Where(r => r.Evento.Equals(6)).OrderBy(r_d => r_d.DataHora));

                //Evento 7 - Ramal desligou - Filtro
                List<DTO.LogRamal> ramal_desligou = new List<DTO.LogRamal>(tabela_completa_historico.Where(r => r.Evento.Equals(7)).OrderBy(r_d => r_d.DataHora));

                //Tempo Médio Atendimento { Desligou - Atendeu = Tempo_logado }
                foreach (var log in ramal_desligou)
                {
                    var hora_atendeu = ramal_atendeu.Where(r => r.TelId == log.TelId).FirstOrDefault();
                    tempo_tma += log.DataHora - hora_atendeu.DataHora;
                    contador_tma++;
                }

                var resultado_real_tma = tempo_tma.TotalSeconds / contador_tma;
                tempo_tma = TimeSpan.FromSeconds(resultado_real_tma);

                var pausas_ramal = new Silver.BLL.UsuarioPausa().Obter(ramal_atendeu[0].DataHora, ramal_desligou[ramal_desligou.Count - 1].DataHora);
                var pausas_discador = new Silver.BLL.LogDiscador().Listar(ramal_atendeu[0].DataHora, ramal_desligou[ramal_desligou.Count - 1].DataHora, id_campanha);

                //Tempo Médio de Espera { Atendeu - Desligou = Tempo_Espera }
                foreach (var log in ramal_atendeu)
                {
                    var hora_desligou = ramal_desligou.Where(r => r.Evento.Equals(7) && r.DataHora < log.DataHora).LastOrDefault();
                    if (hora_desligou != null)
                    {
                        var pausa_no_periodo = pausas_ramal.Where(p => p.Inicio > log.DataHora && p.Inicio < hora_desligou.DataHora);
                        var pausa_discador_periodo = pausas_discador.Where(d => d.DataHora > log.DataHora && d.DataHora < hora_desligou.DataHora && d.Evento.Equals(2));
                        if (pausa_no_periodo.Count() <= 0 || pausa_discador_periodo.Count() <= 0)
                        {
                            tempo_idle += log.DataHora - hora_desligou.DataHora;
                            contador_idle++;
                        }

                        tempo_tme += log.DataHora - hora_desligou.DataHora;
                        contador_tme++;
                    }
                }

                var resultado_real_tme = tempo_tme.TotalSeconds / contador_tme;
                tempo_tme = TimeSpan.FromSeconds(resultado_real_tme);

                var resultado_real_idle = tempo_idle.TotalSeconds / contador_idle;
                tempo_idle = TimeSpan.FromSeconds(resultado_real_idle);

            }

            return new RamalTempoMedio() { TMA = tempo_tma, TME = tempo_tme, IDLE = tempo_idle };
        }

        public static DataTable LinhaTempoRange(params long[] ids_historico)
        {
            DataTable tabela_retorno = new DataTable("tabela_retorno");

            tabela_retorno.Columns.Add(new DataColumn("Hora", typeof(string)));
            tabela_retorno.Columns.Add(new DataColumn("Total", typeof(long)));
            //tabela_retorno.PrimaryKey = new DataColumn[] { tabela_retorno.Columns[0] };

            foreach (long hist in ids_historico)
            {
                var tabela = new Silver.DAL.Dashboard().RangeLinhaTempo(hist);
                foreach (DataRow r in tabela.Rows)
                {
                    var result = tabela_retorno.Select(string.Format("Hora = '{0}'", r[0]));
                    if (result.Length > 0)
                        r[1] = Convert.ToInt64(result[0][1]) + Convert.ToInt64(r[1]);
                    else
                    {
                        DataRow row = tabela_retorno.NewRow();
                        row[0] = r[0];
                        row[1] = r[1];

                        tabela_retorno.Rows.Add(row);
                    }
                }
            }
            return tabela_retorno;
        }

        public static DataTable LinhaTempo(SilverStatus status, params long[] ids_historico)
        {
            DataTable tabela_retorno = new DataTable("tabela_retorno");

            tabela_retorno.Columns.Add(new DataColumn("Horario", typeof(string)));
            tabela_retorno.Columns.Add(new DataColumn("Total", typeof(long)));
            tabela_retorno.PrimaryKey = new DataColumn[] { tabela_retorno.Columns[0] };

            foreach (long hist in ids_historico)
            {
                var tabela = new Silver.DAL.Dashboard().LinhaTempo(hist, status);
                foreach (DataRow r in tabela.Rows)
                {
                    var result = tabela_retorno.Select(string.Format("Horario = '{0}'", r[0]));
                    if (result.Length > 0)
                        r[1] = Convert.ToInt64(result[0][1]) + Convert.ToInt64(r[1]);
                    else
                    {
                        if (string.IsNullOrEmpty(r[0].ToString()))
                            continue;

                        DataRow row = tabela_retorno.NewRow();
                        row[0] = r[0];
                        row[1] = r[1];

                        tabela_retorno.Rows.Add(row);
                    }
                }
            }

            return tabela_retorno;
        }

        #region Versão 2.0.0
        public static int ChamadasRealizada(long id_campanha)
        {
            return new DAL.Dashboard().ChamadasRealizada(id_campanha);
        }

        public static int ChamadasNaoRealizada(long id_campanha)
        {
            return new DAL.Dashboard().ChamadasNaoRealizadas(id_campanha);
        }

        public static int ChamadasPositivas(long id_campanha)
        {
            return new DAL.Dashboard().ChamadasPositivas(id_campanha);
        }

        public static DataTable ListarResultados(params int[] ids_campanhas)
        {
            return new DAL.Dashboard().ListarResultados(ids_campanhas);
        }

        public static DataTable ListarTempoLogado(params int[] ids_campanhas)
        {
            return new DAL.Dashboard().ListarTempoLogado(ids_campanhas);
        }

        public static DataTable ListarTempoEspera(params int[] ids_campanhas)
        {
            return new DAL.Dashboard().ListarTempoEspera(ids_campanhas);
        }
        #endregion

        #region Versao 1.0.0
        public static DataTable ListarCampanhas(params int[] id_campanha)
        {
            DataTable result = new DataTable("Result");
            foreach (var item in id_campanha)
                result.Merge(new DAL.Dashboard().ListarCampanhas(item));

            return result;
        }

        public static DataTable ListarCarga(List<long> ids_historico, int id_campanha)
        {
            return new DAL.Dashboard().ListarCarga(id_campanha, ids_historico);
        }

        public static DataTable ListarTelefones(int id_campanha, SilverStatus status)
        {
            return new DAL.Dashboard().ListarTelefones(id_campanha, status);
        }

        public static DataTable ListarDiscagemOperador(string nome_campanha)
        {
            return new DAL.Dashboard().ListarDiscagemOperador(nome_campanha);
        }

        public static DataTable ListarRangeLinhaTempo(params string[] nome_campanha)
        {
            return new DAL.Dashboard().ListarRangeLinhaTempo(nome_campanha);
        }

        public static DataTable ListarAtendidosLinhaTempo(long TipoTelefone, params string[] nome_campanha)
        {
            return new DAL.Dashboard().ListarAtendidosLinhaTempo(TipoTelefone, nome_campanha);
        }

        public static DataTable ListarNaoAtendidosLinhaTempo(long TipoTelefone, params string[] nome_campanha)
        {
            return new DAL.Dashboard().ListarNaoAtendidosLinhaTempo(TipoTelefone, nome_campanha);
        }

        public static DataTable ListarOcupadoLinhaTempo(string nome_campanha)
        {
            return new DAL.Dashboard().ListarOcupadoLinhaTempo(nome_campanha);
        }

        public static int ListarQtdLigacoes(int id_campanha, SilverStatus status)
        {
            return new DAL.Dashboard().ListarQtdLigacoes(id_campanha, status);
        }

        public static int ListarQuantidadeChamadas(string nome_campanha, StatusTelefoneDashboard status)
        {
            return new DAL.Dashboard().ListarQuantidadeChamadas(nome_campanha, status);
        }

        public static int ListarQuantidadeChamadasRealizadas(long id_campanha)
        {
            return 0;// return new DAL.Dashboard().ListarQuantidadeChamadasRealizadas(id_campanha);
        }

        public static int ListarQuantidadeChamadasNaoRealizadas(long id_campanha)
        {
            return 0;// return new DAL.Dashboard().ListarQuantidadeChamadasNaoRealizadas(id_campanha);
        }

        public static int ListarQuantidadeChamadasPositivas(long id_campanha)
        {
            return 0;// return new DAL.Dashboard().ListarQuantidadeChamadasPositivas(id_campanha);
        }

        public static int QtdLigacaoStatusCarga(int id_campanha, SilverStatus status)
        {
            return new DAL.Dashboard().QtdLigacaoStatusCarga(id_campanha, status);
        }

        public static string[] ListarUsuariosCampanha(params string[] campanhas)
        {
            return new DAL.Dashboard().ListarUsuariosCampanha(campanhas);
        }

        public static int TotalNumerosDiscados(string ramal, params  string[] campanhas)
        {
            return new DAL.Dashboard().TotalNumerosDiscados(ramal, campanhas);
        }

        public static int TotalChamadas(string nome_campanha, DTO.StatusTelefoneDashboard status)
        {

            return 0;
        }
        #endregion
    }
}
