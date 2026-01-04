using System.Collections.Generic;

namespace Silver.BLL
{
    public class CargaTelefone
    {
        public DTO.CargaTelefone Obter(int codigo)
        {
            return new DAL.CargaTelefone().SelectPelaPK(codigo);
        }

        public List<DTO.CargaTelefone> ObterPelaCarga(long codigo)
        {
            return new DAL.CargaTelefone().SelectPelaCarga(codigo);
        }

        public void Cadastrar(DTO.CargaTelefone CargaTelefone)
        {
            new DAL.CargaTelefone().Cadastro(CargaTelefone);
        }

        public void Ativar(int codigo, bool ativar)
        {
            var CargaTelefone = Obter(codigo);
            CargaTelefone.Ativo = ativar;
            Cadastrar(CargaTelefone);
        }

        public void Excluir(int codigo)
        {
            var Carga = new BLL.Carga().Obter(codigo);
            var lstCargaTelefone = ObterPelaCarga(Carga.Id);
            for (var i = 0; i < lstCargaTelefone.Count; i++)
            {
                Ativar(int.Parse(lstCargaTelefone[i].Id.ToString()), false);
            }
        }

        public void AtualizarCargaAntiga(long id_campanha)
        {
            new DAL.CargaTelefone().AtualizarCargaAntiga(id_campanha);
        }
    }
}
