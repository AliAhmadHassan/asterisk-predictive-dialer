namespace Silver.DTO
{
    public partial class UsuarioLogin : Base
    {
        public UsuarioLogin()
        {
            Id = 0;
        }

        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPUusuariologin"
        , ProcedureInserir = "SPIusuariologin"
        , ProcedureRemover = "SPDusuariologin"
        , ProcedureListarTodos = "SPSusuariologin"
        , ProcedureSelecionar = "SPSusuariologinPelaPK")]
        public long Id { get; set; }
        public long IdUsuario { get; set; }
        public System.DateTime Login { get; set; }
        public System.DateTime Logout { get; set; }
        public System.DateTime Checkin { get; set; }
        public long TempoSegundos { get; set; }
    }
}
