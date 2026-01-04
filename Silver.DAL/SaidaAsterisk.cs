using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    public class SaidaAsterisk : Base<DTO.SaidaAsterisk>
    {
        public void Limpar(long id)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPDsaidaasterisk", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inId", id);
                        Conn.Open();
                        cmd.ExecuteScalar();
                    }
                    catch { throw; }
                    finally { }
                }
            }
        }

        public void Limpar()
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPDsaidaasterisk", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        Conn.Open();
                        cmd.ExecuteScalar();
                    }
                    catch { throw; }
                    finally { }
                }
            }
        }

        /// <summary>
        /// Retorno o maior id da tabela de escuta do asterisk
        /// </summary>
        /// <returns></returns>
        public long ObterMaiorId()
        {
            long retorno = 0;
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSsaidasteriskmaxid", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        Conn.Open();
                        retorno = Convert.ToInt64(cmd.ExecuteScalar());
                    }
                    catch { throw new Exception("Erro ao consultar"); }
                    finally { }
                }
            }
            return retorno;
        }

        public List<DTO.SaidaAsterisk> Select(long id)
        {
            var LRetorno = new List<DTO.SaidaAsterisk>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSsaidaasteriskponteiro", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inId", id);
                        Conn.Open();

                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.SaidaAsterisk>(Dr));
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
