using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    public class Pausa : Base<DTO.Pausa>
    {
        public virtual List<DTO.Pausa> Obter(bool ativo)
        {
            var LRetorno = new List<DTO.Pausa>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSpausaAtivos", Conn))
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
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.Pausa>(Dr));
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

        public List<DTO.Pausa> SelectPelaDescricao(string descricao)
        {
            var LPausa = new List<DTO.Pausa>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSpausaPelaDescricao", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inDescricao", descricao);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                        {
                            while (Dr.Read())
                            {
                                LPausa.Add(Auxiliar.RetornaDadosEntidade<DTO.Pausa>(Dr));
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception("Erro ao consultar");
                    }
                }
            }

            return LPausa;
        }
    }
}
