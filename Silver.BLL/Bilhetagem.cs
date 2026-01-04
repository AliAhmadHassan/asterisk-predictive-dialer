using System.Collections.Generic;

namespace Silver.BLL
{
    public class Bilhetagem
    {
        public List<DTO.Bilhetagem> Listar()
        {
            return new DAL.Bilhetagem().Select();
        }

        public List<DTO.Bilhetagem> Obter(bool ativo)
        {
            return new DAL.Bilhetagem().Obter(ativo);
        }

        public DTO.Bilhetagem Obter(long codigo)
        {
            return new DAL.Bilhetagem().SelectPelaPK(codigo);
        }

        public List<DTO.Bilhetagem> ObterLigacoesParaAnalise()
        {
            return new List<DTO.Bilhetagem>();
        }

        public long Cadastrar(DTO.Bilhetagem bilhetagem)
        {
            return new DAL.Bilhetagem().Cadastro(bilhetagem);
        }
    }
}
