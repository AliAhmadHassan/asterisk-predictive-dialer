using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.AsteriskClient
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 23/10/2013
    /// </summary>
    public class AsteriskClientManager : AsteriskBase
    {
        public AsteriskCommand ComandosAsterisk { get; set; }

        public AsteriskListener EscutaAsterisk { get; set; }

        public AsteriskClientManager():base()
        {
            ComandosAsterisk = new AsteriskCommand();
            EscutaAsterisk = new AsteriskListener();
        }

        public AsteriskClientManager(string usuario_asterisk, string senha_asterisk, string ip_asterisk, int porta_asterisk)
            : base(usuario_asterisk, senha_asterisk, ip_asterisk, porta_asterisk)
        {
            ComandosAsterisk = new AsteriskCommand();
            EscutaAsterisk = new AsteriskListener();
        }

        public override void Dispose()
        {
            ComandosAsterisk.Dispose();
            EscutaAsterisk.Dispose();
            base.Dispose();
        }
    }
}
