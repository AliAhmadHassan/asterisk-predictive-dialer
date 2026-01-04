using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.DTO
{
    public class ResumoCampanha
    {
        public long Id { get; set; }

        public string Nome { get; set; }

        public int Operadores { get; set; }

        public int Clientes { get; set; }

        public int Telefones { get; set; }

        public int ClientesDiscados { get; set; }

        public int TelefonesDiscados { get; set; }

        public DTO.StatusProcessoDiscador Status { get; set; }

    }
}
