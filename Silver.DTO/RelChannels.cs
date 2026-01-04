using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.DTO
{
    public class RelChannels
    {
        public int Chan { get; set; }
        public int Grp { get; set; }
        public string Context { get; set; }
        public string PortId { get; set; }
        public int Rsrvd { get; set; }
        public int Alrmd { get; set; }
        public string Lckd { get; set; }
        public string Extension { get; set; }
        public string CardType { get; set; }
        public string Intrf { get; set; }
    }
}
