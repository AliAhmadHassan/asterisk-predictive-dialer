using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 20/08/2014
    /// </summary>
    public class MensagemSistemaVisualizado : Base<DTO.MensagemSistemaVisualizado>
    {
        public bool ExisteVisualizacao(long id_mensagem, long id_usuario)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSMsgSisExisteVisualizacao", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdMensagem", id_mensagem);
                        cmd.Parameters.AddWithValue("inIdUsuario", id_usuario);
                        Conn.Open();
                        return (cmd.ExecuteScalar() != null);
                    }
                    catch { throw; }
                }
            }
        }
    }
}
