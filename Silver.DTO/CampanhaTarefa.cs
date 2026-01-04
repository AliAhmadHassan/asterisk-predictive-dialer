using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.DTO
{
    /// <summary>
    /// Desenvolvido por: Francisco Silva
    ///             Data: 28/06/2013
    /// </summary>
    public class CampanhaTarefa
    {
        public CampanhaTarefa()
        {
            SegundaInicio = null;
            SegundaFim = null;
            TercaInicio = null; 
            TercaFim = null;
            QuartaInicio = null;
            QuartaFim = null;
            QuintaInicio = null;
            QuintaFim = null;
            SextaInicio = null;
            SextaFim = null;
            SabadoInicio = null;
            SabadoFim = null;
            DomingoInicio = null;
            DomingoFim = null;
        }

        public long IdCampanha { get; set; }

        public bool Ativo { get; set; }

        public DateTime? SegundaInicio { get; set; }

        public DateTime? SegundaFim { get; set; }

        public DateTime? TercaInicio { get; set; }

        public DateTime? TercaFim { get; set; }

        public DateTime? QuartaInicio { get; set; }

        public DateTime? QuartaFim { get; set; }

        public DateTime? QuintaInicio { get; set; }

        public DateTime? QuintaFim { get; set; }

        public DateTime? SextaInicio { get; set; }

        public DateTime? SextaFim { get; set; }

        public DateTime? SabadoInicio { get; set; }

        public DateTime? SabadoFim { get; set; }

        public DateTime? DomingoInicio { get; set; }

        public DateTime? DomingoFim { get; set; }
    }
}
