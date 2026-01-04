using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    /// <summary>
    /// Desnvolvida por: Francisco Silva
    ///            Data: 03/07/2014
    /// </summary>
    public class MensagemSistema : Base<DTO.MensagemSistema>
    {
        private List<DTO.MensagemSistema> Listar(long id_campanha, bool visualizada)
        {
            var LRetorno = new List<DTO.MensagemSistema>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSMensagemSistemaPelaCampanha", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdCampanha", id_campanha);
                        cmd.Parameters.AddWithValue("inVisualizada", visualizada);
                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.MensagemSistema>(Dr));
                    }
                    catch { throw; }
                }
            }
            return LRetorno;
        }

        public List<DTO.MensagemSistema> ListarVisualizadas(long id_campanha)
        {
            return Listar(id_campanha, true);
        }

        public List<DTO.MensagemSistema> ListarNaoVisualizadas(long id_campanha)
        {
            return Listar(id_campanha, false);
        }

        public void MarcarComoVisualizada(long id_mensagem)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPUMensagemSistemaVisualizadoUsuario", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdMensagem", id_mensagem);
                        cmd.Parameters.AddWithValue("inDataHoraVisualizacao", DateTime.Now);
                        Conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch { throw; }
                }
            }
        }

        public void MarcarComoVisualizada(long id_usuario, long id_mensagem)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPUMensagemSistemaVisualizadoUsuario", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdUsuario", id_usuario);
                        cmd.Parameters.AddWithValue("inDataHoraVisualizacao", DateTime.Now);
                        cmd.Parameters.AddWithValue("inIdMensagem", id_mensagem);
                        Conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch { throw; }
                }
            }
        }

        public bool ExisteMensagemNaoVisualizada(long id_campanha, Silver.DTO.TipoMensagemSistema tipo_mensagem)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSMensagemSistemaPelaCampanha", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdCampanha", id_campanha);
                        cmd.Parameters.AddWithValue("inVisualizada", false);
                        cmd.Parameters.AddWithValue("inTipoMensagem", (int)tipo_mensagem);
                        Conn.Open();

                        return (cmd.ExecuteScalar() != null);
                    }
                    catch { throw; }
                }
            }
        }
    }
}
