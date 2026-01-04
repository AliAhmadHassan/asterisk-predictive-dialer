using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.DTO
{
    public class BilhetagemGravacaoGrid : BilhetagemGravacao
    {
        public long id { get; set; }

        public DateTime calldate { get; set; }

        public DateTime end { get; set; }

        public string clid { get; set; }

        public string channel { get; set; }

        public string dstchannel { get; set; }

        public long billsec { get; set; }
        
        public string src { get; set; }

        public string dst { get; set; }

        public long duration { get; set; }

        public long idcampanha { get; set; }

        public string pathgravacao { get; set; }
    }
}
