using System.Collections.Generic;

namespace Silver.BLL
{
    public class Rota
    {
        public List<DTO.Rota> Listar()
        {
            return new DAL.Rota().Select();
        }

        public List<DTO.Rota> Obter(bool ativo)
        {
            return new DAL.Rota().Obter(ativo);
        }

        public DTO.Rota Obter(int codigo)
        {
            return new DAL.Rota().SelectPelaPK(codigo);
        }

        public List<DTO.Rota> Buscar(string busca)
        {
            return new DAL.Rota().SelectPeloNome(busca);
        }

        public void Cadastrar(DTO.Rota Rota)
        {
            new DAL.Rota().Cadastro(Rota);
        }

        public void Ativar(int codigo, bool ativar)
        {
            var Rota = Obter(codigo);
            Rota.Ativo = ativar;
            Cadastrar(Rota);
        }
    }
}
