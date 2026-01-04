using System.Collections.Generic;
using Silver.Common;

namespace Silver.BLL
{
    public class Campanha
    {
        public List<DTO.Campanha> Listar()
        {
            return new DAL.Campanha().Select();
        }

        public List<DTO.Campanha> Obter(bool ativo)
        {
            return new DAL.Campanha().Obter(ativo);
        }

        public DTO.Campanha Obter(long codigo)
        {
            return new DAL.Campanha().SelectPelaPK(codigo);
        }

        public List<DTO.Campanha> SelectPeloGrupo(int IdGrupo)
        {
            return new DAL.Campanha().SelectPeloGrupo(IdGrupo);
        }

        public List<DTO.Campanha> SelectPeloGrupo(int[] IdGrupo)
        {
            var result = new List<DTO.Campanha>();

            foreach (var item in IdGrupo)
                result.AddRange(SelectPeloGrupo(item));

            return result;
        }

        public List<DTO.Campanha> Buscar(string busca)
        {
            return new DAL.Campanha().SelectPeloNome(busca);
        }

        public void Cadastrar(DTO.Campanha campanha, long solicitante = 0)
        {
            new DAL.Campanha().Cadastro(campanha);
            if (solicitante > 0)
                ControleSistema.IncluirEvento(DTO.EventoControleSistema.Recarregar_Queue, new DTO.ControleSistema { Valor = "0", Situacao = 1, Campanha = campanha.Id, Solicitante = solicitante });
        }

        public void Ativar(int codigo, bool ativar)
        {
            var campanha = Obter(codigo);
            campanha.Ativo = ativar.ToInt32();
            Cadastrar(campanha);
        }

        public DTO.CampanhaStatus StatusCampanha(int id_campanha)
        {
            return new DAL.Campanha().StatusCampanha(id_campanha);
        }
    }
}