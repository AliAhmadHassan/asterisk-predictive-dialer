using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    public class Usuario : Base<DTO.Usuario>
    {
        /// <summary>
        /// Perquisa o Registro pelos Ativos
        /// </summary>
        /// <param name="ativo">if set to <c>true</c> [ativo].</param>
        /// <returns>
        /// Retorna a Entidade Informada
        /// </returns>
        public virtual List<DTO.Usuario> Obter(bool ativo)
        {
            var LRetorno = new List<DTO.Usuario>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSUsuarioAtivos", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inAtivo", ativo);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.Usuario>(Dr));
                    }
                    catch { throw; }
                }
            }

            return LRetorno;
        }

        public virtual List<DTO.Usuario> ObterOperadores(bool operadores_ativos)
        {
            var LRetorno = new List<DTO.Usuario>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSOperadoresAtivos", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inAtivo", operadores_ativos);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.Usuario>(Dr));
                    }
                    catch { throw; }
                }
            }

            return LRetorno;
        }

        /// <summary>
        /// Perquisa o Registro pela Parte do Nome o Usuario
        /// </summary>
        /// <param name="Id">Valor do Nome para Busca</param>
        /// <returns>
        /// Retorna a Entidade Informada
        /// </returns>
        public virtual List<DTO.Usuario> SelectPeloNome(string Id)
        {
            var LRetorno = new List<DTO.Usuario>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSUsuarioPeloNome", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inNome", Id);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.Usuario>(Dr));
                    }
                    catch { throw; }
                }
            }

            return LRetorno;
        }

        /// <summary>
        /// Perquisa o Registro pela Chave Estrangeira da Tabela
        /// </summary>
        /// <param name="Id">Valor da Chave Estrangeira</param>
        /// <returns>
        /// Retorna a Entidade Informada
        /// </returns>
        public virtual DTO.Usuario SelectPeloRamal(long Id)
        {
            var objRetorno = new DTO.Usuario();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSusuarioPeloRamal", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inRamal", Id);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            if (Dr.Read())
                                objRetorno = Auxiliar.RetornaDadosEntidade<DTO.Usuario>(Dr);

                    }
                    catch { throw; }
                }
            }

            return objRetorno;
        }

        /// <summary>
        /// Selects the pelo grupo.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Erro ao consultar</exception>
        public virtual List<DTO.Usuario> SelectPeloGrupo(long Id)
        {
            var LRetorno = new List<DTO.Usuario>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSusuarioPeloGrupo", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inGrupo", Id);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                        {
                            while (Dr.Read())
                            {
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.Usuario>(Dr));
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
        /// Selects the pela campanha.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Erro ao consultar</exception>
        public virtual List<DTO.Usuario> SelectPelaCampanha(long Id)
        {
            var LRetorno = new List<DTO.Usuario>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSusuarioPelaCampanha", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inCampanha", Id);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                        {
                            while (Dr.Read())
                            {
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.Usuario>(Dr));
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
