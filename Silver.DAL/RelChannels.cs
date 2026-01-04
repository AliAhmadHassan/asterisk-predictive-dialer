using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 23/06/2014
    /// </summary>
    public class RelChannels
    {
        public void Inserir(string dgv_show_channel)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPIRelCanal", Conn))
                {
                    Conn.Open();
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inDGVShowChannels", dgv_show_channel);
                        cmd.Parameters.AddWithValue("inAtualizado", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                    catch { throw; }
                }
            }
        }

        public string UltimoStatus()
        {
            string status_canais = string.Empty;
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                Conn.Open();
                using (var cmd = new MySqlCommand("SPSRelCanalUltimo", Conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlDataReader reader = null;

                    try
                    {
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                            status_canais = reader["DGVShowChannels"].ToString();
                    }
                    catch { throw; }
                    finally { if (reader != null) reader.Close(); }
                }
            }
            return status_canais;
        }
    }
}
