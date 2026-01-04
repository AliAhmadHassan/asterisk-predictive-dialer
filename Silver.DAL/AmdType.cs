using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    public class AmdType : Base<DTO.AmdType>
    {
        public virtual DTO.AmdType Obter(string descricao)
        {
            var AmdType = new DTO.AmdType();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSamdtypePelaDescricao", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inDescricao", descricao);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                        {
                            while (Dr.Read())
                            {
                                AmdType = Auxiliar.RetornaDadosEntidade<DTO.AmdType>(Dr);
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

            return AmdType;
        }
    }
}
