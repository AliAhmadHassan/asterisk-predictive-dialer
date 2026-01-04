using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 11/10/2013
    /// </summary>
    public class LogRamal : Base<DTO.LogRamal>
    {
        public List<DTO.LogRamal> ObterPeloPonteiro(long id_log)
        {
            var LRetorno = new List<DTO.LogRamal>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSlogramalIdMaior", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inId", id_log);
                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.LogRamal>(Dr));
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

        public List<DTO.LogRamal> ObterPeloPeriodo(DateTime inicio, DateTime fim)
        {
            var LRetorno = new List<DTO.LogRamal>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSlogramalEntreDatas", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inInicio", inicio);
                        cmd.Parameters.AddWithValue("inFim", fim);
                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.LogRamal>(Dr));
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

        public List<DTO.LogRamal> ObterPeloPeriodoRamal(DateTime inicio, DateTime fim, long ramal)
        {
            var LRetorno = new List<DTO.LogRamal>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSlogramalIdMaior", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inInicio", inicio);
                        cmd.Parameters.AddWithValue("inFim", fim);
                        cmd.Parameters.AddWithValue("inRamal", ramal);
                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.LogRamal>(Dr));
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
