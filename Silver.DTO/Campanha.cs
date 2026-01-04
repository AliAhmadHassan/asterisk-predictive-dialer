namespace Silver.DTO
{
    public partial class Campanha : Base
    {
        public Campanha()
        {
            Id = 0;
        }

        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPUcampanha"
        , ProcedureInserir = "SPIcampanha"
        , ProcedureRemover = "SPDcampanha"
        , ProcedureListarTodos = "SPScampanha"
        , ProcedureSelecionar = "SPScampanhaPelaPK")]
        public long Id { get; set; }

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string MusicaEspera { get; set; }
        public int Estrategia { get; set; }
        public long QtdToques { get; set; }
        public long TempoEspera { get; set; }
        public string Anuncio { get; set; }
        public long Ativo { get; set; }
        public long Agressividade { get; set; }
        public long IdGrupo { get; set; }
    }
}
