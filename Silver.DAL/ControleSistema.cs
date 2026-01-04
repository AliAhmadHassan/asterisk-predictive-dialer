using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Silver.DAL
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 26/06/2013    
    /// </summary>
    public class ControleSistema
    {
        /// <summary>
        /// Lista todas as solicitações de eventos para o sistema
        /// </summary>
        /// <returns></returns>
        public List<DTO.ControleSistema> ListarControles(long? id_usuario)
        {
            var LRetorno = new List<DTO.ControleSistema>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                string and = string.Empty;
                if (id_usuario != null)
                    and = " and solicitante = " + id_usuario;

                string query = string.Format(@"select Id, Evento, Valor, Situacao, DtHrExecucao, Solicitante, Campanha
                                                                    from controlesistema 
                                                                   where DtHrExecucao is null {0} order by id desc limit 100", and);

                using (var cmd = new MySqlCommand(query, Conn))
                {
                    try
                    {
                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.ControleSistema>(Dr));
                    }
                    catch
                    {
                        throw new Exception("Erro ao consultar");
                    }
                }
            }
            return LRetorno;
        }

        /// <summary>
        /// Listar controles pela campanha
        /// </summary>
        /// <param name="id_campanha"></param>
        /// <returns></returns>
        public List<DTO.ControleSistema> ListarPelaCampanha(long id_campanha)
        {
            var LRetorno = new List<DTO.ControleSistema>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                string and = " and campanha = " + id_campanha;
                string query = string.Format(@"select Id, Evento, Valor, Situacao, DtHrExecucao, Solicitante, Campanha
                                                                    from controlesistema 
                                                                   where DtHrExecucao is null {0} order by id desc limit 100", and);

                using (var cmd = new MySqlCommand(query, Conn))
                {
                    try
                    {
                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.ControleSistema>(Dr));
                    }
                    catch { throw; }
                }
            }
            return LRetorno;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_controle"></param>
        public void AtualizarHoraExecucao(int id_controle)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("update controlesistema set DtHrExecucao = @DtHrExecucao where id= @Id", Conn))
                {
                    Conn.Open();
                    cmd.Parameters.AddWithValue("@DtHrExecucao", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Id", id_controle);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Inclui um evento na tabela de controle
        /// </summary>
        /// <param name="evento">Evento solicitado</param>
        /// <param name="valor">Valor para o evento</param>
        public void IncluirEvento(DTO.EventoControleSistema evento, DTO.ControleSistema controle)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("insert into controlesistema(evento, valor, situacao, solicitante, campanha, porcentagem) values(@evento, @valor, 1, @solicitante, @idcampanha, @porcentagem)", Conn))
                {
                    try
                    {
                        Conn.Open();
                        cmd.Parameters.AddWithValue("@evento", Enum.GetName(typeof(DTO.EventoControleSistema), evento));
                        cmd.Parameters.AddWithValue("@valor", controle.Valor);
                        cmd.Parameters.AddWithValue("@solicitante", controle.Solicitante);
                        cmd.Parameters.AddWithValue("@idcampanha", controle.Campanha);
                        cmd.Parameters.AddWithValue("@porcentagem", 0);
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw new Exception("Erro ao consultar");
                    }
                }
            }
        }

        /// <summary>
        /// Delete todos os registros da tabela de controle do sistema
        /// OBS: Utilizar este método apenas quando todas as solicitações de eventos forem executadas com sucesso
        /// </summary>
        public void LimparEventos()
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("delete from controlesistema", Conn))
                {
                    try
                    {
                        Conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw new Exception("Erro ao consultar");
                    }
                }
            }
        }

        public List<DTO.ControleSistema> ListarTodosControles(long? id_usuario)
        {
            var LRetorno = new List<DTO.ControleSistema>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                string and = string.Empty;
                if (id_usuario != null)
                    and = " and solicitante = " + id_usuario;

                string query = string.Format(@"select Id, Evento, Valor, Situacao, DtHrExecucao, Solicitante, Campanha, Porcentagem
                                                                    from controlesistema 
                                                                   where id != 0 {0} order by Id desc Limit 50", and);

                using (var cmd = new MySqlCommand(query, Conn))
                {
                    try
                    {
                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.ControleSistema>(Dr));
                    }
                    catch
                    {
                        throw new Exception("Erro ao consultar");
                    }
                }
            }
            return LRetorno;
        }

        public void AtualizarStatusExecucao(DTO.SitucaoEventoControleSistema status, long id_controle_sistema)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("update controlesistema set Situacao = @Situacao where id= @Id", Conn))
                {
                    Conn.Open();
                    cmd.Parameters.AddWithValue("@Situacao", (int)status);
                    cmd.Parameters.AddWithValue("@Id", id_controle_sistema);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AtualizarPorcentagem(long id_controle_sistema, decimal porcentagem)
        {
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("Update controlesistema set porcentagem = @porcentagem where id = @id", Conn))
                {
                    try
                    {
                        Conn.Open();
                        cmd.Parameters.AddWithValue("@porcentagem", porcentagem);
                        cmd.Parameters.AddWithValue("@id", id_controle_sistema);
                        cmd.ExecuteNonQuery();
                    }
                    catch { throw; }
                }
            }
        }

        public DTO.ControleSistema ObterControleSistema(long id_campanha)
        {
            var LRetorno = new DTO.ControleSistema();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                string query = string.Format(@"select Id, Evento, Valor, Situacao, DtHrExecucao, Solicitante, Campanha, Porcentagem
                                                 from controlesistema 
                                                where id=@id");

                using (var cmd = new MySqlCommand(query, Conn))
                {
                    try
                    {
                        Conn.Open();
                        cmd.Parameters.AddWithValue("@id", id_campanha);
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno = Auxiliar.RetornaDadosEntidade<DTO.ControleSistema>(Dr);
                    }
                    catch { throw; }
                }
            }
            return LRetorno;
        }

        private DTO.ControleSistema ObterUltimo(long id_campanha, DTO.EventoControleSistema evento_sistema)
        {
            var LRetorno = new List<DTO.ControleSistema>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                string and = " and campanha = " + id_campanha;
                string query = string.Format(@"select Id, Evento, Valor, Situacao, DtHrExecucao, Solicitante, Campanha, Porcentagem
                                                                    from controlesistema 
                                                                   where Evento = '{0}' 
                                                                     {1} order by id desc limit 1", Enum.GetName(typeof(DTO.EventoControleSistema), evento_sistema), and);

                using (var cmd = new MySqlCommand(query, Conn))
                {
                    try
                    {
                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                LRetorno.Add(Auxiliar.RetornaDadosEntidade<DTO.ControleSistema>(Dr));
                    }
                    catch { throw; }
                }
            }

            if (LRetorno.Count > 0)
                return LRetorno[0];
            else
                return null;
        }

        public DTO.ControleSistema ObterUltimaCarga(long id_campanha)
        {
            return ObterUltimo(id_campanha, DTO.EventoControleSistema.Processar_Carga);
        }

        public DTO.ControleSistema ObterUltimoMailing(long id_campanha)
        {
            return ObterUltimo(id_campanha, DTO.EventoControleSistema.Iniciar_Campanha);
        }
    }
}
