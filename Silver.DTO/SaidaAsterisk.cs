using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.DTO
{
    public class SaidaAsterisk:Base
    {
        [AtributoBind(ChavePrimaria = true
        , ProcedureInserir = "SPIsaidaasterisk"
        , ProcedureListarTodos = "SPSsaidaasterisk"
        , ProcedureSelecionar = "SPSsaidaasteriskPelaPK")]
        public long Id { get; set; }

        public string Valor { get; set; }
    }
}
