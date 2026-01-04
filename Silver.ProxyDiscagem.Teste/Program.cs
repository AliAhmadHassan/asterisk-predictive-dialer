using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.ProxyDiscagem.Teste
{
    class Program
    {
        static void Main(string[] args)
        {
            ProxyDiscagem a = new ProxyDiscagem();

            a.IniciarServico();

            Console.ReadLine();
        }
    }
}
