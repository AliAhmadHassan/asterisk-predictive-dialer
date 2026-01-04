using System.Collections.Generic;

namespace Silver.BLL
{
    public class Pausa
    {
        public List<DTO.Pausa> Listar()
        {
            return new DAL.Pausa().Select();
        }

        public List<DTO.Pausa> Obter(bool ativo)
        {
            return new DAL.Pausa().Obter(ativo);
        }

        public List<DTO.Pausa> Obter(string descricao)
        {
            return new DAL.Pausa().SelectPelaDescricao(descricao);
        }

        public DTO.Pausa Obter(long codigoPausa)
        {
            return new DAL.Pausa().SelectPelaPK(codigoPausa);
        }

        public long Cadastro(DTO.Pausa pausa)
        {
            new DAL.Pausa().Cadastro(pausa);
            return pausa.Id;
        }

        public void Ativar(long codigoPausa, bool ativar)
        {
            var pausa = Obter(codigoPausa);
            pausa.Ativo = ativar;
            Cadastro(pausa);
        }
    }
}
