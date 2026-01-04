namespace Silver.DTO
{
    public partial class TarifaTipo : Base
    {
        public TarifaTipo()
        {
            Id = 0;
        }

        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPUtarifatipo"
        , ProcedureInserir = "SPItarifatipo"
        , ProcedureRemover = "SPDtarifatipo"
        , ProcedureListarTodos = "SPStarifatipo"
        , ProcedureSelecionar = "SPStarifatipoPelaPK")]
        public long Id { get; set; }


        public string Descricao { get; set; }
        public bool Ativo { get; set; }
    }
}
