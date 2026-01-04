using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Silver.UI.Web.Charts
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:Chart runat=server></{0}:Chart>")]
    public class Chart : WebControl
    {
        public Grafico Grafico { get; set; }
        public string Largura { get; set; }
        public string Altura { get; set; }
        public Literal Literal { get; set; }
        public TipoSerie Tipo { get; set; }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        public override void DataBind()
        {
            StringBuilder strScript = new StringBuilder();

            strScript.Append("$(function () { ");
            strScript.Append("$('#" + Grafico.Div + "').highcharts({ ");
            strScript.Append("chart: {  ");
            strScript.Append(string.IsNullOrEmpty(Grafico.Zoom) ? "" : "zoomType: '" + Grafico.Zoom + "' ");
            strScript.Append("}, ");

            strScript.Append("title: { ");
            strScript.Append("text: '' ");
            strScript.Append("}, ");

            //Labels
            var labels = (from series in Grafico.Series
                          group series by new { series.Tipo } into tb
                          select new
                          {
                              tb.Key.Tipo,
                              empilhamento = tb.Max(a => (a.Empilhamento)),
                              labels = tb.Max(a => (a.Rotulo))
                          }).ToArray();
            if (labels.Length > 0)
            {
                strScript.Append("plotOptions: { ");
                for (int i = 0; i < labels.Length; i++)
                {
                    strScript.Append(labels[i].Tipo + ": { ");
                    strScript.Append(labels[i].empilhamento ? "stacking: 'normal', " : "");
                    strScript.Append("dataLabels: { ");
                    strScript.Append(labels[i].labels ? "enabled: true " : "");
                    strScript.Append("} ");
                    strScript.Append(i < labels.Length - 1 ? "}, " : "} ");
                }
                strScript.Append("}, ");
            }
            //Fim Labels

            //xAxis - Eixo X
            if (Grafico.EixoX.Count > 0)
            {
                strScript.Append("xAxis: [ ");

                for (int i = 0; i < Grafico.EixoX.Count; i++)
                {
                    strScript.Append("{");

                    if (Grafico.EixoX[i].Categorias != null && Grafico.EixoX[i].Categorias.Count > 0)
                    {
                        strScript.Append("categories: [ ");
                        for (int j = 0; j < Grafico.EixoX[i].Categorias.Count; j++)
                        {
                            strScript.Append("'" + Grafico.EixoX[i].Categorias[j] + "'");
                            strScript.Append(j < Grafico.EixoX[i].Categorias.Count - 1 ? "," : "");
                        }
                        strScript.Append("], ");
                        strScript.Append("labels: { ");
                        strScript.Append("rotation: " + Grafico.EixoX[i].Rotulo.Rotacao + ", y: " + Grafico.EixoX[i].Rotulo.DistanciaY + " ");
                        strScript.Append("} ");
                    }
                    else
                    {
                        strScript.Append("min: " + Grafico.EixoX[i].Minino);
                        strScript.Append(",title: { text: '" + Grafico.EixoX[i].Titulo + "'");
                        strScript.Append("}, ");
                        strScript.Append("opposite: " + Grafico.EixoX[i].Oposto.ToString() + ",");
                        strScript.Append("} ");
                    }
                    strScript.Append(i < Grafico.EixoX.Count - 1 ? "}," : "}");
                }

                strScript.Append("], ");
            }
            //fim Eixo X

            //YAxis - Eixo Y
            if (Grafico.EixoY.Count > 0)
            {
                strScript.Append("yAxis: [ ");

                for (int i = 0; i < Grafico.EixoY.Count; i++)
                {
                    strScript.Append("{");

                    if (Grafico.EixoY[i].Categorias != null && Grafico.EixoY[i].Categorias.Count > 0)
                    {
                        strScript.Append("categories: [ ");
                        for (int j = 0; j < Grafico.EixoY[i].Categorias.Count; j++)
                        {
                            strScript.Append("'" + Grafico.EixoY[i].Categorias[j] + "'");
                            strScript.Append(j < Grafico.EixoY[i].Categorias.Count ? "," : "");
                        }
                        strScript.Append("], ");
                        strScript.Append("labels: { ");
                        strScript.Append(Grafico.EixoY[i].Rotulo.Rotacao == 0 ? "" : "rotation: " + Grafico.EixoY[i].Rotulo.Rotacao);
                        strScript.Append("} ");
                    }
                    else
                    {
                        strScript.Append("min: " + Grafico.EixoY[i].Minino);
                        strScript.Append(",title: { text: '" + Grafico.EixoY[i].Titulo + "'");
                        strScript.Append("}, ");
                        strScript.Append("opposite: " + Grafico.EixoY[i].Oposto.ToString().ToLower() + "");
                    }

                    strScript.Append(i < Grafico.EixoY.Count - 1 ? "}," : "}");
                }

                strScript.Append("], ");
            }
            //fim Eixo Y

            //Legend - Legenda
            if (Grafico.Legenda != null)
            {
                strScript.Append("legend: { ");
                strScript.Append("enabled: " + Grafico.Legenda.Habilitado.ToString() + ",");
                strScript.Append(string.IsNullOrEmpty(Grafico.Legenda.Alinhamento) ? "" : "layout: '" + Grafico.Legenda.Layout + "',");
                strScript.Append(string.IsNullOrEmpty(Grafico.Legenda.Alinhamento) ? "" : "align: '" + Grafico.Legenda.Alinhamento + "',");
                strScript.Append(string.IsNullOrEmpty(Grafico.Legenda.AlinhamentoVertical) ? "" : "verticalAlign: '" + Grafico.Legenda.AlinhamentoVertical + "',");
                strScript.Append("floating: " + Grafico.Legenda.Flutuante.ToString());
                strScript.Append("}, ");
            }
            //Fim Lengeda

            //ToolTip
            strScript.Append("tooltip: { ");
            strScript.Append("shared: true ");
            strScript.Append("}, ");
            //Fim tooltip

            //Series
            strScript.Append("series: [ ");

            for (int i = 0; i < Grafico.Series.Count; i++)
            {
                strScript.Append("{ ");
                strScript.Append("name: '" + Grafico.Series[i].Nome + "', ");
                strScript.Append("type: '" + ObterTipo(Grafico.Series[i].Tipo) + "', ");
                strScript.Append(Grafico.Series[i].EixoY < 0 ? "" : "yAxis: " + Grafico.Series[i].EixoY + ", ");
                strScript.Append(string.IsNullOrEmpty(Grafico.Series[i].Cor) ? "" : "color: '" + Grafico.Series[i].Cor + "', ");
                strScript.Append("data: [ ");
                for (int j = 0; j < Grafico.Series[i].Dados.Count; j++)
                {
                    strScript.Append(Grafico.Series[i].Dados[j]);
                    strScript.Append(j < Grafico.Series[i].Dados.Count - 1 ? "," : "");
                }
                strScript.Append("] ");
                strScript.Append(i < Grafico.Series.Count - 1 ? "}," : "}");
            }
            strScript.Append("] ");
            //Fim Series

            strScript.Append("}) ");
            strScript.Append("}); ");

            Text = "<div id ='" + Grafico.Div + "' style='width: " + (string.IsNullOrEmpty(Largura) ? "auto" : Largura) + "; height: " +
                (string.IsNullOrEmpty(Altura) ? "auto" : Altura) + "; margin: 0 auto; overflow: hidden'></div><script type='text/javascript'>" + strScript.ToString() + "</script>";
        }

        private string ObterTipo(TipoSerie tipo)
        {
            /* column, line, bar, area, pie*/
            switch (tipo)
            {
                case TipoSerie.Coluna:
                    return "column";
                case TipoSerie.Linha:
                    return "line";
                case TipoSerie.Barra:
                    return "bar";
                case TipoSerie.Area:
                    return "area";
                case TipoSerie.Pizza:
                    return "pie";
            }

            return null;
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            output.Write(Text);
        }
    }
}
