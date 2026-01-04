using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.ProxyDiscagem.Server.DTO
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 22/08/2014
    /// </summary>
    [Serializable()]
    public class CampanhaDiscagem
    {
        public long IdCampanha { get; set; }

        private Queue<Silver.DTO.SolicitacaoDiscagem> solicitacoes_discagem = new Queue<Silver.DTO.SolicitacaoDiscagem>();
        public Queue<Silver.DTO.SolicitacaoDiscagem> SolicitacoesCampanha
        {
            get { return solicitacoes_discagem; }
            set { solicitacoes_discagem = value; }
        }
    }
}
