using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    public class Grupo : Base<DTO.Grupo>
    {
        /// <summary>
        /// Perquisa o Registro pelos Ativos
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <param name="Ativo">Valor</param>
        /// <returns>Retorna a Entidade Informada</returns>
        public virtual List<DTO.Grupo> Obter(bool ativo)
        {
            var LRetorno = new List<DTO.Grupo>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSgrupoAtivos", Conn))
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
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.Grupo>(Dr));
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
        /// Perquisa o Registro pela Parte do Nome o Grupo
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <param name="Id">Valor do Nome para Busca</param>
        /// <returns>Retorna a Entidade Informada</returns>
        public virtual List<DTO.Grupo> SelectPeloNome(string Id)
        {
            var LRetorno = new List<DTO.Grupo>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSgrupoPeloNome", Conn))
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
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.Grupo>(Dr));
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

        public List<DTO.Grupo> ObterGruposFilhos(long id_grupo)
        {
            List<DTO.Grupo> grupos_filhos = new List<DTO.Grupo>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                //Caso não exista um menu raiz, a aplicação dará um exceção.
                using (var cmd = new MySqlCommand("SPSgrupoPeloIdGrupo", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdGrupo", id_grupo);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                grupos_filhos.Add(Auxiliar.RetornaDadosEntidade<DTO.Grupo>(Dr));

                        List<DTO.Grupo> grupos_filhos_rec = new List<DTO.Grupo>();
                        if (grupos_filhos.Count > 0)
                            foreach (var item in grupos_filhos)
                                grupos_filhos_rec.AddRange(ObterGruposFilhos(item.Id));

                        grupos_filhos.AddRange(grupos_filhos_rec);
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
            return grupos_filhos;
        }
    }
}

