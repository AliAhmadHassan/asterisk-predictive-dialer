using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    public class Campanha : Base<DTO.Campanha>
    {
        /// <summary>
        /// Perquisa o Registro pelos Ativos
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <param name="Ativo">Valor</param>
        /// <returns>Retorna a Entidade Informada</returns>
        public virtual List<DTO.Campanha> Obter(bool ativo)
        {
            var LRetorno = new List<DTO.Campanha>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPScampanhaAtivos", Conn))
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
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.Campanha>(Dr));
                            }
                        }
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

        /// <summary>
        /// Perquisa o Registro pela Parte do Nome o Campanha
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <param name="Id">Valor do Nome para Busca</param>
        /// <returns>Retorna a Entidade Informada</returns>
        public virtual List<DTO.Campanha> SelectPeloNome(string Id)
        {
            var LRetorno = new List<DTO.Campanha>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPScampanhaPeloNome", Conn))
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
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.Campanha>(Dr));
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

        public virtual List<DTO.Campanha> SelectPeloGrupo(int IdGrupo)
        {
            var LRetorno = new List<DTO.Campanha>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPScampanhaPeloGrupo", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdGrupo", IdGrupo);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                        {
                            while (Dr.Read())
                            {
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.Campanha>(Dr));
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

        public DTO.CampanhaStatus StatusCampanha(int id_campanha)
        {
            var status_campanha = new DTO.CampanhaStatus();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPScampanhaStatus", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdCampanha", id_campanha);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                        {
                            while (Dr.Read())
                            {
                                status_campanha.Carga = Convert.ToInt32(Dr["Carga"]);
                                status_campanha.Telefone = Convert.ToInt32(Dr["Telefone"]);
                                status_campanha.Operador = Convert.ToInt32(Dr["Operador"]);
                            }
                            Dr.Close();
                        }
                    }
                    catch { throw; }
                    finally { }
                }
            }

            return status_campanha;
        }
    }
}
