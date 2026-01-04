using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.BLL
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 21/06/2014
    /// </summary>
    public class Idle
    {
        public void Inserir(DTO.Idle idle)
        {
            new DAL.Idle().Cadastro(idle);
        }

        public List<DTO.Idle> Listar()
        {
            return new DAL.Idle().Select();
        }

        public List<DTO.Idle> ListarPeloRamal(long ramal)
        {
            return new DAL.Idle().SelectPeloRamal(ramal);
        }

        public decimal ObterIdle(params long[] id_historico)
        {
            return new DAL.Idle().ObterIdle(id_historico);
        }
    }
}
