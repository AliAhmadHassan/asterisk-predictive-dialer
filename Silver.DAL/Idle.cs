using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 21/06/2014
    /// </summary>
    public class Idle : Base<DTO.Idle>
    {
        public virtual List<DTO.Idle> SelectPeloRamal(long ramal)
        {
            var LRetorno = new List<DTO.Idle>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSIdlePeloRamal", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inRamal", ramal);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.Idle>(Dr));
                    }
                    catch { throw; }
                }
            }

            return LRetorno;
        }

        public decimal ObterIdle(long[] id_historico)
        {
            decimal idle = 0;
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSIdlePeloHistorico", Conn))
                {
                    try
                    {
                        string ids_historico = string.Empty;
                        foreach (var id in id_historico)
                            ids_historico += id + ", ";

                        ids_historico = ids_historico.Remove(ids_historico.LastIndexOf(','), 1);

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdHistorico", ids_historico);

                        Conn.Open();
                        object retultado = cmd.ExecuteScalar();
                        idle = Convert.ToDecimal(retultado);
                    }
                    catch { throw; }
                }
            }

            return idle;
        }
    }
}
