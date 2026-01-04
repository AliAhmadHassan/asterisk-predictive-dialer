using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Silver.DAL
{
    public class LogDiscador
    {
        public void Incluir(DTO.LogDiscador log)
        {
            using (MySqlConnection ctx = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStringSilver"].ConnectionString))
            {
                ctx.Open();
                string query = @"insert into logdiscador(IdCampanha, Evento, DataHora) values(@IdCampanha, @Evento, @DataHora) ";
                using (MySqlCommand cmd = new MySqlCommand(query, ctx))
                {
                    cmd.Parameters.AddWithValue("@IdCampanha", log.IdCampanha);
                    cmd.Parameters.AddWithValue("@Evento", log.Evento);
                    cmd.Parameters.AddWithValue("@DataHora", log.DataHora);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DTO.LogDiscador DiscadorEmExecucao(long id_campanha)
        {
            DTO.LogDiscador log_discador = null;
            using (MySqlConnection ctx = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStringSilver"].ConnectionString))
            {
                ctx.Open();
                string query = @"select * from logdiscador where IdCampanha = @IdCampanha order by Id desc limit 1";
                using (MySqlCommand cmd = new MySqlCommand(query, ctx))
                {
                    cmd.Parameters.AddWithValue("@IdCampanha", id_campanha);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        log_discador = new DTO.LogDiscador();
                        log_discador.DataHora = Convert.ToDateTime(reader["DataHora"].ToString());
                        log_discador.Evento = Convert.ToInt32(reader["Evento"].ToString());
                        log_discador.Id = Convert.ToInt32(reader["Id"].ToString());
                        log_discador.IdCampanha = Convert.ToInt32(reader["IdCampanha"].ToString());
                    }
                    reader.Close();
                }
            }
            return log_discador;
        }

        public List<DTO.LogDiscador> Listar(DateTime inicio, DateTime fim, int id_campanha)
        {
            List<DTO.LogDiscador> logs = new List<DTO.LogDiscador>();
            using (MySqlConnection ctx = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStringSilver"].ConnectionString))
            {
                ctx.Open();
                string query = @"select Id, IdCampanha, Evento, DataHora from logdiscador where DataHora between @Inicio and @Fim and @IdCampanha";

                using (MySqlCommand cmd = new MySqlCommand(query, ctx))
                {
                    cmd.Parameters.AddWithValue("@Inicio", new DateTime(inicio.Year, inicio.Month, inicio.Day, 0, 0, 0));
                    cmd.Parameters.AddWithValue("@Fim", new DateTime(fim.Year, fim.Month, fim.Day, 23, 59, 59));
                    cmd.Parameters.AddWithValue("@IdCampanha", id_campanha);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                        logs.Add(
                            new DTO.LogDiscador()
                            {
                                DataHora = Convert.ToDateTime(reader["DataHora"].ToString()),
                                Evento = Convert.ToInt32(reader["evento"].ToString()),
                                Id = Convert.ToInt32(reader["id"].ToString()),
                                IdCampanha = Convert.ToInt32(reader["IdCampanha"].ToString())
                            });
                }
            }

            return logs;
        }
    }
}
