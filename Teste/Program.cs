using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teste
{
    class Program
    {
        static void Main(string[] args)
        {
            Silver.AsteriskClient.AsteriskCommand cmd = new Silver.AsteriskClient.AsteriskCommand();
            Silver.AsteriskClient.AsteriskListener lst = new Silver.AsteriskClient.AsteriskListener();
            Silver.AsteriskClient.AsteriskClientManager mng = new Silver.AsteriskClient.AsteriskClientManager();

            lst.SaidaPadrao = Silver.AsteriskClient.SaidaPadraoAsterisk.Delegate;
            lst.IniciarEscuta(cmd.stream_asterisk);
            lst.OnRamalAtendeu = OnRamalAtendeu;
            cmd.RamaisStatus();
            
            Console.Read();
        }

        static void OnRamalAtendeu(long ramal, string telefgone)
        { 
        
        }
    }
}
