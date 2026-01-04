using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    public class BilhetagemGravacao : Base<DTO.BilhetagemGravacaoGrid>
    {
        public List<DTO.BilhetagemGravacaoGrid> Select(long id_campanha, DateTime inicio, DateTime fim)
        {
            var LRetorno = new List<DTO.BilhetagemGravacaoGrid>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSgravacao", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inInicio", new DateTime(inicio.Year, inicio.Month, inicio.Day, 0, 0, 0));
                        cmd.Parameters.AddWithValue("inFim", new DateTime(fim.Year, fim.Month, fim.Day, 23, 59, 59));
                        cmd.Parameters.AddWithValue("inIdCampanha", id_campanha);
                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.BilhetagemGravacaoGrid>(Dr));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                    }
                }
            }

            return LRetorno;
        }

        public DTO.BilhetagemGravacao SelectPelaBilhetagem(long id_bilhetagem)
        {
            var LRetorno = new List<DTO.BilhetagemGravacao>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSbilhetagemarquivopeloID", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inId", id_bilhetagem);
                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.BilhetagemGravacao>(Dr));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                    }
                }
            }

            if (LRetorno.Count > 0)
                return LRetorno[0];
            else
                return null;
        }

        public List<DTO.BilhetagemGravacaoGrid> ListarPeloRamal(string ramal, DateTime inicio, DateTime fim)
        {
            var LRetorno = new List<DTO.BilhetagemGravacaoGrid>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSgravacaoPeloRamal", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inInicio", new DateTime(inicio.Year, inicio.Month, inicio.Day, 0, 0, 0));
                        cmd.Parameters.AddWithValue("inFim", new DateTime(fim.Year, fim.Month, fim.Day, 23, 59, 59));
                        cmd.Parameters.AddWithValue("inRamal", ramal);
                        
                        Conn.Open();

                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.BilhetagemGravacaoGrid>(Dr));
                    }
                    catch { throw; }
                    finally { }
                }
            }

            return LRetorno;
        }

        public List<DTO.BilhetagemGravacaoGrid> ListarPeloCpfCnpj(string cpf_cnpj, DateTime inicio, DateTime fim)
        {
            var LRetorno = new List<DTO.BilhetagemGravacaoGrid>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSgravacaoPeloCpfCnpj", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inInicio", new DateTime(inicio.Year, inicio.Month, inicio.Day, 0, 0, 0));
                        cmd.Parameters.AddWithValue("inFim", new DateTime(fim.Year, fim.Month, fim.Day, 23, 59, 59));
                        cmd.Parameters.AddWithValue("inCpfCnpj", cpf_cnpj);

                        Conn.Open();

                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.BilhetagemGravacaoGrid>(Dr));
                    }
                    catch { throw; }
                    finally { }
                }
            }

            return LRetorno;
        }
    }
}
