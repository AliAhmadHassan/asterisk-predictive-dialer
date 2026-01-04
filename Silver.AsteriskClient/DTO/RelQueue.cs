using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.AsteriskClient.DTO
{
    public class RelCampanhaQueueStatus
    {
        public string Channel { get; set; }

        public string CallerIDNum { get; set; }

        public int Wait { get; set; }
    }
}
