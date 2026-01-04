namespace Silver.DTO
{
    public partial class Carga : Base
    {
        public Carga()
        {
            Id = 0;
        }

        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPUcarga"
        , ProcedureInserir = "SPIcarga"
        , ProcedureRemover = "SPDcarga"
        , ProcedureListarTodos = "SPScarga"
        , ProcedureSelecionar = "SPScargaPelaPK")]
        public long Id { get; set; }


        public long IdCampanha { get; set; }
        public string Chave1 { get; set; }
        public string Chave2 { get; set; }
        public System.DateTime DtCarga { get; set; }
        public long Status { get; set; }
        public bool Ativo { get; set; }
        public int IdHistorico{ get; set; }
    }
}
