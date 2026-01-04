using System.Collections.Generic;

namespace Silver.BLL
{
    public class TarifaRegra
    {
        public List<DTO.TarifaRegra> Listar()
        {
            return new DAL.TarifaRegra().Select();
        }

        public List<DTO.TarifaRegra> Obter(bool ativo)
        {
            return new DAL.TarifaRegra().Obter(ativo);
        }

        public DTO.TarifaRegra Obter(int codigo)
        {
            return new DAL.TarifaRegra().SelectPelaPK(codigo);
        }

        public List<DTO.TarifaRegra> Buscar(string busca)
        {
            return new DAL.TarifaRegra().SelectPeloNome(busca);
        }

        public void Cadastrar(DTO.TarifaRegra TarifaRegra)
        {
            new DAL.TarifaRegra().Cadastro(TarifaRegra);
        }

        public void Ativar(int codigo, bool ativar)
        {
            var TarifaRegra = Obter(codigo);
            TarifaRegra.Ativo = ativar;
            Cadastrar(TarifaRegra);
        }
    }
}
