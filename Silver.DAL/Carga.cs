using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    public class Carga : Base<DTO.Carga>
    {
        /// <summary>
        /// Perquisa o Registro pelos Ativos
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <param name="Ativo">Valor</param>
        /// <returns>Retorna a Entidade Informada</returns>
        public virtual List<DTO.Carga> Obter(bool ativo)
        {
            var LRetorno = new List<DTO.Carga>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPScargaAtivos", Conn))
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
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.Carga>(Dr));
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
        /// Perquisa o Registro pela Parte do Nome o Carga
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <param name="Id">Valor do Nome para Busca</param>
        /// <returns>Retorna a Entidade Informada</returns>
        public virtual List<DTO.Carga> SelectPelaCampanha(long Id)
        {
            var LRetorno = new List<DTO.Carga>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPScargaPelaCampanha", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inId", Id);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                        {
                            while (Dr.Read())
                            {
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.Carga>(Dr));
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
        /// Lista a carga pelo id do histórico
        /// </summary>
        /// <param name="id_historico">id do historico</param>
        /// <returns></returns>
        public virtual List<DTO.Carga> SelectPeloHistorico(long id_historico)
        {
            var LRetorno = new List<DTO.Carga>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPScargaPeloHistorico", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdHistorico", id_historico);
                        Conn.Open();

                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.Carga>(Dr));
                    }
                    catch (Exception ex) { throw ex; }
                    finally { }
                }
            }
            return LRetorno;
        }

        /// <summary>
        /// Perquisa o Registro pela Parte do Nome o Carga
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <param name="Id">Valor do Nome para Busca</param>
        /// <returns>Retorna a Entidade Informada</returns>
        public virtual DTO.Carga SelectPeloTelefone(string Id)
        {
            DTO.Carga carga = new DTO.Carga();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPScargaPeloTelefone", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inTelefone", Id);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                        {
                            if (Dr.Read())
                            {
                                carga = Auxiliar.RetornaDadosEntidade<DTO.Carga>(Dr);
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

            return carga;
        }

        public List<DTO.Carga> SelectPelaChave(string valor, string procedure)
        {
            var LRetorno = new List<DTO.Carga>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand(procedure, Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inChave", valor);
                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.Carga>(Dr));
                    }
                    catch { throw; }
                }
            }

            return LRetorno;
        }

        public void AtualizarCargaAntiga(long id_campanha)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPUcargaNovaCarga", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdCampanha", id_campanha);
                        Conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch { throw; }
                    finally
                    {

                    }
                }
            }
        }
    }
}
