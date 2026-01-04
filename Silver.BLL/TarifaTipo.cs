using System.Collections.Generic;

namespace Silver.BLL
{
    public class TarifaTipo
    {
        public List<DTO.TarifaTipo> Listar()
        {
            return new DAL.TarifaTipo().Select();
        }

        public List<DTO.TarifaTipo> Obter(bool ativo)
        {
            return new DAL.TarifaTipo().Obter(ativo);
        }

        public DTO.TarifaTipo Obter(int codigo)
        {
            return new DAL.TarifaTipo().SelectPelaPK(codigo);
        }

        public List<DTO.TarifaTipo> Buscar(string busca)
        {
            return new DAL.TarifaTipo().SelectPeloNome(busca);
        }

        public void Cadastrar(DTO.TarifaTipo TarifaTipo)
        {
            new DAL.TarifaTipo().Cadastro(TarifaTipo);
        }

        public void Ativar(int codigo, bool ativar)
        {
            var TarifaTipo = Obter(codigo);
            TarifaTipo.Ativo = ativar;
            Cadastrar(TarifaTipo);
        }
    }
}
