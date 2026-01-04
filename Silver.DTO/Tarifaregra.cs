namespace Silver.DTO
{
    public partial class TarifaRegra : Base
    {
        public TarifaRegra()
        {
            Id = 0;
        }

        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPUtarifaregra"
        , ProcedureInserir = "SPItarifaregra"
        , ProcedureRemover = "SPDtarifaregra"
        , ProcedureListarTodos = "SPStarifaregra"
        , ProcedureSelecionar = "SPStarifaregraPelaPK")]
        public long Id { get; set; }

        public string Descricao { get; set; }
        public bool Ativo { get; set; }
    }
}
