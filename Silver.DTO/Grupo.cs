namespace Silver.DTO
{
    using System;
    using System.Collections.Generic;

    public partial class Grupo : Base
    {
        public Grupo()
        {
            Id = 0;
        }

        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPUgrupo"
        , ProcedureInserir = "SPIgrupo"
        , ProcedureRemover = "SPDgrupo"
        , ProcedureListarTodos = "SPSgrupo"
        , ProcedureSelecionar = "SPSgrupoPelaPK")]
        public long Id { get; set; }

        public long IdGrupo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
    }
}
