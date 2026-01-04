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
    public class Rotulo
    {
        public int Rotacao { get; set; }
        
        public int DistanciaX { get; set; }

        public int DistanciaY { get; set; }
    }
}
