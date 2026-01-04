namespace Silver.DTO
{
    public partial class Pausa : Base
    {
        public Pausa()
        {
            Id = 0;
        }

        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPUpausa"
        , ProcedureInserir = "SPIpausa"
        , ProcedureRemover = "SPDpausa"
        , ProcedureListarTodos = "SPSpausa"
        , ProcedureSelecionar = "SPSpausaPelaPK")]
        public long Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
    }
}
