namespace Silver.DTO
{
    public partial class LogExcluir
    {
        public long Id { get; set; }
        public long IdUsuario { get; set; }
        public System.DateTime Data { get; set; }
        public string Tabela { get; set; }
        public string Campo { get; set; }
        public string Chave { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
