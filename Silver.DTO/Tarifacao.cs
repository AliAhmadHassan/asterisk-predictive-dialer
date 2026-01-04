namespace Silver.DTO
{
    public partial class Tarifacao : Base
    {
        public Tarifacao()
        {
            Id = 0;
        }

        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPUtarifacao"
        , ProcedureInserir = "SPItarifacao"
        , ProcedureRemover = "SPDtarifacao"
        , ProcedureListarTodos = "SPStarifacao"
        , ProcedureSelecionar = "SPStarifacaoPelaPK")]
        public long Id { get; set; }


        public long IdBilhetagem { get; set; }
        public long IdTarifa { get; set; }
        public long IdTarifaAlternativa { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorAlternativo { get; set; }
        public bool Ativo { get; set; }
    }
}
