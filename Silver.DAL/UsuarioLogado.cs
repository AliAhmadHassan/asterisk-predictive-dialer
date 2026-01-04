using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    /// <summary>
    /// Desenvolvido por: Francisco Silva
    ///             Data: 20/09/2013
    /// </summary>
    public class UsuarioLogado : Base<DTO.UsuarioLogado>
    {
        public virtual DTO.UsuarioLogado Obter(long ramal)
        {
            var LRetorno = new List<DTO.UsuarioLogado>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSusuarioLogadoPeloRamal", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inRamal", ramal);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.UsuarioLogado>(Dr));
                    
                    }
                    catch { throw new Exception("Erro ao consultar"); }
                    finally { }
                }
            }

            if (LRetorno.Count > 0)
                return LRetorno[0];
            else
                return null;
        }

        public void Atualizar(DTO.UsuarioLogado usuario_logado)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                Conn.Open();
                using (var cmd = new MySqlCommand("SPUusuarioLogado", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inRamal", usuario_logado.Ramal);
                        cmd.Parameters.AddWithValue("inUrl", usuario_logado.Url);
                        cmd.Parameters.AddWithValue("inContato", usuario_logado.Contato);
                        cmd.ExecuteNonQuery();
                    }
                    catch { }
                    finally { }
                }
            }
        }

        public void Cadastrar(DTO.UsuarioLogado usuario_logado)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                Conn.Open();
                using (var cmd = new MySqlCommand("SPIusuarioLogado", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inRamal", usuario_logado.Ramal);
                        cmd.Parameters.AddWithValue("inUrl", usuario_logado.Url);
                        cmd.Parameters.AddWithValue("inContato", usuario_logado.Contato);
                        cmd.ExecuteNonQuery();
                    }
                    catch { }
                    finally { }
                }
            }
        }
    }
}
