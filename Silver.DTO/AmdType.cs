namespace Silver.DTO
{
    public class AmdType : Base
    {
        public AmdType()
        {
            Id = 0;
        }

        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPUamdtype"
        , ProcedureInserir = "SPIamdtype"
        , ProcedureRemover = "SPDamdtype"
        , ProcedureListarTodos = "SPSamdtype"
        , ProcedureSelecionar = "SPSamdtypePelaPK")]
        public long Id { get; set; }

        public string Descricao { get; set; }
    }
}
