using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    public class Menu : Base<DTO.Menu>
    {
        /// <summary>
        /// Perquisa o Registro pelos Ativos
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <param name="Ativo">Valor</param>
        /// <returns>Retorna a Entidade Informada</returns>
        public virtual List<DTO.Menu> Obter(bool ativo)
        {
            var LRetorno = new List<DTO.Menu>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSmenuAtivos", Conn))
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
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.Menu>(Dr));
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

        /// <summary>
        /// Perquisa o Registro pela Parte do Nome o Menu
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <param name="Id">Valor do Nome para Busca</param>
        /// <returns>Retorna a Entidade Informada</returns>
        public virtual List<DTO.Menu> SelectPeloNome(string Id)
        {
            var LRetorno = new List<DTO.Menu>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSmenuPeloNome", Conn))
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
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.Menu>(Dr));
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

        public List<DTO.Menu> Obter(long grupo_menu)
        {
            var LRetorno = new List<DTO.Menu>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSmenuPeloGrupo", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inGrupoMenu", grupo_menu);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.Menu>(Dr));
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return LRetorno;
        }

        public List<DTO.Menu> Obter(long operador, long grupo_menu)
        {
            var LRetorno = new List<DTO.Menu>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSmenuPeloUsuario", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inGrupoMenu", grupo_menu);
                        cmd.Parameters.AddWithValue("inUsuario", operador);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.Menu>(Dr));
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return LRetorno;
        }
    }
}
