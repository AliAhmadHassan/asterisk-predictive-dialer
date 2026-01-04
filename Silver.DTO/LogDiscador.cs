using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.DTO
{
    public class LogDiscador
    {
        public int Id { get; set; }

        public int IdCampanha { get; set; }

        public int Evento { get; set; }
        
        public DateTime DataHora { get; set; }
    }
}
