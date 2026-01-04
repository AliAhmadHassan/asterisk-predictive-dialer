using System.Collections.Generic;

namespace Silver.BLL
{
    public class Tarifacao
    {
        public List<DTO.Tarifacao> Listar()
        {
            return new DAL.Tarifacao().Select();
        }

        public List<DTO.Tarifacao> Obter(bool ativo)
        {
            return new DAL.Tarifacao().Obter(ativo);
        }

        public DTO.Tarifacao Obter(int codigo)
        {
            return new DAL.Tarifacao().SelectPelaPK(codigo);
        }

        public List<DTO.Tarifacao> Buscar(string busca)
        {
            return new DAL.Tarifacao().SelectPeloNome(busca);
        }

        public void Cadastrar(DTO.Tarifacao Tarifacao)
        {
            new DAL.Tarifacao().Cadastro(Tarifacao);
        }

        public void Ativar(int codigo, bool ativar)
        {
            var Tarifacao = Obter(codigo);
            Tarifacao.Ativo = ativar;
            Cadastrar(Tarifacao);
        }
    }
}
