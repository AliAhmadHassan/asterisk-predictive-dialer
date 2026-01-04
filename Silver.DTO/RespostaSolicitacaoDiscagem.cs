using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.DTO
{
    /// <summary>
    /// Desnvolvida por: Francisco Silva
    ///            Data: 02/07/2014
    /// </summary>
    [Serializable()]
    public class RespostaSolicitacaoDiscagem
    {
        public string RespostaSolicitacao { get; set; }

        public string MotivoResposta { get; set; }

        public override string ToString()
        {
            return string.Format("{0}|{1}",  RespostaSolicitacao.Trim(), MotivoResposta.Trim());
        }
    }
}
