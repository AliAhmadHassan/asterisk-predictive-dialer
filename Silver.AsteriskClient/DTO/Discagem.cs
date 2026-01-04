using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.AsteriskClient.DTO
{
    public class Discagem
    {
        public string Protocolo { get; set; }

        public string Telefone { get; set; }

        public string IdCampanha { get; set; }

        public string Campanha { get; set; }

        public string IdTelefone { get; set; }

        public string TipoTelefone { get; set; }

        public override string ToString()
        {
            return string.Format("Action: originate\nChannel: DGV/g{0}/{1}\nContext: interno\nExten: start\nPriority: 1\nActionId:{5}\nVariable: Campanha={2}\nVariable: TelefoneID={3}\nVariable: TipoTelefone={4}\nCallerid: {1}\nVariable: IdCampanha={5}\nAsync: true\n\n", Protocolo, Telefone, Campanha, IdTelefone, TipoTelefone, IdCampanha); ;
        }
    }
}
