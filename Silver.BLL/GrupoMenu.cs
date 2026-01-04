using System.Collections.Generic;
using Silver.DTO;

namespace Silver.BLL
{
    public class GrupoMenu
    {
        public List<DTO.GrupoMenu> Obter(long id, TipoConsulta tipo)
        {
            switch (tipo)
            {
                case TipoConsulta.PeloGrupo:
                    return new DAL.GrupoMenu().SelectPeloGrupo(new DTO.GrupoMenu(id, 0));
                case TipoConsulta.PeloMenu:
                    return new DAL.GrupoMenu().SelectPeloMenu(new DTO.GrupoMenu(0, id));
                default:
                    return null;
            }
        }

        public DTO.GrupoMenu Obter(long idGrupo, long idMenu)
        {
            return new DAL.GrupoMenu().Select(new DTO.GrupoMenu(idGrupo, idMenu));
        }

        public void Remover(DTO.GrupoMenu grupoMenu, TipoExclusao tipo)
        {
            switch (tipo)
            {
                case TipoExclusao.PeloGrupo:
                    new DAL.GrupoMenu().RemoverPeloGrupo(grupoMenu);
                    break;
                case TipoExclusao.PeloMenu:
                    new DAL.GrupoMenu().RemoverPeloMenu(grupoMenu);
                    break;
                case TipoExclusao.Ambos:
                    new DAL.GrupoMenu().Remover(grupoMenu);
                    break;
            }
        }

        public void Incluir(DTO.GrupoMenu grupoMenu)
        {
            new DAL.GrupoMenu().Inserir(grupoMenu);
        }

        public void Cadastrar(long id, List<long> lst, TipoConsulta tipo)
        {
            var lstDTO = Obter(id, tipo);
            switch (tipo)
            {
                case TipoConsulta.PeloGrupo:
                    for (var i = 0; i < lstDTO.Count; i++)
                    {
                        Remover(lstDTO[i], TipoExclusao.PeloGrupo);
                    }
                    for (var j = 0; j < lst.Count; j++)
                    {
                        Incluir(new DTO.GrupoMenu(id, lst[j]));
                    }
                    break;
                case TipoConsulta.PeloMenu:
                    for (var i = 0; i < lstDTO.Count; i++)
                    {
                        Remover(lstDTO[i], TipoExclusao.PeloMenu);
                    }
                    for (var j = 0; j < lst.Count; j++)
                    {
                        Incluir(new DTO.GrupoMenu(lst[j], id));
                    }
                    break;
            }
        }
    }
}
