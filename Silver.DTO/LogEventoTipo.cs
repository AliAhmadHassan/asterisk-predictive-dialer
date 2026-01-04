using System.Collections.Generic;

namespace Silver.DTO
{
    public partial class LogEventoTipo
    {
        public LogEventoTipo()
        {
            this.LogEventos = new HashSet<LogEvento>();
        }
        public long Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public virtual ICollection<LogEvento> LogEventos { get; set; }
    }
}
