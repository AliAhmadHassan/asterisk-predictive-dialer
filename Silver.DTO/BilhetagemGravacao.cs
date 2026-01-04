using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.DTO
{
    [AtributoBind(ChavePrimaria = true
        , ProcedureInserir = "SPIgravacao"
        , ProcedureListarTodos = "SPSgravacao")]
    public class BilhetagemGravacao : Base
    {
        /// <summary>
        /// Id da Bilhetagem
        /// </summary>
        public Int64 Id { get; set; }

        public byte[] Gravacao { get; set; }
    }
}
