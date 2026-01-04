using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 25/10/2013
    /// </summary>
    public class HistoricoCarga : Base<DTO.HistoricoCarga>
    {
        public List<DTO.HistoricoCarga> Select(DateTime data_cadastro)
        {
            var LRetorno = new List<DTO.HistoricoCarga>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPShistoricocargaPelaData", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inDataInicio", data_cadastro);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.HistoricoCarga>(Dr));
                    }
                    catch
                    {
                        throw new Exception("Erro ao consultar");
                    }
                    finally
                    {
                    }
                }
            }

            return LRetorno;
        }
        
        public List<DTO.HistoricoCarga> SelectDescricao(string descricao_caga)
        {
            var LRetorno = new List<DTO.HistoricoCarga>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPShistoricocargaPelaData", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inDescricao", descricao_caga);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.HistoricoCarga>(Dr));
                    }
                    catch
                    {
                        throw new Exception("Erro ao consultar");
                    }
                    finally
                    {
                    }
                }
            }
            return LRetorno;
        }

        public List<DTO.HistoricoCarga> SelectNomeArquivo(string nome_arquivo)
        {
            var LRetorno = new List<DTO.HistoricoCarga>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPShistoricocargaPeloNomeArquivo", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inNomeArquivo", nome_arquivo);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.HistoricoCarga>(Dr));
                    }
                    catch
                    {
                        throw new Exception("Erro ao consultar");
                    }
                    finally
                    {
                    }
                }
            }
            return LRetorno;
        }

        public List<DTO.HistoricoCarga> SelectCampanha(long id_campanha)
        {
            var LRetorno = new List<DTO.HistoricoCarga>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPShistoricocargaPelaCampanha", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdCampanha", id_campanha);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.HistoricoCarga>(Dr));
                    }
                    catch
                    {
                        throw new Exception("Erro ao consultar");
                    }
                    finally
                    {
                    }
                }
            }
            return LRetorno;
        }
    }
}
