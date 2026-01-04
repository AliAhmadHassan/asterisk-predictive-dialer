using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Silver.AsteriskClient;
using System.Collections;

namespace Silver.BLL
{
    public class RoboDiscagemGerenciamento
    {
        public event OutputLogs OnOutputLogs;

        private ClienteAsterisk clienteAsterisk;

        public class CacheProcessamento
        {
            public Thread ThreadCampanha { get; set; }

            public DTO.Campanha Campanha { get; set; }
        }

        public List<CacheProcessamento> Processos { get; set; }

        public void PausarCampanha(string nome_campanha, TimeSpan tempo)
        {
            Monitor.Pulse(Processos.Where(c => c.Campanha.Nome.Equals(nome_campanha)));
        }

        public void CancelarProcesso(string campanha)
        {

        }

        public void Main()
        {
            //TODO - Iniciar instância do Robô discagem

            //TODO - Iniciar Componentes de escuta

            //TODO - Iniciar a Escuta Asterisk

            //TODO - Iniciar Campanhas

            Thread t1 = new Thread(new ThreadStart(new BLL.RoboDiscagem().Processar));
            t1.Name = "Nome Campanha";
            t1.Start();

            CacheProcessamento cp = new CacheProcessamento() { Campanha = new DTO.Campanha() { }, ThreadCampanha = t1 };
            Processos.Add(cp);
        }

        private void IniciarCampanha()
        {

        }

    }
}

