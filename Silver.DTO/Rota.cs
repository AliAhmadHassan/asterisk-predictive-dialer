namespace Silver.DTO
{
    public partial class Rota : Base
    {
        public Rota()
        {
            Id = 0;
        }

        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPUrota"
        , ProcedureInserir = "SPIrota"
        , ProcedureRemover = "SPDrota"
        , ProcedureListarTodos = "SPSrota"
        , ProcedureSelecionar = "SPSrotaPelaPK")]
        public long Id { get; set; }


        public string Descricao { get; set; }
        public long IdOperadora { get; set; }
        public long IdTarifaTipo { get; set; }
        public long Prioridade { get; set; }
        public bool Ativo { get; set; }
    }
}
