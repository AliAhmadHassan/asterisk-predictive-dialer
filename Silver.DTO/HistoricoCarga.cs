using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.DTO
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 25/10/2013
    /// </summary>
    public class HistoricoCarga : Base
    {
        public HistoricoCarga()
        {
            Id = 0;
        }

        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPUhistoricocarga"
        , ProcedureInserir = "SPIhistoricocarga"
        , ProcedureRemover = "SPDhistoricocarga"
        , ProcedureListarTodos = "SPShistoricocarga"
        , ProcedureSelecionar = "SPShistoricocargaPelaPK")]
        public long Id { get; set; }

        public int IdOperador { get; set; }

        public int IdCampanha { get; set; }

        public DateTime DataHoraInicio { get; set; }

        public DateTime DataHoraFim { get; set; }

        public string Descricao { get; set; }

        public string NomeArquivo { get; set; }

        public int Tamanho { get; set; }

        public int Status { get; set; }

        public bool Ativo { get; set; }

        public int TotalCliente { get; set; }

        public int TotalTelefone { get; set; }
    }
}
