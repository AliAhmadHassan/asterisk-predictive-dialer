using System.Collections.Generic;

namespace Silver.BLL
{
    public class TarifaAlternativa
    {
        public List<DTO.TarifaAlternativa> Listar()
        {
            return new DAL.TarifaAlternativa().Select();
        }

        public List<DTO.TarifaAlternativa> Obter(bool ativo)
        {
            return new DAL.TarifaAlternativa().Obter(ativo);
        }

        public DTO.TarifaAlternativa Obter(int codigo)
        {
            return new DAL.TarifaAlternativa().SelectPelaPK(codigo);
        }

        public List<DTO.TarifaAlternativa> Buscar(string busca)
        {
            return new DAL.TarifaAlternativa().SelectPeloNome(busca);
        }

        public List<DTO.TarifaAlternativa> Buscar(long busca)
        {
            return new DAL.TarifaAlternativa().SelectPelaCampanha(busca);
        }

        public void Cadastrar(DTO.TarifaAlternativa TarifaAlternativa)
        {
            new DAL.TarifaAlternativa().Cadastro(TarifaAlternativa);
        }

        public void Ativar(int codigo, bool ativar)
        {
            var TarifaAlternativa = Obter(codigo);
            TarifaAlternativa.Ativo = ativar;
            Cadastrar(TarifaAlternativa);
        }
    }
}
