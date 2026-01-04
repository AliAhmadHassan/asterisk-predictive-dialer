namespace Silver.DTO
{
    public partial class LogErro
    {
        public long Id { get; set; }
        public long IdUsuario { get; set; }
        public long IdMenu { get; set; }
        public System.DateTime Data { get; set; }
        public string Mensagem { get; set; }
        public string Trace { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Menu Menu { get; set; }
    }
}
