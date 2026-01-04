namespace Silver.DTO
{
    public partial class Menu : Base
    {
        public Menu()
        {
            Id = 0;
        }

        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPUmenu"
        , ProcedureInserir = "SPImenu"
        , ProcedureRemover = "SPDmenu"
        , ProcedureListarTodos = "SPSmenu"
        , ProcedureSelecionar = "SPSmenuPelaPK")]
        public long Id { get; set; }

        public long IdMenu { get; set; }
        public string Descricao { get; set; }
        public string Url { get; set; }
        public byte[] Icone { get; set; }
        public bool Ativo { get; set; }
        public int GrupoMenu { get; set; }
    }
}
