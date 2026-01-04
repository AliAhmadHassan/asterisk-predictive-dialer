using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    public class TarifaAlternativa : Base<DTO.TarifaAlternativa>
    {
        public virtual List<DTO.TarifaAlternativa> Obter(bool ativo)
        {
            var LRetorno = new List<DTO.TarifaAlternativa>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPStarifaalternativaAtivos", Conn))
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
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.TarifaAlternativa>(Dr));
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

        public virtual List<DTO.TarifaAlternativa> SelectPeloNome(string Id)
        {
            var LRetorno = new List<DTO.TarifaAlternativa>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPStarifaalternativaPeloNome", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inDescricao", Id);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                        {
                            while (Dr.Read())
                            {
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.TarifaAlternativa>(Dr));
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

        public virtual List<DTO.TarifaAlternativa> SelectPelaCampanha(long Id)
        {
            var LRetorno = new List<DTO.TarifaAlternativa>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPStarifaalternativaPelaCampanha", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdCampanha", Id);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                        {
                            while (Dr.Read())
                            {
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.TarifaAlternativa>(Dr));
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
