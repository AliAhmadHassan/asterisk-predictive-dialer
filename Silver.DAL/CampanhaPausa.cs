using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    public class CampanhaPausa
    {
        public DTO.CampanhaPausa Select(DTO.CampanhaPausa campanhaPausa)
        {
            DTO.CampanhaPausa cp = null;

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPScampanhapausaPorAmbos", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdPausa", campanhaPausa.IdPausa);
                        cmd.Parameters.AddWithValue("inIdCampanha", campanhaPausa.IdCampanha);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                        {
                            if (Dr.Read())
                            {
                                cp = Auxiliar.RetornaDadosEntidade<DTO.CampanhaPausa>(Dr);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception("Erro ao consultar");
                    }
                }
            }

            return cp;
        }

        public List<DTO.CampanhaPausa> SelectPelaCampanha(DTO.CampanhaPausa campanhaPausa)
        {
            var LCampanhaPausa = new List<DTO.CampanhaPausa>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPScampanhapausaPelaCampanha", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdCampanha", campanhaPausa.IdCampanha);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                        {
                            while (Dr.Read())
                            {
                                LCampanhaPausa.Add(Auxiliar.RetornaDadosEntidade<DTO.CampanhaPausa>(Dr));
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception("Erro ao consultar");
                    }
                }
            }

            return LCampanhaPausa;
        }

        public List<DTO.CampanhaPausa> SelectPelaCampanha(long id_campanha)
        {
            var LCampanhaPausa = new List<DTO.CampanhaPausa>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPScampanhapausaPelaCampanha", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdCampanha", id_campanha);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LCampanhaPausa.Add(Auxiliar.RetornaDadosEntidade<DTO.CampanhaPausa>(Dr));
                    }
                    catch (Exception) { throw; }
                }
            }

            return LCampanhaPausa;
        }

        public List<DTO.CampanhaPausa> SelectPelaPausa(DTO.CampanhaPausa campanhaPausa)
        {
            var LCampanhaPausa = new List<DTO.CampanhaPausa>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPScampanhapausaPelaPausa", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdPausa", campanhaPausa.IdPausa);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                        {
                            while (Dr.Read())
                            {
                                LCampanhaPausa.Add(Auxiliar.RetornaDadosEntidade<DTO.CampanhaPausa>(Dr));
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception("Erro ao consultar");
                    }
                }
            }

            return LCampanhaPausa;
        }

        public void Remover(DTO.CampanhaPausa campanhaPausa)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPDcampanhapausa", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdCampanha", campanhaPausa.IdCampanha);
                        cmd.Parameters.AddWithValue("inIdPausa", campanhaPausa.IdPausa);
                        Conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw new Exception("Erro ao deletar o registro");
                    }
                }
            }
        }

        public void RemoverPelaCampanha(DTO.CampanhaPausa campanhaPausa)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPDcampanhapausaPelaCampanha", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdCampanha", campanhaPausa.IdCampanha);
                        Conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw new Exception("Erro ao deletar o registro");
                    }
                }
            }
        }

        public void RemoverPelaPausa(DTO.CampanhaPausa campanhaPausa)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPDcampanhapausaPelaPausa", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdPausa", campanhaPausa.IdPausa);
                        Conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw new Exception("Erro ao deletar o registro");
                    }
                }
            }
        }

        public void Cadastrar(DTO.CampanhaPausa campanhaPausa)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPIcampanhapausa", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdCampanha", campanhaPausa.IdCampanha);
                        cmd.Parameters.AddWithValue("inIdPausa", campanhaPausa.IdPausa);
                        Conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw new Exception("Erro ao inserir registro.");
                    }
                    finally
                    {
                    }
                }
            }
        }
    }
}
