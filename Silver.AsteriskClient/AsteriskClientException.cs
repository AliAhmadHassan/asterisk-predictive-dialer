using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.AsteriskClient
{
    public class AsteriskClientException : Exception
    {
        public AsteriskClientException(string mensagem)
            : base(mensagem)
        {

        }
    }
}
