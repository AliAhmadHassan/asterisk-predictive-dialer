namespace Silver.DTO
{
    public partial class E1 : Base
    {
        public E1()
        {
            Id = 0;
        }

        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPUe1"
        , ProcedureInserir = "SPIe1"
        , ProcedureRemover = "SPDe1"
        , ProcedureListarTodos = "SPSe1"
        , ProcedureSelecionar = "SPSe1PelaPK")]
        public long Id { get; set; }
        public string Descricao { get; set; }
        public long IdServidor { get; set; }
        public long IdOperadora { get; set; }
        public string Placa { get; set; }
        public string Posicao { get; set; }
        public string Contrato { get; set; }
        public bool Ativo { get; set; }
    }
}
