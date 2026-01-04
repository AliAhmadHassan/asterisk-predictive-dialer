using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Silver.BLL;
using System.Reflection;
using Silver.Common;
using Silver.DTO;
using DotNet.Highcharts;
using DotNet.Highcharts.Options;
using System.Drawing;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Enums;
using System.Collections;

namespace Silver.UI.Web.Presentation.Pages.Dashboard
{
    public partial class Dashboard : Page
    {
        private DTO.Usuario usuario_sessao;

        protected void Page_Load(object sender, EventArgs e)
        {
            usuario_sessao = (DTO.Usuario)Session["Usuario"];
            if (usuario_sessao == null)
                Response.Redirect("~/Login.aspx");

            lbNome.Text = usuario_sessao.Nome.ToString();
            lbRamal.Text = usuario_sessao.Ramal.ToString();

            if (!IsPostBack)
            {
                Silver.UI.Web.Presentation.Pages.Dashboard.Tree<DTO.Grupo> tree = new Tree<DTO.Grupo>();

                var grupos = new BLL.Grupo().ObterGruposFilhos((Session["Usuario"] as DTO.Usuario).IdGrupo);
                Session.Add("GrupoHierarquico", grupos);

                tree.DataSource = grupos;
                tree.Id = "Id";
                tree.IdPai = "IdGrupo";
                tree.Descricao = "Nome";

                trv_grupos.Nodes.Add(tree.DataBind());
                trv_grupos.DataBind();

                PreencherListaCarga();
                CarregarCampanhas();
                CarregarSolicitacoes();
            }

            ClientScript.RegisterClientScriptInclude(Guid.NewGuid().ToString(), Page.ResolveClientUrl("~/Pages/Dashboard/js/highcharts.js"));
            ClientScript.RegisterClientScriptInclude(Guid.NewGuid().ToString(), Page.ResolveClientUrl("~/Pages/Dashboard/js/highcharts-more.js"));
            ClientScript.RegisterClientScriptInclude(Guid.NewGuid().ToString(), Page.ResolveClientUrl("~/Pages/Dashboard/js/funnel.js"));

            #region temp
            //Silver.UI.Web.Charts.ConfiguracaoSerie configuracao;
            //var servico = new ChartServico();
            //var configuracoes = new List<Silver.UI.Web.Charts.ConfiguracaoSerie>();

            //configuracao = new Charts.ConfiguracaoSerie();
            //configuracao.Campo = "TOTAL";
            //configuracao.Cor = "gray";
            //configuracao.Nome = "Total";
            //configuracao.Tipo = TipoSerie.Area;
            //configuracoes.Add(configuracao);

            //var configuracoes_chart2 = new List<Silver.UI.Web.Charts.ConfiguracaoSerie>();
            //var configuracao_chart2 = new Charts.ConfiguracaoSerie();

            //configuracao_chart2.Campo = "TOTAL";
            //configuracao_chart2.Cor = "green";
            //configuracao_chart2.Nome = "Total";
            //configuracao_chart2.Tipo = TipoSerie.Barra;
            //configuracoes_chart2.Add(configuracao_chart2);

            //var configuracoes_chart3 = new List<Silver.UI.Web.Charts.ConfiguracaoSerie>();
            //var configuracao_chart3 = new Charts.ConfiguracaoSerie();

            //configuracao_chart3.Campo = "TOTAL";
            //configuracao_chart3.Cor = "red";
            //configuracao_chart3.Nome = "Total";
            //configuracao_chart3.Tipo = TipoSerie.Coluna;
            //configuracoes_chart3.Add(configuracao_chart3);

            //var configuracoes_chart4 = new List<Silver.UI.Web.Charts.ConfiguracaoSerie>();
            //var configuracao_chart4 = new Charts.ConfiguracaoSerie();

            //configuracao_chart4.Campo = "TOTAL";
            //configuracao_chart4.Cor = "blue";
            //configuracao_chart4.Nome = "Total";
            //configuracao_chart4.Tipo = TipoSerie.Linha;
            //configuracoes_chart4.Add(configuracao_chart4);

            //var configuracoes_chart5 = new List<Silver.UI.Web.Charts.ConfiguracaoSerie>();
            //var configuracao_chart5 = new Charts.ConfiguracaoSerie();

            //configuracao_chart5.Campo = "TOTAL";
            //configuracao_chart5.Cor = "tomato";
            //configuracao_chart5.Nome = "Total";
            //configuracao_chart5.Tipo = TipoSerie.Pizza;
            //configuracoes_chart5.Add(configuracao_chart5);

            //var configuracoes_chart6 = new List<Silver.UI.Web.Charts.ConfiguracaoSerie>();
            //var configuracao_chart6 = new Charts.ConfiguracaoSerie();

            //configuracao_chart6.Campo = "TOTAL";
            //configuracao_chart6.Cor = "pink";
            //configuracao_chart6.Nome = "Total";
            //configuracao_chart6.Tipo = TipoSerie.Area;
            //configuracoes_chart6.Add(configuracao_chart6);

            //var configuracoes_chart7 = new List<Silver.UI.Web.Charts.ConfiguracaoSerie>();
            //var configuracao_chart7 = new Charts.ConfiguracaoSerie();

            //configuracao_chart7.Campo = "TOTAL";
            //configuracao_chart7.Cor = "brown";
            //configuracao_chart7.Nome = "Total";
            //configuracao_chart7.Tipo = TipoSerie.Barra;
            //configuracoes_chart7.Add(configuracao_chart7);

            //var configuracoes_chart8 = new List<Silver.UI.Web.Charts.ConfiguracaoSerie>();
            //var configuracao_chart8 = new Charts.ConfiguracaoSerie();

            //configuracao_chart8.Campo = "TOTAL";
            //configuracao_chart8.Cor = "red";
            //configuracao_chart8.Nome = "Total";
            //configuracao_chart8.Tipo = TipoSerie.Coluna;


            //var configuracoes_chart10 = new List<Silver.UI.Web.Charts.ConfiguracaoSerie>();
            //var configuracao_chart10 = new Charts.ConfiguracaoSerie();

            //configuracao_chart10.Campo = "TOTAL";
            //configuracao_chart10.Cor = "green";
            //configuracao_chart10.Nome = "Total";
            //configuracao_chart10.Tipo = TipoSerie.Area;
            //configuracoes_chart8.Add(configuracao_chart10);
            //configuracoes_chart8.Add(configuracao_chart8);



            //var configuracoes_chart9 = new List<Silver.UI.Web.Charts.ConfiguracaoSerie>();
            //var configuracao_chart9 = new Charts.ConfiguracaoSerie();

            //configuracao_chart9.Campo = "TOTAL";
            //configuracao_chart9.Cor = "green";
            //configuracao_chart9.Nome = "Total";
            //configuracao_chart9.Tipo = TipoSerie.Linha;
            //configuracoes_chart9.Add(configuracao_chart9);



            //DataTable dt = new DataTable("Chart");
            //dt.Columns.Add(new DataColumn("Vencimento", typeof(int)));
            //dt.Columns.Add(new DataColumn("Total", typeof(int)));

            //for (int i = 0; i < 5; i++)
            //{
            //    DataRow dr = dt.NewRow();
            //    dr[0] = i * new Random().Next();
            //    dr[1] = i * new Random().Next();
            //    dt.Rows.Add(dr);
            //}

            //Chart1.Grafico = servico.ObjGrafico("Chart1", dt, configuracoes, "Vencimento", false, "x");
            //Chart1.Altura = "150px";
            //Chart1.Largura = "150px";
            //Chart1.DataBind();

            ////Chart2.Grafico = servico.ObjGrafico("Chart2", dt, configuracoes_chart2, "Vencimento", false, "x");
            ////Chart2.Altura = "150px";
            ////Chart2.Largura = "150px";
            ////Chart2.DataBind();

            ////Chart3.Grafico = servico.ObjGrafico("Chart3", dt, configuracoes_chart3, "Vencimento", false, "x");
            ////Chart3.Altura = "150px";
            ////Chart3.Largura = "150px";
            ////Chart3.DataBind();

            ////Chart4.Grafico = servico.ObjGrafico("Chart4", dt, configuracoes_chart4, "Vencimento", false, "x");
            ////Chart4.Altura = "150px";
            ////Chart4.Largura = "150px";
            ////Chart4.DataBind();

            ////Chart5.Grafico = servico.ObjGrafico("Chart5", dt, configuracoes_chart5, "Vencimento", false, "x");
            ////Chart5.Altura = "150px";
            ////Chart5.Largura = "150px";
            ////Chart5.DataBind();

            ////Chart6.Grafico = servico.ObjGrafico("Chart6", dt, configuracoes_chart6, "Vencimento", false, "x");
            ////Chart6.Altura = "150px";
            ////Chart6.Largura = "150px";
            ////Chart6.DataBind();

            ////Chart7.Grafico = servico.ObjGrafico("Chart7", dt, configuracoes_chart7, "Vencimento", false, "x");
            ////Chart7.Altura = "150px";
            ////Chart7.Largura = "150px";
            ////Chart7.DataBind();

            ////Chart8.Grafico = servico.ObjGrafico("Chart8", dt, configuracoes_chart8, "Vencimento", false, "x");
            ////Chart8.Altura = "150px";
            ////Chart8.Largura = "150px";
            ////Chart8.DataBind();

            ////Chart9.Grafico = servico.ObjGrafico("Chart9", dt, configuracoes_chart9, "Vencimento", false, "x");
            ////Chart9.Altura = "150px";
            ////Chart9.Largura = "150px";
            ////Chart9.DataBind();

            ////Chart10.Grafico = servico.ObjGrafico("Chart10", dt, configuracoes_chart7, "Vencimento", false, "x");
            ////Chart10.Altura = "150px";
            ////Chart10.Largura = "150px";
            ////Chart10.DataBind();

            ////graficoQuebra.Altura = "150px";
            ////graficoQuebra.Largura = "150px";
            ////graficoQuebra.Chart = new Graficos_BLL().objGrafico(graficoQuebra.ID, new DataTable(), listDefineSeries, "VENCIMENTO", false, "x");
            ////graficoQuebra.DataBind(); 
            #endregion
        }

        protected void CarregarCampanhas()
        {
            ddl_campanhas.DataSource = RetornarCampanhasGrupo((Session["Usuario"] as DTO.Usuario).IdGrupo);
            ddl_campanhas.DataTextField = "Nome";
            ddl_campanhas.DataValueField = "Id";
            ddl_campanhas.DataBind();
        }

        protected void CarregarSolicitacoes()
        {
            //dgv_solicitacoes.DataSource = Silver.BLL.ControleSistema.ListarControles(usuario_sessao.Id);
            //dgv_solicitacoes.DataBind();
        }

        protected void imb_logout_Click(object sender, ImageClickEventArgs e)
        {
            this.Logout();
        }

        protected void lnk_logout_Click(object sender, EventArgs e)
        {
            this.Logout();
        }

        protected void trv_grupos_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (trv_grupos.SelectedValue == null) return;
            Session["CampanhaHierarquica"] = RetornarCampanhasGrupo(trv_grupos.SelectedValue.ToInt64());
            PreencherListaCarga();

            lbl_gruposelecionado.Text = trv_grupos.SelectedNode.Text;
        }

        private static List<DTO.Campanha> RetornarCampanhasGrupo(long grupo_selecionado)
        {
            var grupos_filhos = new Silver.BLL.Grupo().ObterGruposFilhos(grupo_selecionado);
            var ids_grupos = new List<int>();

            foreach (DTO.Grupo g in grupos_filhos)
                ids_grupos.Add((int)g.Id);

            var campanhas_grupo = new Silver.BLL.Campanha().SelectPeloGrupo(ids_grupos.ToArray());
            return campanhas_grupo;
        }

        private void PreencherListaCarga()
        {
            List<DTO.Campanha> campanhas = new List<DTO.Campanha>();

            if (Session["CampanhaHierarquica"] != null)
                campanhas = Session["CampanhaHierarquica"] as List<DTO.Campanha>;
            else
                campanhas = RetornarCampanhasGrupo((Session["Usuario"] as DTO.Usuario).IdGrupo);

            var servico_historico = new Silver.BLL.HistoricoCarga();
            var cargas = new List<DTO.HistoricoCarga>();

            foreach (DTO.Campanha c in campanhas)
                cargas.AddRange(servico_historico.ListarCampanha(c.Id));

            Session["CargaHierarquica"] = cargas;

            lst_carga.DataSource = cargas;
            lst_carga.DataValueField = "Id";
            lst_carga.DataTextField = "Descricao";
            lst_carga.DataBind();

            ddl_campanhas.DataSource = campanhas;
            ddl_campanhas.DataTextField = "Nome";
            ddl_campanhas.DataValueField = "Id";
            ddl_campanhas.DataBind();
        }

        private void PreencherCharts()
        {
            var campanhas_grupo = Session["CampanhaHierarquica"] as List<DTO.Campanha>;
            List<long> ids_historico = new List<long>();

            foreach (ListItem item in lst_carga.Items)
                if (item.Selected)
                    ids_historico.Add(item.Value.ToInt64());

            var ids_campanhas = new List<Int64>();
            foreach (var camp in campanhas_grupo)
                ids_campanhas.Add((int)camp.Id);

            CriarChartCarga(ids_historico);

            #region MyRegion

            CriarChartCampanhas(ids_historico);

            CriarChartResultados(campanhas_grupo, ids_historico);

            #region Chart Custos


            #endregion

            CriarChartAgressividade(campanhas_grupo);

            CriarChartDiscagemOperador(ids_historico.ToArray());

            CriarChartLinhaTempo(ids_historico.ToArray());

            CriarChartTM(campanhas_grupo.ToArray(), ids_historico.ToArray());

            #endregion

        }

        private void CriarChartTM(DTO.Campanha[] campanhas, params long[] ids_historico)
        {
            var table_result = BLL.Dashboard.TMA(ids_historico);
            CriarChartIDLE(Convert.ToInt64(new Silver.BLL.Idle().ObterIdle(ids_historico)));


            Highcharts chart_tma = null;
            #region MyRegion

            chart_tma = new Highcharts("chart_tempologado")
                   .InitChart(new Chart
                   {
                       Type = ChartTypes.Gauge,
                       PlotBackgroundColor = null,
                       PlotBackgroundImage = null,
                       PlotBorderWidth = 0,
                       PlotShadow = false,
                       Height = 300,
                       ZoomType = ZoomTypes.Xy,
                   })
                   .SetTitle(new Title { Text = "TMA em segundos" })
                   .SetPane(new Pane
                   {
                       StartAngle = -150,
                       EndAngle = 150,
                       Background = new[]
                            {
                                new BackgroundObject
                                    {
                                        BackgroundColor = new BackColorOrGradient(new Gradient
                                            {
                                                LinearGradient = new[] { 0, 0, 0, 1 },
                                                Stops = new object[,] { { 0, "#FFF" }, { 1, "#333" } }
                                            }),
                                        BorderWidth = new PercentageOrPixel(0),
                                        OuterRadius = new PercentageOrPixel(109, true)
                                    },
                                new BackgroundObject
                                    {
                                        BackgroundColor = new BackColorOrGradient(new Gradient
                                            {
                                                LinearGradient = new[] { 0, 0, 0, 1 },
                                                Stops = new object[,] { { 0, "#333" }, { 1, "#FFF" } }
                                            }),
                                        BorderWidth = new PercentageOrPixel(1),
                                        OuterRadius = new PercentageOrPixel(107, true)
                                    },
                                new BackgroundObject(),
                                new BackgroundObject
                                    {
                                        BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#DDD")),
                                        BorderWidth = new PercentageOrPixel(0),
                                        OuterRadius = new PercentageOrPixel(105, true),
                                        InnerRadius = new PercentageOrPixel(103, true)
                                    }
                            }
                   })
                   .SetYAxis(new YAxis
                   {
                       Min = 0,
                       Max = Convert.ToInt32(table_result + 60),

                       MinorTickWidth = 1,
                       MinorTickLength = 12,
                       MinorTickPosition = TickPositions.Inside,
                       MinorTickColor = ColorTranslator.FromHtml("#666"),
                       TickPixelInterval = 60,
                       TickWidth = 2,
                       TickPosition = TickPositions.Inside,
                       TickLength = 10,
                       TickColor = ColorTranslator.FromHtml("#666"),
                       Labels = new YAxisLabels
                       {
                           Step = 1,
                       },
                       Title = new YAxisTitle { Text = "TMA" },
                       PlotBands = new[]
                            {
                                new YAxisPlotBands { From = 0, To = 120, Color = ColorTranslator.FromHtml("#55BF3B") },
                                new YAxisPlotBands { From = 120, To = 160, Color = ColorTranslator.FromHtml("#DDDF0D") },
                                new YAxisPlotBands { From = 160, To = 200, Color = ColorTranslator.FromHtml("#DF5353") }
                            }
                   })
                   .SetSeries(new Series
                   {
                       Name = "TMA em segundos",
                       Data = new Data(new object[] { Convert.ToInt32(table_result).ToString() })
                   });

            #endregion
            lit_tempologado.Text = chart_tma.ToHtmlString();

            //CriarChartTME((long)table_result.IDLE.TotalSeconds);

        }

        private void CriarChartTME(long tme)
        {
            Highcharts chart_tme = null;
            #region MyRegion
            chart_tme = new Highcharts("chart_tme")
                   .InitChart(new Chart
                   {
                       Type = ChartTypes.Gauge,
                       PlotBackgroundColor = null,
                       PlotBackgroundImage = null,
                       PlotBorderWidth = 0,
                       PlotShadow = false,
                       Height = 300,
                       ZoomType = ZoomTypes.Xy,
                   })
                   .SetTitle(new Title { Text = "TME em segundos" })
                   .SetPane(new Pane
                   {
                       StartAngle = -150,
                       EndAngle = 150,
                       Background = new[]
                            {
                                new BackgroundObject
                                    {
                                        BackgroundColor = new BackColorOrGradient(new Gradient
                                            {
                                                LinearGradient = new[] { 0, 0, 0, 1 },
                                                Stops = new object[,] { { 0, "#FFF" }, { 1, "#333" } }
                                            }),
                                        BorderWidth = new PercentageOrPixel(0),
                                        OuterRadius = new PercentageOrPixel(109, true)
                                    },
                                new BackgroundObject
                                    {
                                        BackgroundColor = new BackColorOrGradient(new Gradient
                                            {
                                                LinearGradient = new[] { 0, 0, 0, 1 },
                                                Stops = new object[,] { { 0, "#333" }, { 1, "#FFF" } }
                                            }),
                                        BorderWidth = new PercentageOrPixel(1),
                                        OuterRadius = new PercentageOrPixel(107, true)
                                    },
                                new BackgroundObject(),
                                new BackgroundObject
                                    {
                                        BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#DDD")),
                                        BorderWidth = new PercentageOrPixel(0),
                                        OuterRadius = new PercentageOrPixel(105, true),
                                        InnerRadius = new PercentageOrPixel(103, true)
                                    }
                            }
                   })
                   .SetYAxis(new YAxis
                   {
                       Min = 0,
                       Max = tme + 60,

                       MinorTickWidth = 1,
                       MinorTickLength = 12,
                       MinorTickPosition = TickPositions.Inside,
                       MinorTickColor = ColorTranslator.FromHtml("#666"),
                       TickPixelInterval = 60,
                       TickWidth = 2,
                       TickPosition = TickPositions.Inside,
                       TickLength = 10,
                       TickColor = ColorTranslator.FromHtml("#666"),
                       Labels = new YAxisLabels
                       {
                           Step = 1,
                       },
                       Title = new YAxisTitle { Text = "TME" },
                       PlotBands = new[]
                            {
                                new YAxisPlotBands { From = 0, To = 120, Color = ColorTranslator.FromHtml("#55BF3B") },
                                new YAxisPlotBands { From = 120, To = 160, Color = ColorTranslator.FromHtml("#DDDF0D") },
                                new YAxisPlotBands { From = 160, To = 200, Color = ColorTranslator.FromHtml("#DF5353") }
                            }
                   })
                   .SetSeries(new Series
                   {
                       Name = "TME em segundos",
                       Data = new Data(new object[] { tme.ToString() })
                   });

            lit_tempoespera.Text = chart_tme.ToHtmlString();
            #endregion
        }

        private void CriarChartIDLE(long idle)
        {
            Highcharts chart_idle = null;
            #region MyRegion
            chart_idle = new Highcharts("chart_idle")
                   .InitChart(new Chart
                   {
                       Type = ChartTypes.Gauge,
                       PlotBackgroundColor = null,
                       PlotBackgroundImage = null,
                       PlotBorderWidth = 0,
                       PlotShadow = false,
                       Height = 300,
                       ZoomType = ZoomTypes.Xy,
                   })
                   .SetTitle(new Title { Text = "IDLE em segundos" })
                   .SetPane(new Pane
                   {
                       StartAngle = -150,
                       EndAngle = 150,
                       Background = new[]
                            {
                                new BackgroundObject
                                    {
                                        BackgroundColor = new BackColorOrGradient(new Gradient
                                            {
                                                LinearGradient = new[] { 0, 0, 0, 1 },
                                                Stops = new object[,] { { 0, "#FFF" }, { 1, "#333" } }
                                            }),
                                        BorderWidth = new PercentageOrPixel(0),
                                        OuterRadius = new PercentageOrPixel(109, true)
                                    },
                                new BackgroundObject
                                    {
                                        BackgroundColor = new BackColorOrGradient(new Gradient
                                            {
                                                LinearGradient = new[] { 0, 0, 0, 1 },
                                                Stops = new object[,] { { 0, "#333" }, { 1, "#FFF" } }
                                            }),
                                        BorderWidth = new PercentageOrPixel(1),
                                        OuterRadius = new PercentageOrPixel(107, true)
                                    },
                                new BackgroundObject(),
                                new BackgroundObject
                                    {
                                        BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#DDD")),
                                        BorderWidth = new PercentageOrPixel(0),
                                        OuterRadius = new PercentageOrPixel(105, true),
                                        InnerRadius = new PercentageOrPixel(103, true)
                                    }
                            }
                   })
                   .SetYAxis(new YAxis
                   {
                       Min = 0,
                       Max = idle + 60,

                       MinorTickWidth = 1,
                       MinorTickLength = 12,
                       MinorTickPosition = TickPositions.Inside,
                       MinorTickColor = ColorTranslator.FromHtml("#666"),
                       TickPixelInterval = 60,
                       TickWidth = 2,
                       TickPosition = TickPositions.Inside,
                       TickLength = 10,
                       TickColor = ColorTranslator.FromHtml("#666"),
                       Labels = new YAxisLabels
                       {
                           Step = 1,
                       },
                       Title = new YAxisTitle { Text = "IDLE" },
                       PlotBands = new[]
                            {
                                new YAxisPlotBands { From = 0, To = 120, Color = ColorTranslator.FromHtml("#55BF3B") },
                                new YAxisPlotBands { From = 120, To = 160, Color = ColorTranslator.FromHtml("#DDDF0D") },
                                new YAxisPlotBands { From = 160, To = 200, Color = ColorTranslator.FromHtml("#DF5353") }
                            }
                   })
                   .SetSeries(new Series
                   {
                       Name = "IDLE em segundos",
                       Data = new Data(new object[] { idle.ToString() })
                   });

            #endregion

            lit_idle.Text = chart_idle.ToHtmlString();
        }

        private List<int> ObterIdsCampanha(List<DTO.Campanha> campanhas)
        {
            var ids_campanhas = new List<Int32>();
            foreach (var camp in campanhas)
                ids_campanhas.Add((int)camp.Id);

            return ids_campanhas;
        }

        private List<string> ObterNomeCampanha(List<DTO.Campanha> campanhas_grupo)
        {
            var nome_campanhas = new List<string>();

            foreach (var c in campanhas_grupo)
                nome_campanhas.Add(c.Nome.Trim());
            return nome_campanhas;
        }

        private void CriarChartLinhaTempo(params long[] ids_historico)
        {
            var horarios = new string[] 
            { 
                    "08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30", "13:00", 
                    "13:30", "14:00", "14:30", "15:00", "15:30", "16:00", "16:30", "17:00", "17:30", "18:00", "18:30", "19:00", "19:30", "20:00"
            };

            #region Range

            var result_range_linhatempo = BLL.Dashboard.LinhaTempoRange(ids_historico);
            object[] serie_range = new object[horarios.Length];
            foreach (DataRow item in result_range_linhatempo.Rows)
            {
                if (string.IsNullOrEmpty(item[0].ToString())) continue;
                for (int i = 0; i < horarios.Length; i++)
                {
                    if (item[0].ToString().Trim().Equals(horarios[i]))
                    {
                        serie_range[i] = item[1].ToString();
                        continue;
                    }
                }
            }
            var serie_empty = serie_range.Where(s => s == null).ToList();
            for (int i = 0; i < serie_empty.Count; i++)
                serie_empty[i] = 0;

            #endregion

            #region Residencial
            var result_residencial_linhatempo = BLL.Dashboard.LinhaTempo((SilverStatus)1, ids_historico);
            object[] serie_residencial = new object[horarios.Length];
            foreach (DataRow item in result_residencial_linhatempo.Rows)
            {
                if (string.IsNullOrEmpty(item[0].ToString())) continue;
                for (int i = 0; i < horarios.Length; i++)
                {
                    if (item[0].ToString().Trim().Equals(horarios[i]))
                    {
                        serie_residencial[i] = item[1].ToString();
                        continue;
                    }
                }
            }
            serie_empty = serie_residencial.Where(s => s == null).ToList();
            for (int i = 0; i < serie_empty.Count - 1; i++)
                serie_empty[i] = 0;

            #endregion

            #region Celular

            var result_celular_linhatempo = BLL.Dashboard.LinhaTempo((SilverStatus)2, ids_historico);
            object[] serie_celular = new object[horarios.Length];

            foreach (DataRow item in result_celular_linhatempo.Rows)
            {
                if (string.IsNullOrEmpty(item[0].ToString())) continue;
                for (int i = 0; i < horarios.Length; i++)
                {
                    if (item[0].ToString().Trim().Equals(horarios[i]))
                    {
                        serie_celular[i] = item[1].ToString();
                        continue;
                    }
                }
            }
            serie_empty = serie_celular.Where(s => s == null).ToList();
            for (int i = 0; i < serie_empty.Count - 1; i++)
                serie_empty[i] = 0;

            #endregion

            #region Comercial

            var result_comercial_linhatempo = BLL.Dashboard.LinhaTempo((SilverStatus)3, ids_historico);
            object[] serie_comercial = new object[horarios.Length];

            foreach (DataRow item in result_comercial_linhatempo.Rows)
            {
                if (string.IsNullOrEmpty(item[0].ToString())) continue;
                for (int i = 0; i < horarios.Length; i++)
                {
                    if (item[0].ToString().Trim().Equals(horarios[i]))
                    {
                        serie_comercial[i] = item[1].ToString();
                        continue;
                    }
                }
            }
            serie_empty = serie_comercial.Where(s => s != null).ToList();
            for (int i = 0; i < serie_empty.Count - 1; i++)
                serie_empty[i] = 0;

            #endregion

            #region Chart

            Highcharts chart_linhatempo = new Highcharts("chart_linhatempo")
                  .InitChart(new Chart { Type = ChartTypes.Area, MarginRight = 100, Height = 300, ZoomType = ZoomTypes.Xy, PlotBorderWidth = 1, })
                  .SetTitle(new Title { Text = "Linha Tempo", X = -50 })
                  .SetPlotOptions(new PlotOptions
                  {
                      Series = new PlotOptionsSeries
                      {
                          DataLabels = new PlotOptionsSeriesDataLabels
                          {
                              Enabled = true,
                              Format = "<b>{point.name}</b> ({point.y:,.0f})",
                              Color = Color.FromName("black")
                          }
                      }
                  })
                  .SetOptions(new GlobalOptions { Global = new DotNet.Highcharts.Options.Global { UseUTC = false } })
                  .SetXAxis(new XAxis { Type = AxisTypes.Category, Categories = horarios })
                  .SetLegend(new Legend { Enabled = true })
                  .SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.series.name +'</b><br/>Horário: '+ this.x +'<br>Total: '+ this.y; }" })
                  .SetSeries(new[] 
                   { 
                          new Series { Name = "Quantidade", Data =new Data(serie_range) }, 
                          new Series { Name = "Residencial", Data = new Data(serie_residencial), Color = Color.Green , Type = ChartTypes.Line},
                          new Series { Name = "Celular", Data = new Data(serie_celular), Color = Color.OrangeRed , Type = ChartTypes.Line},
                          new Series { Name = "Comercial", Data = new Data(serie_comercial), Color = Color.Violet, Type = ChartTypes.Line},
                  });
            #endregion

            lit_linhatempo.Text = chart_linhatempo.ToHtmlString();
        }

        private void CriarChartDiscagemOperador(params long[] ids_historico)
        {
            #region Chart Discagem Operador
            var result_discagem = Silver.BLL.Dashboard.Discagem(ids_historico.ToArray());

            object[] series_discagemoperador = new object[result_discagem.Rows.Count];
            for (int i = 0; i < result_discagem.Rows.Count; i++)
                series_discagemoperador[i] = new object[] { "\"" + result_discagem.Rows[i][0].ToString() + "\"", result_discagem.Rows[i][1].ToString() };

            Highcharts chart_discagemoperador = new Highcharts("chart_discagemoperador")
                 .InitChart(new Chart { PlotShadow = false, PlotBackgroundColor = null, PlotBorderWidth = null, Height = 300, ZoomType = ZoomTypes.Xy })
                 .SetTitle(new Title { Text = "Discagem Operador" })
                 .SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.percentage +' %'; }", Crosshairs = new Crosshairs(new CrosshairsForamt { }, new CrosshairsForamt()) { } })
                 .SetPlotOptions(new PlotOptions
                 {
                     Pie = new PlotOptionsPie
                     {
                         AllowPointSelect = true,
                         Cursor = Cursors.Pointer,
                         DataLabels = new PlotOptionsPieDataLabels { Enabled = false },
                         ShowInLegend = true
                     }
                 })
                 .SetSeries(new Series
                 {
                     Type = ChartTypes.Pie,
                     Name = "Chamadas por Operador",
                     Data = new Data(series_discagemoperador)
                 });

            lit_discagemoperador.Text = chart_discagemoperador.ToHtmlString();
            #endregion
        }

        private void CriarChartAgressividade(List<DTO.Campanha> campanhas_grupo)
        {
            #region Chart Agressividade

            Highcharts chart_agressividade = null;
            List<ChartAgressividade> charts_agressividade = new List<ChartAgressividade>();

            foreach (DTO.Campanha camp in campanhas_grupo)
            {
                ChartAgressividade chart1 = new ChartAgressividade();
                chart1.IdCampanha = camp.Id;
                chart1.NomeDivHTML = string.Format("chart_agressividade_{0}", camp.Id);

                #region MyRegion
                chart_agressividade = new Highcharts(chart1.NomeDivHTML)
                       .InitChart(new Chart
                       {
                           Type = ChartTypes.Gauge,
                           PlotBackgroundColor = null,
                           PlotBackgroundImage = null,
                           PlotBorderWidth = 0,
                           PlotShadow = false,
                           Height = 150,
                           ZoomType = ZoomTypes.Xy,
                       })
                       .SetTitle(new Title { Text = camp.Nome.ToUpper() })
                       .SetPane(new Pane
                       {
                           StartAngle = -150,
                           EndAngle = 150,
                           Background = new[]
                            {
                                new BackgroundObject
                                    {
                                        BackgroundColor = new BackColorOrGradient(new Gradient
                                            {
                                                LinearGradient = new[] { 0, 0, 0, 1 },
                                                Stops = new object[,] { { 0, "#FFF" }, { 1, "#333" } }
                                            }),
                                        BorderWidth = new PercentageOrPixel(0),
                                        OuterRadius = new PercentageOrPixel(109, true)
                                    },
                                new BackgroundObject
                                    {
                                        BackgroundColor = new BackColorOrGradient(new Gradient
                                            {
                                                LinearGradient = new[] { 0, 0, 0, 1 },
                                                Stops = new object[,] { { 0, "#333" }, { 1, "#FFF" } }
                                            }),
                                        BorderWidth = new PercentageOrPixel(1),
                                        OuterRadius = new PercentageOrPixel(107, true)
                                    },
                                new BackgroundObject(),
                                new BackgroundObject
                                    {
                                        BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#DDD")),
                                        BorderWidth = new PercentageOrPixel(0),
                                        OuterRadius = new PercentageOrPixel(105, true),
                                        InnerRadius = new PercentageOrPixel(103, true)
                                    }
                            }
                       })
                       .SetYAxis(new YAxis
                       {
                           Min = 0,
                           Max = 10,

                           MinorTickWidth = 1,
                           MinorTickLength = 10,
                           MinorTickPosition = TickPositions.Inside,
                           MinorTickColor = ColorTranslator.FromHtml("#666"),
                           TickPixelInterval = 60,
                           TickWidth = 2,
                           TickPosition = TickPositions.Inside,
                           TickLength = 10,
                           TickColor = ColorTranslator.FromHtml("#666"),
                           Labels = new YAxisLabels
                           {
                               Step = 1,
                           },
                           Title = new YAxisTitle { Text = "Agressi" },
                           PlotBands = new[]
                            {
                                new YAxisPlotBands { From = 0, To = 120, Color = ColorTranslator.FromHtml("#55BF3B") },
                                new YAxisPlotBands { From = 120, To = 160, Color = ColorTranslator.FromHtml("#DDDF0D") },
                                new YAxisPlotBands { From = 160, To = 200, Color = ColorTranslator.FromHtml("#DF5353") }
                            }
                       })
                       .SetSeries(new Series
                       {
                           Name = camp.Nome,
                           Data = new Data(new object[] { camp.Agressividade })
                       });

                #endregion
                chart1.HightchartHTML = chart_agressividade.ToHtmlString();
                charts_agressividade.Add(chart1);
            }

            dtl_agressividade.DataSource = charts_agressividade;
            dtl_agressividade.DataBind();

            #endregion
        }

        private void CriarChartResultados(List<DTO.Campanha> campanhas_grupo, List<long> ids_historico)
        {
            #region Chart Resultados

            var nomes = ObterNomeCampanha(campanhas_grupo).ToArray();


            var dt_resultados = BLL.Dashboard.Resultados(ids_historico.ToArray());

            string[] status_descricao = new string[dt_resultados.Rows.Count];
            object[] status_quantidade = new object[dt_resultados.Rows.Count];
            Series[] series_resultados = new Series[dt_resultados.Rows.Count];

            for (int i = 0; i < dt_resultados.Rows.Count; i++)
            {
                status_descricao[i] = dt_resultados.Rows[i]["Descricao"].ToString();
                status_quantidade[i] = dt_resultados.Rows[i]["total"].ToString();
            }

            for (int i = 0; i < status_descricao.Length; i++)
            {
                series_resultados[i] = new Series
                {
                    Name = status_descricao[i],
                    Data = new Data(new object[] { status_quantidade[i] })
                };
            }

            Highcharts chart_resultado = new Highcharts("chart_resultado")
               .InitChart(new Chart { DefaultSeriesType = ChartTypes.Bar, Height = 300, ZoomType = ZoomTypes.Xy })
               .SetTitle(new Title { Text = "Resultados" })
               .SetXAxis(new XAxis { Categories = new string[] { "Total" }, DateTimeLabelFormats = new DateTimeLabel() { } })
               .SetYAxis(new YAxis { Min = 0, Title = new YAxisTitle { Text = "Quantidade de ligações" } })
               .SetPlotOptions(new PlotOptions { Column = new PlotOptionsColumn { Stacking = Stackings.Normal } })
               .SetSeries(series_resultados);

            lit_resultado.Text = chart_resultado.ToHtmlString();

            #endregion
        }

        private void CriarChartCampanhas(List<long> ids_historicos)
        {
            #region Chart Campanhas

            var result_campanha = BLL.Dashboard.Campanhas(ids_historicos.ToArray());
            var lista_campanha = new List<object[]>();

            foreach (DataRow item in result_campanha.Rows)
                lista_campanha.Add(new object[] { item[0].ToInt32(), item[1].ToInt32() });

            Data dados_campanha = new Data(lista_campanha.ToArray());

            var campanhas_grupo = Session["CampanhaHierarquica"] as List<DTO.Campanha>;
            var nome_campanhas = ObterNomeCampanha(campanhas_grupo);

            string[] nomes = nome_campanhas.ToArray();
            Series[] series = new Series[campanhas_grupo.Count];

            for (int i = 0; i < campanhas_grupo.Count; i++)
            {
                var serie_realizada = BLL.Dashboard.ChamadasRealizada((int)campanhas_grupo[i].Id);
                var serie_naorealizada = BLL.Dashboard.ChamadasNaoRealizada((int)campanhas_grupo[i].Id);
                var serie_positiva = BLL.Dashboard.ChamadasPositivas((int)campanhas_grupo[i].Id);

                series[i] = new Series
                {
                    Name = campanhas_grupo[i].Nome,
                    Data = new Data(
                                new object[] 
                                { 
                                    serie_realizada,
                                    serie_naorealizada,
                                    serie_positiva
                                })
                };
            }

            Highcharts chart_campanha = new Highcharts("chart_campanha")
               .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column, Height = 300, ZoomType = ZoomTypes.Xy })
               .SetTitle(new Title { Text = "Campanhas" })
               .SetXAxis(new XAxis { Categories = new[] { "Realizadas", "Não Realizadas", "Positivas" } })
               .SetYAxis(new YAxis { Min = 0, Title = new YAxisTitle { Text = "Quantidade de ligações" } })
               .SetPlotOptions(new PlotOptions { Column = new PlotOptionsColumn { Stacking = Stackings.Normal } })
               .SetSeries(series);
            lit_campanha.Text = chart_campanha.ToHtmlString();
            #endregion

            //return nomes;
        }

        private void CriarChartCarga(List<long> ids_historico)
        {
            #region Carga

            DataTable tabela_cargas = new DataTable("Carga");

            var result_carga = BLL.Dashboard.Cargas(ids_historico.ToArray());
            var lista_carga = new List<object[]>();

            foreach (DataRow item in result_carga.Rows)
                lista_carga.Add(new object[] { item[0].ToString(), item[1].ToInt32() });

            Data dados_carga = new Data(lista_carga.ToArray());

            Highcharts chart_carga = new Highcharts("chart_carga")
               .InitChart(new Chart { Type = ChartTypes.Funnel, MarginRight = 100, Height = 300, ZoomType = ZoomTypes.Xy })
               .SetTitle(new Title { Text = "Tipos de Telefone", X = -50 })
               .SetPlotOptions(new PlotOptions
               {
                   Series = new PlotOptionsSeries
                   {
                       DataLabels = new PlotOptionsSeriesDataLabels
                       {
                           Enabled = true,
                           Format = "<b>{point.name}</b> ({point.y:,.0f})",
                           Color = Color.FromName("black")
                       }
                   },
               })
               .SetLegend(new Legend { Enabled = false })
               .SetSeries(new Series { Name = "Tipo", Data = dados_carga });

            lit_carga.Text = chart_carga.ToHtmlString();
            #endregion
        }

        protected void btn_iniciar_campanha_Click(object sender, ImageClickEventArgs e)
        {
            if (ddl_campanhas.SelectedItem == null) return;
            BLL.ControleSistema.IncluirEvento(EventoControleSistema.Iniciar_Campanha, new DTO.ControleSistema { Valor = ddl_campanhas.SelectedItem.Value, Campanha = ddl_campanhas.SelectedItem.Value.ToInt64(), Situacao = (int)SitucaoEventoControleSistema.Aguardando, Solicitante = (Session["Usuario"] as DTO.Usuario).Id });
        }

        protected void btn_parar_campanha_Click(object sender, ImageClickEventArgs e)
        {
            if (ddl_campanhas.SelectedItem == null) return;
            BLL.ControleSistema.IncluirEvento(EventoControleSistema.Parar_Campanha, new DTO.ControleSistema { Valor = ddl_campanhas.SelectedItem.Value, Campanha = ddl_campanhas.SelectedItem.Value.ToInt64(), Situacao = (int)SitucaoEventoControleSistema.Aguardando, Solicitante = (Session["Usuario"] as DTO.Usuario).Id });
        }

        protected void btn_continuar_campanha_Click(object sender, ImageClickEventArgs e)
        {
            if (ddl_campanhas.SelectedItem == null) return;
            //BLL.ControleSistema.IncluirEvento(EventoControleSistema.Continuar_Campanha, ddl_campanhas.SelectedItem.Value.ToString(), (Session["Usuario"] as DTO.Usuario).Id);
        }

        protected void btn_atualizar_requisicao_Click(object sender, EventArgs e)
        {
            CarregarSolicitacoes();
        }

        protected void btn_atualizar_historico_Click(object sender, EventArgs e)
        {

        }

        protected void btn_reiniciar_Click(object sender, ImageClickEventArgs e)
        {
            BLL.ControleSistema.IncluirEvento(EventoControleSistema.Recarregar_Campanha, new DTO.ControleSistema { Valor = ddl_campanhas.SelectedItem.Value, Campanha = ddl_campanhas.SelectedItem.Value.ToInt64(), Situacao = (int)SitucaoEventoControleSistema.Aguardando, Solicitante = (Session["Usuario"] as DTO.Usuario).Id });
        }

        protected void imgbtn_carga_Click(object sender, ImageClickEventArgs e)
        {
            //CriarChartCarga(ObterIdsCampanha(Session["CampanhaHierarquica"] as List<DTO.Campanha>));
        }

        protected void btn_filtrar_Click(object sender, ImageClickEventArgs e)
        {
            PreencherCharts();
        }

        protected void dtl_agressividade_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            ChartAgressividade chart = (ChartAgressividade)e.Item.DataItem;
            var pnl = e.Item.FindControl("pnl_agressividade");

            Literal lit_agressividade = new Literal();
            lit_agressividade.ID = chart.NomeDivHTML;
            lit_agressividade.Text = chart.HightchartHTML;
            pnl.Controls.Add(lit_agressividade);
        }
    }

    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 30/10/2013
    /// Utilizada para exibição de multiplos charts de agressividade             
    /// </summary>
    public class ChartAgressividade
    {
        public long IdCampanha { get; set; }

        public string NomeDivHTML { get; set; }

        public string HightchartHTML { get; set; }

    }
}