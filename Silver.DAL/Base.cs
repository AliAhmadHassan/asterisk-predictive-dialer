using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Silver.Common;

namespace Silver.DAL
{
    public class Base<T>
    {
        /// <summary>
        /// Retorna Toda a tabela
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <returns>Lista do tipo de Entidade informada</returns>
        public virtual List<T> Select()
        {
            var atributos = Auxiliar.RetornoAtributos<T>((T)Activator.CreateInstance(typeof(T)));

            var LObjeto = new List<T>();

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
                                LObjeto.Add(Auxiliar.RetornaDadosEntidade<T>(Dr));
                    }
                    catch { throw; }
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
        public virtual T SelectPelaPK(long Id)
        {
            var _entidade = (T)Activator.CreateInstance(typeof(T));
            var atributos = Auxiliar.RetornoAtributos<T>((T)Activator.CreateInstance(typeof(T)));

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
                            if (Dr.Read())
                                _entidade = Auxiliar.RetornaDadosEntidade<T>(Dr);
                    }
                    catch { throw; }
                }
            }

            return _entidade;
        }

        /// <summary>
        /// Retorna um datatable com o resultado da query informada no parametro
        /// </summary>
        /// <param name="query">Query que preencherá o datatable</param>
        /// <returns></returns>
        /// <remarks>Utilizar apenas para preencher Combobox nos User Controls</remarks>
        public virtual DataTable Select(string query)
        {
            var table = new DataTable();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand(query, Conn))
                {
                    var adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(table);
                }
            }
            return table;
        }

        /// <summary>
        /// Entidade a ser Removida do Banco de Dados
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <param name="Entidade">Nome da Entidade a ser Removido</param>
        public virtual void Remover(T Entidade)
        {
            var atributos = Auxiliar.RetornoAtributos<T>(Entidade);

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
                    catch { throw; }
                }
            }
        }

        /// <summary>
        /// Metodo para inserir/alterar registro no Banco de Dados
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <param name="Entidade">Nome da Entidade a ser inserido</param>
        public long Cadastro(T Entidade)
        {
            var atributos = Auxiliar.RetornoAtributos<T>(Entidade);
            if (Convert.ToInt64(Entidade.GetType().GetProperty(atributos.NomeChavePrimeria).GetValue(Entidade, null)) == 0)
                return Inserir(ref Entidade, atributos.NomeChavePrimeria, atributos.NomeProcedureInserir);
            else
            {
                Alterar(Entidade, atributos.NomeProcedureAlterar);
                return 0;
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
        private long Inserir(ref T Entidade, string ChavePrimaria, string NomeProcedure)
        {
            object result = 0;
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand(NomeProcedure, Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        var parametros = Auxiliar.GeraParametros<T>(Entidade);

                        foreach (MySqlParameter Param in parametros)
                            if (Param.ParameterName != "inId")
                                cmd.Parameters.Add(Param);

                        Conn.Open();
                        result = cmd.ExecuteScalar();
                        Entidade.GetType().GetProperty(ChavePrimaria).SetValue(Entidade, result.ToInt64(), null);
                    }
                    catch { throw; }
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
        private void Alterar(T Entidade, string NomeProcedure)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand(NomeProcedure, Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        foreach (MySqlParameter Param in Auxiliar.GeraParametros<T>(Entidade))
                            cmd.Parameters.Add(Param);

                        Conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch { throw; }
                }
            }
        }
    }
}
