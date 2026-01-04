using System.Collections.Generic;

namespace Silver.BLL
{
    public class AmdType
    {
        public List<DTO.AmdType> Obter()
        {
            return new DAL.AmdType().Select();
        }

        public DTO.AmdType Obter(long codigo)
        {
            return new DAL.AmdType().SelectPelaPK(codigo);
        }

        public DTO.AmdType Obter(string descricao)
        {
            return new DAL.AmdType().Obter(descricao);
        }
    }
}
