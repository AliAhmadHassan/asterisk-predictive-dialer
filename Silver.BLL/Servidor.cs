using System.Collections.Generic;

namespace Silver.BLL
{
    public class Servidor
    {
        public List<DTO.Servidor> Listar()
        {
            return new DAL.Servidor().Select();
        }

        public List<DTO.Servidor> Obter(bool ativo)
        {
            return new DAL.Servidor().Obter(ativo);
        }

        public DTO.Servidor Obter(int codigo)
        {
            return new DAL.Servidor().SelectPelaPK(codigo);
        }

        public List<DTO.Servidor> Buscar(string busca)
        {
            return new DAL.Servidor().SelectPeloNome(busca);
        }

        public void Cadastrar(DTO.Servidor Servidor)
        {
            new DAL.Servidor().Cadastro(Servidor);
        }

        public void Ativar(int codigo, bool ativar)
        {
            var Servidor = Obter(codigo);
            Servidor.Ativo = ativar;
            Cadastrar(Servidor);
        }
    }
}
