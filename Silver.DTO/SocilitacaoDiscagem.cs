using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Silver.DTO
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 27/06/2014
    /// </summary>
    [Serializable()]
    public class SolicitacaoDiscagem
    {
        public string DDD { get; set; }

        public string Telefone { get; set; }

        public string IdCampanha { get; set; }

        public string Campanha { get; set; }

        public string IdTelefone { get; set; }

        public string TipoTelefone { get; set; }

        public TcpClient Cliente { get; set; }

        public string RotaPadrao { get; set; }

        public string TipoIdentificado { get; set; }

        public List<int> Protocolo { get; set; }

        public override string ToString()
        {
            return string.Format("{0}|{1}|{2}|{3}|{4}|{5}", DDD, Telefone, Campanha, IdTelefone, TipoTelefone, IdCampanha);
        }
    }
}
