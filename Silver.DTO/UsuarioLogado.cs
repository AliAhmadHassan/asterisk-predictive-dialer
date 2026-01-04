using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.DTO
{
    public class UsuarioLogado
    {
         [Silver.DTO.Base.AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPUusuarioLogado"
        , ProcedureInserir = "SPIusuarioLogado"
        , ProcedureRemover = "SPDusuarioLogado"
        , ProcedureListarTodos = "SPSusuarioLogado"
        , ProcedureSelecionar = "SPSusuarioLogadoPeloRamal")]
        public long Ramal { get; set; }

        public string Url { get; set; }

        public string Contato { get; set; }
    }
}
