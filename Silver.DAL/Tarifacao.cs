using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    public class Tarifacao : Base<DTO.Tarifacao>
    {
        public virtual List<DTO.Tarifacao> Obter(bool ativo)
        {
            var LRetorno = new List<DTO.Tarifacao>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPStarifacaoAtivos", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inAtivo", ativo);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                        {
                            while (Dr.Read())
                            {
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.Tarifacao>(Dr));
                            }
                        }
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

        public virtual List<DTO.Tarifacao> SelectPeloNome(string Id)
        {
            var LRetorno = new List<DTO.Tarifacao>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPStarifacaoPeloNome", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inNome", Id);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                        {
                            while (Dr.Read())
                            {
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.Tarifacao>(Dr));
                            }
                        }
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
