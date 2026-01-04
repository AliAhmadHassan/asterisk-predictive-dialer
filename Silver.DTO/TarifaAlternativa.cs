namespace Silver.DTO
{
    public partial class TarifaAlternativa : Base
    {
        public TarifaAlternativa()
        {
            Id = 0;
        }

        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPUtarifaalternativa"
        , ProcedureInserir = "SPItarifaalternativa"
        , ProcedureRemover = "SPDtarifaalternativa"
        , ProcedureListarTodos = "SPStarifaalternativa"
        , ProcedureSelecionar = "SPStarifaalternativaPelaPK")]
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
