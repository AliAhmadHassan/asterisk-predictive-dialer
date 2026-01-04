namespace Silver.DTO
{
    public class GrupoMenu
    {
        public GrupoMenu()
        {
        }
        public GrupoMenu(long idGrupo, long idMenu)
        {
            IdGrupo = idGrupo;
            IdMenu = idMenu;
        }
        public long IdGrupo { get; set; }
        public long IdMenu { get; set; }
        public long GrupoIdGrupo { get; set; }
        public string GrupoNome { get; set; }
        public string GrupoDescricao { get; set; }
        public bool GrupoAtivo { get; set; }
        public long MenuIdMenu { get; set; }
        public string MenuDescricao { get; set; }
        public string Url { get; set; }
        public byte[] Icone { get; set; }
        public bool MenuAtivo { get; set; }
    }
}
