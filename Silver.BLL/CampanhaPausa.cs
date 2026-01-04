using System.Collections.Generic;
using Silver.DTO;

namespace Silver.BLL
{
    public class CampanhaPausa
    {
        /// <summary>
        /// Retorna uma lista de objetos [DTO.CampanhaPausa] diferente para cada tipo de consulta.
        /// </summary>
        /// <param name="campanhaPausa">Objeto DTO.CampanhaPausa</param>
        /// <param name="tipo">Enumerado TipoConsulta:
        /// PelaCampanha = Faz a busca pelo Id da campanha
        /// PelaPausa = Faz a busca pelo Id da pausa</param>
        /// <returns></returns>
        public List<DTO.CampanhaPausa> Obter(long id, TipoID tipo)
        {
            switch (tipo)
            {
                case TipoID.Campanha:
                    return new DAL.CampanhaPausa().SelectPelaCampanha(new DTO.CampanhaPausa(id, 0));
                case TipoID.Pausa:
                    return new DAL.CampanhaPausa().SelectPelaPausa(new DTO.CampanhaPausa(0, id));
                default:
                    return null;
            }
        }

        public List<DTO.CampanhaPausa> SelectPelaCampanha(long id_campanha)
        {
            return new DAL.CampanhaPausa().SelectPelaCampanha(id_campanha);
        }

        /// <summary>
        /// Retorna um objeto [DTO.CampanhaPausa] conforme os números de Id informados.
        /// </summary>
        /// <param name="idCampanha"></param>
        /// <param name="idPausa"></param>
        /// <returns></returns>
        public DTO.CampanhaPausa Obter(long idCampanha, long idPausa)
        {
            return new DAL.CampanhaPausa().Select(new DTO.CampanhaPausa(idCampanha, idPausa));
        }

        public void Remover(DTO.CampanhaPausa campanhaPausa, TipoExclusao tipo)
        {
            switch (tipo)
            {
                case TipoExclusao.PelaCampanha:
                    new DAL.CampanhaPausa().RemoverPelaCampanha(campanhaPausa);
                    break;
                case TipoExclusao.PelaPausa:
                    new DAL.CampanhaPausa().RemoverPelaPausa(campanhaPausa);
                    break;
                case TipoExclusao.Ambos:
                    new DAL.CampanhaPausa().Remover(campanhaPausa);
                    break;
            }
        }

        public void Incluir(DTO.CampanhaPausa CampanhaPausa)
        {
            new DAL.CampanhaPausa().Cadastrar(CampanhaPausa);
        }

        public void Cadastrar(long id, List<long> lst, TipoID tipo)
        {
            var lstDTO = Obter(id, tipo);
            switch (tipo)
            {
                case TipoID.Campanha:
                    for (var i = 0; i < lstDTO.Count; i++)
                    {
                        Remover(lstDTO[i], TipoExclusao.PelaCampanha);
                    }
                    for (var j = 0; j < lst.Count; j++)
                    {
                        Incluir(new DTO.CampanhaPausa(id, lst[j]));
                    }
                    break;
                case TipoID.Pausa:
                    for (var i = 0; i < lstDTO.Count; i++)
                    {
                        Remover(lstDTO[i], TipoExclusao.PelaPausa);
                    }
                    for (var j = 0; j < lst.Count; j++)
                    {
                        Incluir(new DTO.CampanhaPausa(lst[j], id));
                    }
                    break;
            }
        }

        public void Cadastrar(List<long> idCampanhas, long idPausa)
        {
            DTO.CampanhaPausa campanhaPausa = null;

            foreach (long idCampanha in idCampanhas)
            {
                campanhaPausa = new DTO.CampanhaPausa(idCampanha, idPausa);

                if (Obter(campanhaPausa.IdCampanha, campanhaPausa.IdPausa) == null)
                {
                    new DAL.CampanhaPausa().Cadastrar(campanhaPausa);
                }
            }
        }
    }
}
