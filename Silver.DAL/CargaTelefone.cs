using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    public class CargaTelefone : Base<DTO.CargaTelefone>
    {
        public virtual List<DTO.CargaTelefone> SelectPelaCarga(long Id)
        {
            var LRetorno = new List<DTO.CargaTelefone>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPScargatelefonePelaCarga", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inCarga", Id);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                        {
                            while (Dr.Read())
                            {
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.CargaTelefone>(Dr));
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

        public void AtualizarCargaAntiga(long id_campanha)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPUcargatelefoneCargaAntiga", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdCampanha", id_campanha);
                        Conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch { throw; }
                    finally { }
                }
            }
        }
    }
}
