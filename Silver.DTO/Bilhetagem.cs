namespace Silver.DTO
{
    public partial class Bilhetagem : Base
    {
        public Bilhetagem()
        {
            Id = 0;
        }

        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPUbilhetagem"
        , ProcedureInserir = "SPIbilhetagem"
        , ProcedureRemover = "SPDbilhetagem"
        , ProcedureListarTodos = "SPSbilhetagem"
        , ProcedureSelecionar = "SPSbilhetagemPelaPK")]
        public long Id { get; set; }

        public System.DateTime calldate { get; set; }
        public string clid { get; set; }
        public string src { get; set; }
        public string dst { get; set; }
        public string dcontext { get; set; }
        public string channel { get; set; }
        public string dstchannel { get; set; }
        public string lastapp { get; set; }
        public string lastdata { get; set; }
        public long duration { get; set; }
        public long billsec { get; set; }
        public string disposition { get; set; }
        public long amaflags { get; set; }
        public string accountcode { get; set; }
        public string userfield { get; set; }
        public string uniqueid { get; set; }
        public string linkedid { get; set; }
        public string sequence { get; set; }
        public string peeraccount { get; set; }
        public bool amd { get; set; }
        public long amdtype { get; set; }
        public long otiosetime { get; set; }
        public bool callanalyzed { get; set; }
        public string recordingpath { get; set; }
        public long idcampanha { get; set; }
        public string pathgravacao { get; set; }
    }
}
