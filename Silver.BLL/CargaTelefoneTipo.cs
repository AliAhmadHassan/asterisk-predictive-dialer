using System.Linq;

namespace Silver.BLL
{
    public class CargaTelefoneTipo
    {
        public DTO.CargaTelefoneTipo Buscar(long idTelefoneTipo)
        {
            return new DAL.CargaTelefoneTipo().SelectPelaPK(idTelefoneTipo);
        }

        public DTO.CargaTelefoneTipo Buscar(string busca)
        {
            return new DAL.CargaTelefoneTipo().SelectPeloNome(busca).FirstOrDefault();
        }

        public void Cadastrar(DTO.CargaTelefoneTipo CargaTelefoneTipo)
        {
            new DAL.CargaTelefoneTipo().Cadastro(CargaTelefoneTipo);
        }
    }
}
