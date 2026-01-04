namespace Silver.DTO
{
    public partial class Servidor : Base
    {
        public Servidor()
        {
            Id = 0;
        }

        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPUservidor"
        , ProcedureInserir = "SPIservidor"
        , ProcedureRemover = "SPDservidor"
        , ProcedureListarTodos = "SPSservidor"
        , ProcedureSelecionar = "SPSservidorPelaPK")]
        public long Id { get; set; }


        public string Descricao { get; set; }
        public string Ip { get; set; }
        public string Url { get; set; }
        public bool Ativo { get; set; }
    }
}
