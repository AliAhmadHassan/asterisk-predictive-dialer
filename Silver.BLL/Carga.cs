using System.Collections.Generic;
using System;

namespace Silver.BLL
{
    public class Carga
    {
        public virtual List<DTO.Carga> SelectPeloHistorico(long id_historico)
        {
            return new DAL.Carga().SelectPeloHistorico(id_historico);
        }

        public List<DTO.Carga> Listar()
        {
            return new DAL.Carga().Select();
        }

        public List<DTO.Carga> Obter(bool ativo)
        {
            return new DAL.Carga().Obter(ativo);
        }

        public DTO.Carga Obter(int codigo)
        {
            return new DAL.Carga().SelectPelaPK(codigo);
        }

        public void AtualizarCargaAntiga(long id_campanha)
        {
            new DAL.Carga().AtualizarCargaAntiga(id_campanha);
        }

        public virtual List<DTO.Carga> SelectPelaCampanha(long Id)
        {
            return new DAL.Carga().SelectPelaCampanha(Id);
        }

        public virtual DTO.Carga SelectPeloTelefone(string Id)
        {
            return new DAL.Carga().SelectPeloTelefone(Id);

        }

        public virtual string SelectPeloTelefoneChave1(string valor)
        {
            return SelectPeloTelefone(valor).Chave1;
        }

        public virtual List<DTO.Carga> SelecionaPelaChave1(string valor)
        {
            return new DAL.Carga().SelectPelaChave(valor, "SPScargaPelaChave1");
        }

        public virtual List<DTO.Carga> SelectPelaChave2(string valor)
        {
            return new DAL.Carga().SelectPelaChave(valor, "SPSCargaPelaChave2");
        }

        public List<DTO.Carga> Buscar(long busca)
        {
            return new DAL.Carga().SelectPelaCampanha(busca);
        }

        public Int64 Cadastrar(DTO.Carga Carga)
        {
            return new DAL.Carga().Cadastro(Carga);
        }

        public void Ativar(int codigo, bool ativar)
        {
            var Carga = Obter(codigo);
            Carga.Ativo = ativar;
            Cadastrar(Carga);
        }
    }
}
