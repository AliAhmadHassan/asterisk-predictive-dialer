using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.DTO
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 20/08/2014
    /// </summary>
    public class MensagemSistemaVisualizado : Base
    {
        [AtributoBind(ChavePrimaria = true
          , ProcedureAlterar = ""
          , ProcedureInserir = "SPIMsgSisVisualizacao"
          , ProcedureRemover = ""
          , ProcedureListarTodos = ""
          , ProcedureSelecionar = "")]
        public long Id { get; set; }

        public long IdMensagem { get; set; }

        public long  IdUsuario{ get; set; }

        public DateTime DataHora { get; set; }

    }
}
