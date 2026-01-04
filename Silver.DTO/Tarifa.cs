namespace Silver.DTO
{
    public partial class Tarifa : Base
    {
        public Tarifa()
        {
            Id = 0;
        }

        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPUtarifa"
        , ProcedureInserir = "SPItarifa"
        , ProcedureRemover = "SPDtarifa"
        , ProcedureListarTodos = "SPStarifa"
        , ProcedureSelecionar = "SPStarifaPelaPK")]
        public long Id { get; set; }


        public string Descricao { get; set; }
        public long IdTarifaTipo { get; set; }
        public long IdTarifaRegra { get; set; }
        public long IdOperadora { get; set; }
        public long IdCampanha { get; set; }
        public decimal Valor { get; set; }
        public bool Ativo { get; set; }
    }
}
