using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asterisk.Server
{
    static class Program
    {
        private static Silver.AsteriskClient.ClienteAsterisk cliente_asterisk = new Silver.AsteriskClient.ClienteAsterisk();

        static void Main(string[] args)
        {
            Console.Out.WriteLine("Iniciando servidor");
            cliente_asterisk.IniciarConexao();
            Console.Read();
        }
    }
}
