using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.BLL
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 11/10/2013
    /// </summary>
    public class LogRamal
    {
        public void Cadastrar(DTO.LogRamal log_ramal)
        {
            new DAL.LogRamal().Cadastro(log_ramal);
        }

        public List<DTO.LogRamal> Obter(long id_log)
        {
            return new DAL.LogRamal().ObterPeloPonteiro(id_log);
        }

        public List<DTO.LogRamal> Obter(DateTime inicio, DateTime fim)
        {
            return new DAL.LogRamal().ObterPeloPeriodo(inicio, fim);
        }

        public List<DTO.LogRamal> Obter(DateTime inicio, DateTime fim, long ramal)
        {
            return new DAL.LogRamal().ObterPeloPeriodoRamal(inicio, fim, ramal);
        }
    }
}
