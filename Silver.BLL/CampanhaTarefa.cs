using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.BLL
{
    /// <summary>
    /// Desenvolvido por: Francisco Silva
    ///             Data: 27/06/2013
    /// </summary>
    public class CampanhaTarefa
    {
        public void Incluir(DTO.CampanhaTarefa campanha_tarefa)
        {
            Remover(campanha_tarefa.IdCampanha);
            new DAL.CampanhaTarefa().Incluir(campanha_tarefa);
        }

        public DTO.CampanhaTarefa Obter(long id_campanha)
        {
            return new DAL.CampanhaTarefa().Obter(id_campanha);
        }

        private void Remover(long id_campanha)
        {
            new DAL.CampanhaTarefa().Remover(id_campanha);
        }
    }
}
