namespace Silver.DTO
{
    public partial class CargaTelefoneTipo : Base
    {
        public CargaTelefoneTipo()
        {
            Id = 0;
        }

        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPUcargatelefonetipo"
        , ProcedureInserir = "SPIcargatelefonetipo"
        , ProcedureRemover = "SPDcargatelefonetipo"
        , ProcedureListarTodos = "SPScargatelefonetipo"
        , ProcedureSelecionar = "SPScargatelefonetipoPelaPK")]
        public long Id { get; set; }


        public string Descricao { get; set; }
        public bool Ativo { get; set; }
    }
}
