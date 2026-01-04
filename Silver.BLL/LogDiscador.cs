using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.BLL
{
    public class LogDiscador
    {
        public void Incluir(DTO.LogDiscador log)
        {
            new DAL.LogDiscador().Incluir(log);
        }

        public List<DTO.LogDiscador> Listar(DateTime inicio, DateTime fim, int id_campanha)
        {
            return new DAL.LogDiscador().Listar(inicio, fim, id_campanha);
        }

        public DTO.LogDiscador DiscadorEmExecucao(long id_campanha)
        {
            return new DAL.LogDiscador().DiscadorEmExecucao(id_campanha);
        }
    }
}
