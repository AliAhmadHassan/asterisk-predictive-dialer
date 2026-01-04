using System.Collections.Generic;

namespace Silver.BLL
{
    public class Grupo
    {
        public List<DTO.Grupo> Listar()
        {
            return new DAL.Grupo().Select();
        }

        public List<DTO.Grupo> Obter(bool ativo)
        {
            return new DAL.Grupo().Obter(ativo);
        }

        public DTO.Grupo Obter(int codigo)
        {
            return new DAL.Grupo().SelectPelaPK(codigo);
        }

        public List<DTO.Grupo> Buscar(string busca)
        {
            return new DAL.Grupo().SelectPeloNome(busca);
        }

        public void Cadastrar(DTO.Grupo grupo)
        {
            new DAL.Grupo().Cadastro(grupo);
        }

        public void Ativar(int codigo, bool ativar)
        {
            var grupo = Obter(codigo);
            grupo.Ativo = ativar;
            Cadastrar(grupo);
        }

        public List<DTO.Grupo> ObterGruposFilhos(long id_grupo)
        {
            List<DTO.Grupo> grupos = new List<DTO.Grupo>();
            grupos.Add(Obter((int)id_grupo));
            grupos.AddRange(new Silver.DAL.Grupo().ObterGruposFilhos(id_grupo));
            return grupos;
        }
    }
}
