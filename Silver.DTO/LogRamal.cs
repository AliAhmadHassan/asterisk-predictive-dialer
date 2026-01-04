using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.DTO
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 11/10/2013
    /// </summary>
    public class LogRamal : Base
    {
        public LogRamal()
        {
            Id = 0;
        }

        #region Atributos
        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = ""
        , ProcedureInserir = "SPIlogramal"
        , ProcedureRemover = ""
        , ProcedureListarTodos = ""
        , ProcedureSelecionar = "")]
        #endregion
        public Int64 Id { get; set; }

        public DateTime DataHora { get; set; }

        public Int64 Ramal { get; set; }

        public long Evento { get; set; }

        public int TelId { get; set; }

    }
}
