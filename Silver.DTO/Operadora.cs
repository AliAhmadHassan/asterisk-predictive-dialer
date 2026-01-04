namespace Silver.DTO
{
    public partial class Operadora : Base
    {
        public Operadora()
        {
            Id = 0;
        }

        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPUoperadora"
        , ProcedureInserir = "SPIoperadora"
        , ProcedureRemover = "SPDoperadora"
        , ProcedureListarTodos = "SPSoperadora"
        , ProcedureSelecionar = "SPSoperadoraPelaPK")]
        public long Id { get; set; }


        public string Descricao { get; set; }
        public long ConsumoMinino { get; set; }
        public bool Ativo { get; set; }
    }
}
