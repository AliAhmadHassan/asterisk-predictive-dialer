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
    public class Legenda
    {
        public string Alinhamento { get; set; }

        public string Layout { get; set; }

        public string AlinhamentoVertical { get; set; }

        public bool Flutuante { get; set; }

        public bool Habilitado { get; set; }
    }
}
