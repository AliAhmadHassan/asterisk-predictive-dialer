using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.DTO
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 21/06/2014
    /// </summary>
    public class Idle : Base
    {
        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = ""
        , ProcedureInserir = "SPIIdle"
        , ProcedureRemover = ""
        , ProcedureListarTodos = "SPSIdle"
        , ProcedureSelecionar = "SPSe1PeloRamal")]
        public long Id { get; set; }

        public long Ramal { get; set; }

        public DateTime Desligou { get; set; }

        public DateTime Atendeu { get; set; }

        public long IdHistorico { get; set; }
    }
}
