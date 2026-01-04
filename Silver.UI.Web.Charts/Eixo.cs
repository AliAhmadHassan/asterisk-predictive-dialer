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
    [Serializable()]
    public class Eixo
    {
        public Eixo()
        {
            Categorias = new List<string>();
            Rotulo = new Rotulo();
        }

        public List<string> Categorias { get; set; }

        public string Titulo { get; set; }

        public bool Oposto { get; set; }

        public int Minino { get; set; }

        public Rotulo Rotulo { get; set; }
    }
}
