using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.UI.Web.Charts
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 12/08/2013
    /// </summary>
    public class Grafico
    {
        public Grafico()
        {
            EixoX = new List<EixoX>();
            EixoY = new List<EixoY>();
            Series = new List<Serie>();
        }

        public string Div { get; set; }

        public string Zoom { get; set; }

        public Legenda Legenda { get; set; }

        public List<EixoX> EixoX { get; set; }

        public List<EixoY> EixoY { get; set; }

        public List<Serie> Series { get; set; }
    }
}
