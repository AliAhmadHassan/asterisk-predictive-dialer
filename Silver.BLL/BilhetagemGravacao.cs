using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace Silver.BLL
{
    public class BilhetagemGravacao
    {
        /// <summary>
        /// Cadastrars the specified gravacao_bilhetagem.
        /// </summary>
        /// <param name="gravacao_bilhetagem">The gravacao_bilhetagem.</param>
        public void Cadastrar(DTO.BilhetagemGravacaoGrid gravacao_bilhetagem)
        {
            new DAL.BilhetagemGravacao().Cadastro(gravacao_bilhetagem);
        }

        /// <summary>
        /// Listars the specified id_campanha.
        /// </summary>
        /// <param name="id_campanha">The id_campanha.</param>
        /// <param name="inicio">The inicio.</param>
        /// <param name="fim">The fim.</param>
        /// <returns></returns>
        public List<DTO.BilhetagemGravacaoGrid> Listar(Int64 id_campanha, DateTime inicio, DateTime fim)
        {
            return new DAL.BilhetagemGravacao().Select(id_campanha, inicio, fim);
        }

        public List<DTO.BilhetagemGravacaoGrid> ListarPeloRamal(string ramal, DateTime inicio, DateTime fim)
        {
            return new DAL.BilhetagemGravacao().ListarPeloRamal(ramal, inicio, fim);
        }

        public List<DTO.BilhetagemGravacaoGrid> ListarPeloCpfCnpj(string cpf_cnpj, DateTime inicio, DateTime fim)
        {
            return new DAL.BilhetagemGravacao().ListarPeloCpfCnpj(cpf_cnpj, inicio, fim);
        }

        /// <summary>
        /// Selects the pela bilhetagem.
        /// </summary>
        /// <param name="id_bilhetagem">The id_bilhetagem.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// Não foi possível encontrar o path de compartilhamento de gravações
        /// or
        /// Não foi possível encontrar o arquivo de gravação na caminho informada na base de dados.
        /// </exception>
        public DTO.BilhetagemGravacao SelectPelaBilhetagem(long id_bilhetagem)
        {
            var result = new DAL.BilhetagemGravacao().SelectPelaBilhetagem(id_bilhetagem);
            if (result == null)
            {
                var path_compartilhamento_gravacao = ConfigurationManager.AppSettings["Application.Path.Gravacoes.Silver"];

                if (string.IsNullOrEmpty(path_compartilhamento_gravacao))
                    throw new Exception("Não foi possível encontrar o path de compartilhamento de gravações");

                var bilhetagem = new BLL.Bilhetagem().Obter(id_bilhetagem);

                if (!File.Exists(bilhetagem.pathgravacao))
                    throw new Exception("Não foi possível encontrar o arquivo de gravação na caminho informada na base de dados.");

                using (FileStream fs = new FileStream(bilhetagem.pathgravacao, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                        result.Gravacao = br.ReadBytes((int)fs.Length);
                }
            }
            return result;
        }
    }
}
