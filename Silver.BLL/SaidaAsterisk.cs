using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.BLL
{
    public class SaidaAsterisk
    {
        public void Cadastrar(string texto)
        {
            new DAL.SaidaAsterisk().Cadastro(new DTO.SaidaAsterisk { Valor = texto });
        }

        public List<DTO.SaidaAsterisk> Listar(long id)
        {
            return new DAL.SaidaAsterisk().Select(id);
        }

        public void Limpar(params long[] ids)
        {
            if (ids == null)
                new DAL.SaidaAsterisk().Limpar();
            else
                foreach (long id in ids)
                    new DAL.SaidaAsterisk().Limpar(id);
        }

        public void Limpar()
        {
            Limpar(null);
        }

        public long ObterMaiorId()
        {
            return new DAL.SaidaAsterisk().ObterMaiorId();
        }
    }
}
