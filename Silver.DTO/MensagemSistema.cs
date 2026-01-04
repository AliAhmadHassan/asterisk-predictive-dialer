using System;

namespace Silver.DTO
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 03/07/2014
    /// </summary>
    [Serializable()]
    public class MensagemSistema : Base
    {
        #region Atributos
        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = ""
        , ProcedureInserir = "SPIMensagemSistema"
        , ProcedureRemover = ""
        , ProcedureListarTodos = ""
        , ProcedureSelecionar = "")]
        #endregion
        public Int64 Id { get; set; }

        public DateTime DataHora { get; set; }

        public string Mensagem { get; set; }

        public bool Visualizada { get; set; }

        public long IdCampanha { get; set; }

        public DateTime  DataHoraVisualizacao { get; set; }

        public int TipoMensagem { get; set; }
    }
}
