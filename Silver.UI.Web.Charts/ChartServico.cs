using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Silver.UI.Web.Charts;
using System.Web;
using System.IO;

namespace Silver.UI.Web.Charts
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva   
    ///             Data: 12/08/2013
    /// </summary>
    public class ChartServico
    {
        #region Lista de Cores
        private List<string> cores = new List<string> 
        {
            "#E6E8FA", "#FF0000", "#CDC5BF", "#990000", "#999999", "#660000", "#E9C2A6", "#EBC79E", "#D19275",
            "#333333", "#330000", "#666666", "#CCCCCC", "#CC0000", "#D9D9F3", "#FF6666", "#FFCCCC", "#8E2323", 
            "#A62A2A", "#D19275", "#EBC79E", "#E9C2A6", "#A67A64", "#97694F", "#855E42", "#6B4226", "#5C3317", 
            "#5C4033", "#CCFFFF", "#66FFFF", "#33CCFF", "#3366FF", "#3333FF", "#000099", "#000066", "#330033", 
            "#663366", "#993366", "#CC33CC", "#CC66CC", "#FF99FF", "#FFCCFF", "#99FF99", "#66FF99", "#33FF33", 
            "#00CC00", "#009900", "#006600", "#003300", "#663300", "#993300", "#CC6600", "#FF6600", "#FF9900", 
            "#FFCC33", "#FFCC99", "#FFFFED", "#FFFFCC", "#FFFFAA", "#FFFF90", "#FFFF80", "#FFFF66", "#FFFF33", 
            "#5C4033", "#5C3317", "#6B4226", "#855E42", "#97694F", "#A67A64", "#A62A2A", "#8E2323", "#8E6B23" 
        };
        #endregion

        #region Cores do Mapa
        private List<string> coresMapa = new List<string> 
        {
            "FFFB1919","FFE73838","FFDF5050","FFD75D5D","FFD37272","FFD38F72","FFD39D72","FFD3BA72","FFD3C872","FF275C3A",
            "FFC0D372","FFACD372","FF98D372","FF7FD372","FF72D37D","FF72CB7D","FF72BF7D","FF488D60","FF35734B","FF1A482A"
        };
        #endregion

        public Grafico ObjGrafico(string Div, DataTable dados, List<ConfiguracaoSerie> listSerie, string EixoX_categories = null, bool multiSerie = false, string Zoom = "x",
            bool data = false, int EixoX_rotation = 0, int distanciaY = 20, bool legenda = false, bool legenda_enabled = false, string legenda_align = null, string legenda_layout = null,
            string legenda_verticalAlign = null, bool legenda_floating = false)
        {
            Grafico grafico = new Grafico();
            Serie Serie = new Serie();
            EixoY EixoY = new EixoY();
            EixoX EixoX = new EixoX();
            List<string> categorias = new List<string>();

            grafico.Div = Div;
            grafico.Zoom = Zoom;

            if (legenda)
            {
                grafico.Legenda.Habilitado = legenda_enabled;
                grafico.Legenda.Layout = legenda_layout;
                grafico.Legenda.Alinhamento = legenda_align;
                grafico.Legenda.AlinhamentoVertical = legenda_verticalAlign;
                grafico.Legenda.Flutuante = legenda_floating;
            }

            if (!data)
            {
                if (!string.IsNullOrEmpty(EixoX_categories))
                    for (int i = 0; i < dados.Rows.Count; i++)
                        categorias.Add(dados.Rows[i][EixoX_categories].ToString());
            }
            else
            {
                if (!string.IsNullOrEmpty(EixoX_categories))
                    for (int i = 0; i < dados.Rows.Count; i++)
                        categorias.Add(string.Format("{0:d/MMM}", dados.Rows[i][EixoX_categories]));
            }

            EixoX.Categorias = categorias;
            EixoX.Rotulo.Rotacao = EixoX_rotation;
            EixoX.Rotulo.DistanciaY = distanciaY;

            grafico.EixoX.Add(EixoX);

            bool oposto = false;

            for (int j = 0; j < listSerie.Count; j++)
            {
                Serie = new Serie();
                if (multiSerie)
                {
                    EixoY = new EixoY();
                    EixoY.Titulo = listSerie[j].Nome;
                    EixoY.Oposto = oposto;

                    grafico.EixoY.Add(EixoY);
                    oposto = oposto ? false : true;
                    Serie.EixoY = j;
                }

                Serie.Nome = listSerie[j].Nome;
                Serie.Tipo = listSerie[j].Tipo;
                Serie.Rotulo = listSerie[j].Rotulo;
                Serie.Empilhamento = listSerie[j].Empilhamento;
                Serie.Cor = listSerie[j].Cor == "auto" ? cores[j] : listSerie[j].Cor;

                if (listSerie[j].Tipo == TipoSerie.Pizza)
                {
                    for (int i = 0; i < dados.Rows.Count; i++)
                        Serie.Dados.Add("['" + dados.Rows[i][EixoX_categories].ToString() + "'," + verificaNumero(dados.Rows[i][listSerie[j].Campo].ToString()).ToString().Replace(',', '.') + "]");
                }
                else
                {
                    for (int i = 0; i < dados.Rows.Count; i++)
                        Serie.Dados.Add(verificaNumero(dados.Rows[i][listSerie[j].Campo].ToString()).ToString().Replace(',', '.'));
                }

                grafico.Series.Add(Serie);
            }

            return grafico;
        }

        private double verificaNumero(string valor)
        {
            try
            {
                if (valor.Split(' ').Length > 1)
                {
                    return Convert.ToDouble(valor.Split(' ')[1]).ToString().Split(',').Length > 1 ? Convert.ToDouble(valor.Split(' ')[1]).ToString().Split(',')[1].Length > 3 ?
                        Math.Round(Convert.ToDouble(valor.Split(' ')[1]), 3) : Convert.ToDouble(valor.Split(' ')[1]) : Convert.ToDouble(valor.Split(' ')[1]);
                }
                else
                {
                    return Convert.ToDouble(valor).ToString().Split(',').Length > 1 ? Convert.ToDouble(valor).ToString().Split(',')[1].Length > 3 ?
                        Math.Round(Convert.ToDouble(valor), 3) : Convert.ToDouble(valor) : Convert.ToDouble(valor);
                }
            }
            catch
            {
                return 0;
            }
        }

    }
}
