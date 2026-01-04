using System.Collections.Generic;

namespace Silver.BLL
{
    public class Operadora
    {
        public List<DTO.Operadora> Listar()
        {
            return new DAL.Operadora().Select();
        }

        public List<DTO.Operadora> Obter(bool ativo)
        {
            return new DAL.Operadora().Obter(ativo);
        }

        public DTO.Operadora Obter(int codigo)
        {
            return new DAL.Operadora().SelectPelaPK(codigo);
        }

        public List<DTO.Operadora> Buscar(string busca)
        {
            return new DAL.Operadora().SelectPeloNome(busca);
        }

        public void Cadastrar(DTO.Operadora Operadora)
        {
            new DAL.Operadora().Cadastro(Operadora);
        }

        public void Ativar(int codigo, bool ativar)
        {
            var Operadora = Obter(codigo);
            Operadora.Ativo = ativar;
            Cadastrar(Operadora);
        }
    }
}
