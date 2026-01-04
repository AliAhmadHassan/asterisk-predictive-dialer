using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    public class UsuarioPausa : Base<DTO.UsuarioPausa>
    {
        /// <summary>
        /// Perquisa o Registro pela Chave Estrangeira da Tabela
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <param name="Id">Valor da Chave Estrangeira</param>
        /// <returns>Retorna a Entidade Informada</returns>
        public virtual List<DTO.UsuarioPausa> SelectPeloUsuario(long Id)
        {
            var LUsuarioPausa = new List<DTO.UsuarioPausa>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSusuariopausaPeloUsuario", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdUsuario", Id);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                        {
                            while (Dr.Read())
                            {
                                LUsuarioPausa.Add(Auxiliar.RetornaDadosEntidade<DTO.UsuarioPausa>(Dr));
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

            return LUsuarioPausa;
        }

        /// <summary>
        /// Verifica se existe pausa dentro do periodo
        /// </summary>
        /// <param name="inicio"></param>
        /// <returns></returns>
        public virtual List<DTO.UsuarioPausa> SelectPeloInicio(DateTime dt_inicio, DateTime dt_fim)
        {
            var pausas = new List<DTO.UsuarioPausa>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSusuariopausaPeloInicio", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inInicio", dt_inicio);
                        cmd.Parameters.AddWithValue("inFim", dt_fim);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                pausas.Add(Auxiliar.RetornaDadosEntidade<DTO.UsuarioPausa>(Dr));
                    }
                    catch { throw; }
                    finally { }
                }
            }

            return pausas;
        }
    }
}
