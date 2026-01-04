using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Silver.DTO;
using System.IO;
using System.Xml.Serialization;
using Silver.Common;

namespace Silver.DAL
{
    /// <summary>
    /// Desenvolvida por: Ali Ahmad Hassan
    ///             Data: 09/04/2013
    /// </summary>
    public class RelCampanha
    {
        public DTO.RelCampanha SelectQueueShow(long id)
        {
            DTO.RelCampanha Retorno = null;
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSRelCampanhaPelaCampanha", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdCampanha", id);
                        Conn.Open();

                        using (var Dr = cmd.ExecuteReader())
                            if (Dr.Read())
                                Retorno = (DTO.RelCampanha)new XmlSerializer(typeof(DTO.RelCampanha), "RelCampanha").Deserialize(new StringReader(Dr["QueueShow"].ToString()));
                    }
                    catch{}
                    finally{}
                }
            }
            return Retorno;
        }

        public List<RelCampanhaQueueStatus> SelectQueueStatus(long id)
        {
            List<RelCampanhaQueueStatus> Retorno = new List<RelCampanhaQueueStatus>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSRelCampanhaPelaCampanha", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdCampanha", id);
                        Conn.Open();

                        using (var Dr = cmd.ExecuteReader())
                            if (Dr.Read())
                            {
                                Retorno = (List<RelCampanhaQueueStatus>)new XmlSerializer(typeof(List<RelCampanhaQueueStatus>), "RelCampanhaQueueStatus").Deserialize(new StringReader(Dr["QueueStatus"].ToString()));
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
            return Retorno;
        }

        /// <summary>
        /// Retorna Toda a tabela
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <returns>Lista do tipo de Entidade informada</returns>
        public virtual List<DTO.RelCampanhaDB> Select()
        {
            var atributos = Auxiliar.RetornoAtributos<DTO.RelCampanhaDB>((DTO.RelCampanhaDB)Activator.CreateInstance(typeof(DTO.RelCampanhaDB)));

            var LObjeto = new List<DTO.RelCampanhaDB>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand(atributos.NomeProcedureListarTodos, Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LObjeto.Add(Auxiliar.RetornaDadosEntidade<DTO.RelCampanhaDB>(Dr));
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
            return LObjeto;
        }

        /// <summary>
        /// Perquisa o Registro pela Chave Primaria da Tabela
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <param name="Id">Valor da Chave Primaria</param>
        /// <returns>Retorna a Entidade Informada</returns>
        public virtual DTO.RelCampanhaDB SelectPelaPK(long Id)
        {
            var _entidade = (DTO.RelCampanhaDB)Activator.CreateInstance(typeof(DTO.RelCampanhaDB));
            var atributos = Auxiliar.RetornoAtributos<DTO.RelCampanhaDB>((DTO.RelCampanhaDB)Activator.CreateInstance(typeof(DTO.RelCampanhaDB)));

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand(atributos.NomeProcedurePelaPK, Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("in" + atributos.NomeChavePrimeria, (int)Id);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                        {
                            if (Dr.Read())
                                _entidade = Auxiliar.RetornaDadosEntidade<DTO.RelCampanhaDB>(Dr);
                            else
                                _entidade = null;
                        }
                    }
                    catch (Exception) { throw; }
                    finally { }
                }
            }

            return _entidade;
        }

        /// <summary>
        /// Entidade a ser Removida do Banco de Dados
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <param name="Entidade">Nome da Entidade a ser Removido</param>
        public virtual void Remover(DTO.RelCampanhaDB Entidade)
        {
            var atributos = Auxiliar.RetornoAtributos<DTO.RelCampanhaDB>(Entidade);

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand(atributos.NomeProcedureRemover, Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("in" + atributos.NomeChavePrimeria, (long)Entidade.GetType().GetProperty(atributos.NomeChavePrimeria).GetValue(Entidade, null));
                        Conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw new Exception("Erro ao deletar o registro");
                    }
                    finally
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Insere registro Generico no Banco de dados
        ///
        /// Autor: Ali Ahmad Hassan
        /// Data: 2013-04-09
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <param name="Entidade">Nome da Entidade a ser inserido</param>
        /// <param name="CampoChave">Noma do Campo da Chave Primaria</param>
        /// <param name="NomeProcedure">Nome da Procedure para Inserir</param>
        public long Inserir(ref DTO.RelCampanhaDB Entidade, string ChavePrimaria, string NomeProcedure)
        {
            object result = 0;
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand(NomeProcedure, Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        var parametros = Auxiliar.GeraParametros<DTO.RelCampanhaDB>(Entidade);

                        foreach (MySqlParameter Param in parametros)
                            if (Param.ParameterName != "inId")
                                cmd.Parameters.Add(Param);

                        Conn.Open();
                        result = cmd.ExecuteScalar();
                        Entidade.GetType().GetProperty(ChavePrimaria).SetValue(Entidade, result.ToInt64(), null);
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
            return result.ToInt64();
        }

        /// <summary>
        /// Altera registro Generico no Banco de dados
        ///
        /// Autor: Ali Ahmad Hassan
        /// Data: 2013-04-09
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <param name="Entidade">Nome da Entidade a ser inserido</param>
        /// <param name="NomeProcedure">Nome da Procedure para Alterar</param>
        public void Alterar(DTO.RelCampanhaDB Entidade, string NomeProcedure)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand(NomeProcedure, Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        foreach (MySqlParameter Param in Auxiliar.GeraParametros<DTO.RelCampanhaDB>(Entidade))
                        {
                            cmd.Parameters.Add(Param);
                        }
                        Conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw new Exception("Erro ao Alterar registro de Categoria");
                    }
                    finally
                    {
                    }
                }
            }
        }
    }
}
