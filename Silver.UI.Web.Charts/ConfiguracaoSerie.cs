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
    public class ConfiguracaoSerie
    {
        public string Nome { get; set; }

        public string Cor { get; set; }

        public TipoSerie Tipo { get; set; }

        public string Campo { get; set; }

        public bool Rotulo { get; set; }

        public bool Empilhamento { get; set; }
    }
}
