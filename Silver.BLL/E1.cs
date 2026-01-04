using System.Collections.Generic;

namespace Silver.BLL
{
    public class E1
    {
        /// <summary>
        /// Lista dos os E1
        /// </summary>
        /// <returns></returns>
        public List<DTO.E1> Listar()
        {
            return new DAL.E1().Select();
        }

        public List<DTO.E1> Obter(bool ativo)
        {
            return new DAL.E1().Obter(ativo);
        }

        public DTO.E1 Obter(int codigo)
        {
            return new DAL.E1().SelectPelaPK(codigo);
        }

        public List<DTO.E1> Buscar(string busca)
        {
            return new DAL.E1().SelectPeloNome(busca);
        }

        public void Cadastrar(DTO.E1 E1)
        {
            new DAL.E1().Cadastro(E1);
        }

        public void Ativar(int codigo, bool ativar)
        {
            var E1 = Obter(codigo);
            E1.Ativo = ativar;
            Cadastrar(E1);
        }
    }
}
