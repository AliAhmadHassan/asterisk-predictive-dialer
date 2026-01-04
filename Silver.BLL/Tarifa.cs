using System.Collections.Generic;

namespace Silver.BLL
{
    public class Tarifa
    {
        public List<DTO.Tarifa> Listar()
        {
            return new DAL.Tarifa().Select();
        }

        public List<DTO.Tarifa> Obter(bool ativo)
        {
            return new DAL.Tarifa().Obter(ativo);
        }

        public DTO.Tarifa Obter(int codigo)
        {
            return new DAL.Tarifa().SelectPelaPK(codigo);
        }

        public List<DTO.Tarifa> Buscar(string busca)
        {
            return new DAL.Tarifa().SelectPeloNome(busca);
        }

        public List<DTO.Tarifa> Buscar(long busca)
        {
            return new DAL.Tarifa().SelectPelaCampanha(busca);
        }

        public void Cadastrar(DTO.Tarifa Tarifa)
        {
            new DAL.Tarifa().Cadastro(Tarifa);
        }

        public void Ativar(int codigo, bool ativar)
        {
            var Tarifa = Obter(codigo);
            Tarifa.Ativo = ativar;
            Cadastrar(Tarifa);
        }
    }
}
