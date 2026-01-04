using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.BLL
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva]
    ///             Data: 25/10/2013
    /// </summary>
    public class HistoricoCarga
    {
        public List<DTO.HistoricoCarga> Listar()
        {
            return new DAL.HistoricoCarga().Select();
        }

        public DTO.HistoricoCarga Obter(long id)
        {
            return new DAL.HistoricoCarga().SelectPelaPK(id);
        }

        public long Cadastrar(DTO.HistoricoCarga historico)
        {
            return new DAL.HistoricoCarga().Cadastro(historico);                                           
        }

        public List<DTO.HistoricoCarga> Listar(DateTime data_cadastro)
        {
            return new DAL.HistoricoCarga().Select(data_cadastro);
        }

        public List<DTO.HistoricoCarga> ListarDescricao(string descricao_caga)
        {
            return new DAL.HistoricoCarga().SelectDescricao(descricao_caga);
        }

        public List<DTO.HistoricoCarga> ListarNomeArquivo(string nome_arquivo)
        {
            return new DAL.HistoricoCarga().SelectNomeArquivo(nome_arquivo);
        }

        public List<DTO.HistoricoCarga> ListarCampanha(long id_campanha)
        {
            return new DAL.HistoricoCarga().SelectCampanha(id_campanha);
        }
    }
}