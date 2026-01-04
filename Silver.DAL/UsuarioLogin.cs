using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    public class UsuarioLogin : Base<DTO.UsuarioLogin>
    {
        /// <summary>
        /// Perquisa o Registro pela Chave Estrangeira da Tabela
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade</typeparam>
        /// <param name="Id">Valor da Chave Estrangeira</param>
        /// <returns>Retorna a Entidade Informada</returns>
        public virtual List<DTO.UsuarioLogin> SelectPeloUsuario(long Id)
        {
            var LUsuarioLogin = new List<DTO.UsuarioLogin>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSusuariologinPeloUsuario", Conn))
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
                                LUsuarioLogin.Add(Auxiliar.RetornaDadosEntidade<DTO.UsuarioLogin>(Dr));
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

            return LUsuarioLogin;
        }
    }
}
