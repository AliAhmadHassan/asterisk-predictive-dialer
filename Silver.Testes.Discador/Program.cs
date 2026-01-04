using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Silver.AsteriskClient;
using System.Configuration;
using Silver.Common;
using Silver.DTO;

namespace Silver.Testes.Discador
{
    class Program
    {
        private static Silver.AsteriskClient.ClienteAsterisk cliente_asterisk = new AsteriskClient.ClienteAsterisk();
        private static BLL.ProcessoCampanha processoCampanha = new BLL.ProcessoCampanha(new BLL.Campanha().Obter(ConfigurationManager.AppSettings["application.Campanha.Id"].ToInt64()));
        private static List<string> LDDD11 = new List<string>();


        #region Protocolos
        protected static SortedList<Protocolo, string> OperadoraDDD =
           new SortedList<Protocolo, string>() {
            { Protocolo.Algar, "015" },
            { Protocolo.GVT, "015" },
            { Protocolo.Mahatel, "015" },
            { Protocolo.Nexus, "015" },
            { Protocolo.Pontal, "015" },
            { Protocolo.Telefonica, "015" },
            { Protocolo.Transit, "015" }};
        #endregion

        static void Main(string[] args)
        {
        }
    }
}
