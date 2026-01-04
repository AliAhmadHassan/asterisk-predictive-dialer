namespace Silver.DTO
{
    public partial class Usuario : Base
    {
        public Usuario()
        {
            Id = 0;
        }

        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPUUsuario"
        , ProcedureInserir = "SPIUsuario"
        , ProcedureRemover = "SPDUsuario"
        , ProcedureListarTodos = "SPSUsuario"
        , ProcedureSelecionar = "SPSUsuarioPelaPK")]
        public long Id { get; set; }


        public long Ramal { get; set; }
        public string Nome { get; set; }
        public long IdGrupo { get; set; }
        public long IdCampanha { get; set; }
        public string Senha { get; set; }
        public System.DateTime SenhaExpiracao { get; set; }
        public string UltimaSenha { get; set; }
        public string PenultimaSenha { get; set; }
        public bool Operador { get; set; }
        public bool Ativo { get; set; }

        /// <summary>
        /// Este propriedade não tem referência com nenhum campo da tabela Usuario, será utilizado para poder adicionar e remover o usuario em questão de sua respectiva fila no asterisk. Foi adicionado ao objeto com o intuito de centralizar as informações.
        /// </summary>
        public string CampanhaDescricao { get; set; }
    }

    public class UsuarioRobo : Usuario
    {
        public DTO.EstadoUsuario Status { get; set; }

        public string Telefone { get; set; }
    }
}
