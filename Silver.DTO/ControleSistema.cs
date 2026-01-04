using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.DTO
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 26/06/2013    
    /// </summary>
    public class ControleSistema
    {
        public long Id { get; set; }

        public string Evento { get; set; }

        public string Valor { get; set; }

        public long Situacao { get; set; }

        public DateTime DtHrExecucao { get; set; }

        public long Solicitante { get; set; }

        public long Campanha { get; set; }

        public decimal Porcentagem { get; set; }
    }
}
