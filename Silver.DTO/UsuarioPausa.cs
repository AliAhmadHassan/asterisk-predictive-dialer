namespace Silver.DTO
{
    public partial class UsuarioPausa : Base
    {
        public UsuarioPausa()
        {
            Id = 0;
        }

        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPUusuariopausa"
        , ProcedureInserir = "SPIusuariopausa"
        , ProcedureRemover = "SPDusuariopausa"
        , ProcedureListarTodos = "SPSusuariopausa"
        , ProcedureSelecionar = "SPSusuariopausaPelaPK")]
        public long Id { get; set; }


        public long IdUsuario { get; set; }
        public long IdPausa { get; set; }
        public System.DateTime Inicio { get; set; }
        public System.DateTime Fim { get; set; }
        public long TempoSegundos { get; set; }
    }
}
