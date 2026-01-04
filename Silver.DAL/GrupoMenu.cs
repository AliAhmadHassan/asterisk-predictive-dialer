using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    public class GrupoMenu
    {
        public DTO.GrupoMenu Select(DTO.GrupoMenu grupoMenu)
        {
            DTO.GrupoMenu cp = null;

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSgrupomenuPorAmbos", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdMenu", grupoMenu.IdMenu);
                        cmd.Parameters.AddWithValue("inIdGrupo", grupoMenu.IdGrupo);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                        {
                            if (Dr.Read())
                            {
                                cp = Auxiliar.RetornaDadosEntidade<DTO.GrupoMenu>(Dr);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception("Erro ao consultar");
                    }
                }
            }

            return cp;
        }

        public List<DTO.GrupoMenu> SelectPeloGrupo(DTO.GrupoMenu grupoMenu)
        {
            var LGrupoMenu = new List<DTO.GrupoMenu>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSgrupomenuPeloGrupo", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdGrupo", grupoMenu.IdGrupo);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                        {
                            while (Dr.Read())
                            {
                                LGrupoMenu.Add(Auxiliar.RetornaDadosEntidade<DTO.GrupoMenu>(Dr));
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception("Erro ao consultar");
                    }
                }
            }

            return LGrupoMenu;
        }

        public List<DTO.GrupoMenu> SelectPeloMenu(DTO.GrupoMenu grupoMenu)
        {
            var LGrupoMenu = new List<DTO.GrupoMenu>();

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSgrupomenuPeloMenu", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdMenu", grupoMenu.IdMenu);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                        {
                            while (Dr.Read())
                            {
                                LGrupoMenu.Add(Auxiliar.RetornaDadosEntidade<DTO.GrupoMenu>(Dr));
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception("Erro ao consultar");
                    }
                }
            }

            return LGrupoMenu;
        }

        public void Remover(DTO.GrupoMenu grupoMenu)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPDgrupomenu", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdGrupo", grupoMenu.IdGrupo);
                        cmd.Parameters.AddWithValue("inIdMenu", grupoMenu.IdMenu);
                        Conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw new Exception("Erro ao deletar o registro");
                    }
                }
            }
        }

        public void RemoverPeloGrupo(DTO.GrupoMenu grupoMenu)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPDgrupomenuPeloGrupo", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdGrupo", grupoMenu.IdGrupo);
                        Conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw new Exception("Erro ao deletar o registro");
                    }
                }
            }
        }

        public void RemoverPeloMenu(DTO.GrupoMenu grupoMenu)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPDgrupomenuPeloMenu", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdMenu", grupoMenu.IdMenu);
                        Conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw new Exception("Erro ao deletar o registro");
                    }
                }
            }
        }

        public void Inserir(DTO.GrupoMenu grupoMenu)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPIgrupomenu", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdGrupo", grupoMenu.IdGrupo);
                        cmd.Parameters.AddWithValue("inIdMenu", grupoMenu.IdMenu);
                        Conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw new Exception("Erro ao inserir registro.");
                    }
                    finally
                    {
                    }
                }
            }
        }
    }
}
