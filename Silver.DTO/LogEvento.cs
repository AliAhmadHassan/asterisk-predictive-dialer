namespace Silver.DTO
{
    public partial class LogEvento
    {
        public long Id { get; set; }
        public long IdUsuario { get; set; }
        public long IdTipo { get; set; }
        public System.DateTime Data { get; set; }
        public string Mensagem { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual LogEventoTipo LogEventoTipo { get; set; }
    }
}
