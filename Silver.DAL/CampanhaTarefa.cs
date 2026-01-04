using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Silver.Common;

namespace Silver.DAL
{
    /// <summary>
    /// Desenvolvido por: Francisco Silva
    ///             Data: 27/06/2013
    ///OBS: Esta classe não se enquadrou no formato padrão de classes do sistema
    ///     por este motivo ela está implementada de forma diferente das demais
    /// </summary>
    public class CampanhaTarefa
    {
        /// <summary>
        /// Construtor Padrão
        /// </summary>
        public CampanhaTarefa() { }

        /// <summary>
        /// Remove da tabela os registros referente as tarefas
        /// da campanha
        /// </summary>
        /// <param name="id_campanha"></param>
        public void Remover(long id_campanha)
        {
            var query = string.Empty;

            query = @"delete 
                        from campanhatarefa 
                       where idcampanha = @idcampanha";

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand(query, Conn))
                {
                    cmd.Parameters.AddWithValue("@idcampanha", id_campanha);
                    Conn.Open();
                    cmd.ExecuteNonQuery();
                }
                Conn.Close();
            }
        }

        /// <summary>
        /// Inclui novas tarefas na base de dados
        /// </summary>
        /// <param name="tarefa">Tarefa que será incluída na base de dados</param>
        public void Incluir(DTO.CampanhaTarefa tarefa)
        {
            var query = string.Empty;

            query = @"insert into campanhatarefa(IdCampanha,        Ativo,  
                                                 SegundaInicio,     SegundaFim,         TercaInicio,            TercaFim, 
                                                 QuartaInicio,      QuartaFim,          QuintaInicio,           QuintaFim, 
                                                 SextaInicio,       SextaFim,           SabadoInicio,           SabadoFim,          
                                                 DomingoInicio,     DomingoFim)
                                         values(@IdCampanha,        @Ativo,  
                                                @SegundaInicio,     @SegundaFim,         @TercaInicio,            @TercaFim, 
                                                @QuartaInicio,      @QuartaFim,          @QuintaInicio,           @QuintaFim, 
                                                @SextaInicio,       @SextaFim,           @SabadoInicio,           @SabadoFim,
                                                @DomingoInicio,     @DomingoFim)";


            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand(query, Conn))
                {
                    cmd.Parameters.AddWithValue("@IdCampanha", tarefa.IdCampanha);
                    cmd.Parameters.AddWithValue("@Ativo", tarefa.Ativo);
                    cmd.Parameters.AddWithValue("@SegundaInicio", tarefa.SegundaInicio);
                    cmd.Parameters.AddWithValue("@SegundaFim", tarefa.SegundaFim);
                    cmd.Parameters.AddWithValue("@TercaInicio", tarefa.TercaInicio);
                    cmd.Parameters.AddWithValue("@TercaFim", tarefa.TercaFim);
                    cmd.Parameters.AddWithValue("@QuartaInicio", tarefa.QuartaInicio);
                    cmd.Parameters.AddWithValue("@QuartaFim", tarefa.QuartaFim);
                    cmd.Parameters.AddWithValue("@QuintaInicio", tarefa.QuintaInicio);
                    cmd.Parameters.AddWithValue("@QuintaFim", tarefa.QuintaFim);
                    cmd.Parameters.AddWithValue("@SextaInicio", tarefa.SextaInicio);
                    cmd.Parameters.AddWithValue("@SextaFim", tarefa.SextaFim);
                    cmd.Parameters.AddWithValue("@SabadoInicio", tarefa.SabadoInicio);
                    cmd.Parameters.AddWithValue("@SabadoFim", tarefa.SabadoFim);
                    cmd.Parameters.AddWithValue("@DomingoInicio", tarefa.DomingoInicio);
                    cmd.Parameters.AddWithValue("@DomingoFim", tarefa.DomingoFim);
                    Conn.Open();
                    cmd.ExecuteNonQuery();
                }
                Conn.Close();
            }
        }

        /// <summary>
        /// Consulta uma tarefa de uma determinada campanha
        /// </summary>
        /// <param name="id_campanha">Id da campanha que se deseja conhecer as tarefas</param>
        /// <returns></returns>
        public DTO.CampanhaTarefa Obter(long id_campanha)
        {
            var tarefa = new DTO.CampanhaTarefa();
            var query = string.Empty;

            query = @"select IdCampanha,        Ativo,  
                             SegundaInicio,     SegundaFim,         TercaInicio,            TercaFim, 
                             QuartaInicio,      QuartaFim,          QuintaInicio,           QuintaFim, 
                             SextaInicio,       SextaFim,           SabadoInicio,           SabadoFim,          
                             DomingoInicio,     DomingoFim
                        from campanhatarefa
                       where IdCampanha = @IdCampanha";

            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                Conn.Open();
                using (var cmd = new MySqlCommand(query, Conn))
                {
                    cmd.Parameters.AddWithValue("@IdCampanha", id_campanha);
                    MySqlDataReader reader = null;
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        tarefa.IdCampanha = reader["IdCampanha"].ToInt64();
                        tarefa.Ativo = Convert.ToBoolean(reader["Ativo"]);

                        tarefa.SegundaInicio = reader["SegundaInicio"] != null ? (DateTime?)reader["SegundaInicio"].ToString().ToDateTime() : null;
                        tarefa.SegundaFim = reader["SegundaInicio"] != null ? (DateTime?)reader["SegundaFim"].ToString().ToDateTime() : null;

                        tarefa.TercaInicio = reader["TercaInicio"] != null ? (DateTime?)reader["TercaInicio"].ToString().ToDateTime() : null;
                        tarefa.TercaFim = reader["TercaFim"] != null ? (DateTime?)reader["TercaFim"].ToString().ToDateTime() : null;

                        tarefa.QuartaInicio = reader["QuartaInicio"] != null ? (DateTime?)reader["QuartaInicio"].ToString().ToDateTime() : null;
                        tarefa.QuartaFim = reader["QuartaFim"] != null ? (DateTime?)reader["QuartaFim"].ToString().ToDateTime() : null;

                        tarefa.QuintaInicio = reader["QuintaInicio"] != null ? (DateTime?)reader["QuintaInicio"].ToString().ToDateTime() : null;
                        tarefa.QuintaFim = reader["QuintaFim"] != null ? (DateTime?)reader["QuintaFim"].ToString().ToDateTime() : null;

                        tarefa.SextaInicio = reader["SextaInicio"] != null ? (DateTime?)reader["SextaInicio"].ToString().ToDateTime() : null;
                        tarefa.SextaFim = reader["SextaFim"] != null ? (DateTime?)reader["SextaFim"].ToString().ToDateTime() : null;

                        tarefa.SabadoInicio = reader["SabadoInicio"] != null ? (DateTime?)reader["SabadoInicio"].ToString().ToDateTime() : null;
                        tarefa.SabadoFim = reader["SabadoFim"] != null ? (DateTime?)reader["SabadoFim"].ToString().ToDateTime() : null;

                        tarefa.DomingoInicio = reader["DomingoInicio"] != null ? (DateTime?)reader["DomingoInicio"].ToString().ToDateTime() : null;
                        tarefa.DomingoFim = reader["DomingoFim"] != null ? (DateTime?)reader["DomingoFim"].ToString().ToDateTime() : null;

                    }
                    reader.Close();
                }
                Conn.Close();
            }
            return tarefa;
        }
    }
}
