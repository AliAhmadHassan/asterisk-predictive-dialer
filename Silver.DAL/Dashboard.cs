using System;
using System.Data;
using MySql.Data.MySqlClient;
using Silver.DTO;
using System.Collections.Generic;
using Silver.Common;

namespace Silver.DAL
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 13/08/2013
    /// </summary>
    public class Dashboard
    {

        public decimal TMA(params long[] id_historico)
        {
            decimal tma = 0;
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSTMAPeloHistorico", Conn))
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
                        tma = Convert.ToDecimal(retultado);
                    }
                    catch { throw; }
                }
            }

            return tma;
        }

        private string GetString(params long[] lista_generica)
        {
            var filtro_query = "";
            foreach (long id in lista_generica)
                filtro_query += "'" + id.ToString() + "', ";

            filtro_query = filtro_query.Remove(filtro_query.LastIndexOf(','), 1);
            return filtro_query;
        }

        public DataTable Cargas(long id_historico)
        {
            var tabela_retorno = new DataTable("tabela_retorno");
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSdashboardcarga", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdHistorico", id_historico);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                            adapter.Fill(tabela_retorno);
                    }
                    catch { throw; }
                    finally { }
                }
            }

            return tabela_retorno;
        }

        public DataTable Campanhas(long id_historico)
        {
            var tabela_retorno = new DataTable("tabela_retorno");
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSdashboardcampanha", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdHistorico", id_historico);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                            adapter.Fill(tabela_retorno);
                    }
                    catch { throw; }
                    finally { }
                }
            }

            return tabela_retorno;
        }

        public DataTable Resultados(long id_historico)
        {
            var tabela_retorno = new DataTable("tabela_retorno");
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSdashboardresultados", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdHistorico", id_historico);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                            adapter.Fill(tabela_retorno);
                    }
                    catch { throw; }
                    finally { }
                }
            }

            return tabela_retorno;
        }

        public DataTable Discagem(long id_historico)
        {
            var tabela_retorno = new DataTable("tabela_retorno");
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSdashboardDiscagem", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdHistorico", id_historico);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                            adapter.Fill(tabela_retorno);
                    }
                    catch { throw; }
                    finally { }
                }
            }

            return tabela_retorno;
        }

        /// <summary>
        /// Utilizado para TMA e TME
        /// </summary>
        /// <param name="id_historico"></param>
        /// <returns></returns>
        public List<DTO.LogRamal> LogRamal(long id_historico)
        {
            var retorno = new List<DTO.LogRamal>();
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSlogramalIdHistorico", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdHistorico", id_historico);

                        Conn.Open();
                        using (var Dr = cmd.ExecuteReader())
                            while (Dr.Read())
                                retorno.Add(Auxiliar.RetornaDadosEntidade<DTO.LogRamal>(Dr));
                    }
                    catch { throw; }
                }
            }

            return retorno;
        }

        public DataTable RangeLinhaTempo(long id_historico)
        {
            var tabela_retorno = new DataTable("tabela_retorno");
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSdashboardLinhaTempoRange", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdHistorico", id_historico);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                            adapter.Fill(tabela_retorno);
                    }
                    catch { throw; }
                    finally { }
                }
            }

            return tabela_retorno;
        }

        public DataTable LinhaTempo(long id_historico, DTO.SilverStatus status)
        {
            var tabela_retorno = new DataTable("tabela_retorno");
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSdashboardLinhaTempo", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdHistorico", id_historico);
                        cmd.Parameters.AddWithValue("inStatusTelefone", (int)status);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                            adapter.Fill(tabela_retorno);
                    }
                    catch { throw; }
                    finally { }
                }
            }

            return tabela_retorno;
        }

        #region Versão 1.0.0

        public DataTable ListarCarga(int id_campanha, List<long> ids_historico)
        {
            var retorno = new DataTable("retorno");
            using (var Conn = new MySqlConnection(Conexao.strConn))
            {
                using (var cmd = new MySqlCommand("SPSdashboardcarga", Conn))
                {
                    try
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("inIdCampanha", id_campanha);
                        cmd.Parameters.AddWithValue("inIdHistorico", GetString(ids_historico.ToArray()));

                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        adapter.Fill(retorno);


                    }
                    catch { }
                    finally { }
                }
            }
            return retorno;
        }

        public DataTable ListarCampanhas(int id_campanha)
        {
            using (var conn = new MySqlConnection(Conexao.strConn))
            {
                var query = string.Empty;
                query = @"select count(cargatelefone.status) as quantidade, cargatelefone.status
                            from campanha , carga, cargatelefone 
                           where campanha.id 	= carga.idcampanha
                             and carga.id		= cargatelefone.idcarga
                             and campanha.id    = @idcampanha
                           group by cargatelefone.status, cargatelefone.status, campanha.nome
                           order by nome";

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idcampanha", id_campanha);
                var adapter = new MySqlDataAdapter(cmd);

                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public DataTable ListarTelefones(int id_campanha, SilverStatus status)
        {
            using (var conn = new MySqlConnection(Conexao.strConn))
            {
                var query = string.Empty;
                query = @"select count(cargatelefone.status) as {0}, cargatelefone.status, campanha.nome
                            from campanha , carga, cargatelefone 
                           where campanha.id 	= carga.idcampanha
                             and carga.id		= cargatelefone.idcarga
                             and campanha.id    = @idcampanha";

                if (status != SilverStatus.Todos)
                    query += "  and cargatelefone.status = @status ";

                query += @" group by cargatelefone.status, cargatelefone.status, campanha.nome
                           order by nome";

                query = string.Format(query, Enum.GetName(typeof(SilverStatus), status));

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idcampanha", id_campanha);
                cmd.Parameters.AddWithValue("@status", status);

                var adapter = new MySqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public DataTable ListarDiscagemOperador(string nome_campanha)
        {
            using (var conn = new MySqlConnection(Conexao.strConn))
            {
                var query = string.Empty;
                query = @"select mid(dstchannel,5,4) as ramal, count(dstchannel) as total, lastdata as campanha
                            from bilhetagem
                           where CHAR_LENGTH(dstchannel) > 0
                             and lastdata = @nome_campanha
                           group by ramal";

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nome_campanha", nome_campanha);
                var adapter = new MySqlDataAdapter(cmd);

                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public DataTable ListarRangeLinhaTempo(params string[] campanhas)
        {
            using (var conn = new MySqlConnection(Conexao.strConn))
            {
                string campanhas_formatada = string.Empty;
                foreach (var c in campanhas)
                    campanhas_formatada += string.Format("'{0}',", c);

                campanhas_formatada = campanhas_formatada.Remove(campanhas_formatada.LastIndexOf(','), 1);

                var query = string.Empty;
                query = string.Format(@"select 
                            case
                                when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                                when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                                when time(calldate) between '07:00:00' and '07:29:59' then '07:00'
                                when time(calldate) between '07:30:00' and '07:59:59' then '07:30'
                                when time(calldate) between '08:00:00' and '08:29:59' then '08:00'
                                when time(calldate) between '08:30:00' and '08:59:59' then '08:30'
                                when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                                when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                                when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                                when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                                when time(calldate) between '10:00:00' and '10:29:59' then '10:00'
                                when time(calldate) between '10:30:00' and '10:59:59' then '10:30'
                                when time(calldate) between '11:00:00' and '11:29:59' then '11:00'
                                when time(calldate) between '11:30:00' and '11:59:59' then '11:30'
                                when time(calldate) between '12:00:00' and '12:29:59' then '12:00'
                                when time(calldate) between '12:30:00' and '12:59:59' then '12:30'
                                when time(calldate) between '13:00:00' and '13:29:59' then '13:00'
                                when time(calldate) between '13:30:00' and '13:59:59' then '13:30'
                                when time(calldate) between '14:00:00' and '14:29:59' then '14:00'
                                when time(calldate) between '14:30:00' and '14:59:59' then '14:30'
                                when time(calldate) between '15:00:00' and '15:29:59' then '15:00'
                                when time(calldate) between '15:30:00' and '15:59:59' then '15:30'
                                when time(calldate) between '16:00:00' and '16:29:59' then '16:00'
                                when time(calldate) between '16:30:00' and '16:59:59' then '16:30'
                                when time(calldate) between '17:00:00' and '17:29:59' then '17:00'
                                when time(calldate) between '17:30:00' and '17:59:59' then '17:30'
                                when time(calldate) between '18:00:00' and '18:29:59' then '18:00'
                                when time(calldate) between '18:30:00' and '18:59:59' then '18:30'
                                when time(calldate) between '19:00:00' and '19:29:59' then '19:00'
                                when time(calldate) between '19:30:00' and '19:59:59' then '19:30'
                                when time(calldate) between '20:00:00' and '20:29:59' then '20:00'
                                when time(calldate) between '20:30:00' and '20:59:59' then '20:30'
                                when time(calldate) between '21:00:00' and '21:29:59' then '21:00'
                                when time(calldate) between '21:30:00' and '21:59:59' then '21:30'
                                when time(calldate) between '22:00:00' and '22:29:59' then '22:00'
                                when time(calldate) between '22:30:00' and '22:59:59' then '22:30'
                                when time(calldate) between '23:00:00' and '23:29:59' then '23:00'
                                when time(calldate) between '23:30:00' and '23:59:59' then '23:30'
                            end as Hora,
                            count(clid) as Total
                        from
                            bilhetagem
                        where lastdata in ({0})
                        group by case
                            when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                            when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                            when time(calldate) between '07:00:00' and '07:29:59' then '07:00'
                            when time(calldate) between '07:30:00' and '07:59:59' then '07:30'
                            when time(calldate) between '08:00:00' and '08:29:59' then '08:00'
                            when time(calldate) between '08:30:00' and '08:59:59' then '08:30'
                            when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                            when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                            when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                            when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                            when time(calldate) between '10:00:00' and '10:29:59' then '10:00'
                            when time(calldate) between '10:30:00' and '10:59:59' then '10:30'
                            when time(calldate) between '11:00:00' and '11:29:59' then '11:00'
                            when time(calldate) between '11:30:00' and '11:59:59' then '11:30'
                            when time(calldate) between '12:00:00' and '12:29:59' then '12:00'
                            when time(calldate) between '12:30:00' and '12:59:59' then '12:30'
                            when time(calldate) between '13:00:00' and '13:29:59' then '13:00'
                            when time(calldate) between '13:30:00' and '13:59:59' then '13:30'
                            when time(calldate) between '14:00:00' and '14:29:59' then '14:00'
                            when time(calldate) between '14:30:00' and '14:59:59' then '14:30'
                            when time(calldate) between '15:00:00' and '15:29:59' then '15:00'
                            when time(calldate) between '15:30:00' and '15:59:59' then '15:30'
                            when time(calldate) between '16:00:00' and '16:29:59' then '16:00'
                            when time(calldate) between '16:30:00' and '16:59:59' then '16:30'
                            when time(calldate) between '17:00:00' and '17:29:59' then '17:00'
                            when time(calldate) between '17:30:00' and '17:59:59' then '17:30'
                            when time(calldate) between '18:00:00' and '18:29:59' then '18:00'
                            when time(calldate) between '18:30:00' and '18:59:59' then '18:30'
                            when time(calldate) between '19:00:00' and '19:29:59' then '19:00'
                            when time(calldate) between '19:30:00' and '19:59:59' then '19:30'
                            when time(calldate) between '20:00:00' and '20:29:59' then '20:00'
                            when time(calldate) between '20:30:00' and '20:59:59' then '20:30'
                            when time(calldate) between '21:00:00' and '21:29:59' then '21:00'
                            when time(calldate) between '21:30:00' and '21:59:59' then '21:30'
                            when time(calldate) between '22:00:00' and '22:29:59' then '22:00'
                            when time(calldate) between '22:30:00' and '22:59:59' then '22:30'
                            when time(calldate) between '23:00:00' and '23:29:59' then '23:00'
                            when time(calldate) between '23:30:00' and '23:59:59' then '23:30'
                        end , disposition", campanhas_formatada);

                var cmd = new MySqlCommand(query, conn);
                var adapter = new MySqlDataAdapter(cmd);

                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public DataTable ListarAtendidosLinhaTempo(long TipoTelefone, params string[] campanhas)
        {
            using (var conn = new MySqlConnection(Conexao.strConn))
            {
                string campanhas_formatada = string.Empty;
                foreach (var c in campanhas)
                    campanhas_formatada += string.Format("'{0}',", c);

                campanhas_formatada = campanhas_formatada.Remove(campanhas_formatada.LastIndexOf(','), 1);

                var query = string.Empty;
                query = string.Format(@"select 
                            case
                                when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                                when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                                when time(calldate) between '07:00:00' and '07:29:59' then '07:00'
                                when time(calldate) between '07:30:00' and '07:59:59' then '07:30'
                                when time(calldate) between '08:00:00' and '08:29:59' then '08:00'
                                when time(calldate) between '08:30:00' and '08:59:59' then '08:30'
                                when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                                when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                                when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                                when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                                when time(calldate) between '10:00:00' and '10:29:59' then '10:00'
                                when time(calldate) between '10:30:00' and '10:59:59' then '10:30'
                                when time(calldate) between '11:00:00' and '11:29:59' then '11:00'
                                when time(calldate) between '11:30:00' and '11:59:59' then '11:30'
                                when time(calldate) between '12:00:00' and '12:29:59' then '12:00'
                                when time(calldate) between '12:30:00' and '12:59:59' then '12:30'
                                when time(calldate) between '13:00:00' and '13:29:59' then '13:00'
                                when time(calldate) between '13:30:00' and '13:59:59' then '13:30'
                                when time(calldate) between '14:00:00' and '14:29:59' then '14:00'
                                when time(calldate) between '14:30:00' and '14:59:59' then '14:30'
                                when time(calldate) between '15:00:00' and '15:29:59' then '15:00'
                                when time(calldate) between '15:30:00' and '15:59:59' then '15:30'
                                when time(calldate) between '16:00:00' and '16:29:59' then '16:00'
                                when time(calldate) between '16:30:00' and '16:59:59' then '16:30'
                                when time(calldate) between '17:00:00' and '17:29:59' then '17:00'
                                when time(calldate) between '17:30:00' and '17:59:59' then '17:30'
                                when time(calldate) between '18:00:00' and '18:29:59' then '18:00'
                                when time(calldate) between '18:30:00' and '18:59:59' then '18:30'
                                when time(calldate) between '19:00:00' and '19:29:59' then '19:00'
                                when time(calldate) between '19:30:00' and '19:59:59' then '19:30'
                                when time(calldate) between '20:00:00' and '20:29:59' then '20:00'
                                when time(calldate) between '20:30:00' and '20:59:59' then '20:30'
                                when time(calldate) between '21:00:00' and '21:29:59' then '21:00'
                                when time(calldate) between '21:30:00' and '21:59:59' then '21:30'
                                when time(calldate) between '22:00:00' and '22:29:59' then '22:00'
                                when time(calldate) between '22:30:00' and '22:59:59' then '22:30'
                                when time(calldate) between '23:00:00' and '23:29:59' then '23:00'
                                when time(calldate) between '23:30:00' and '23:59:59' then '23:30'
                            end as Hora,
                            count(clid) as Atendido
                        from
                            bilhetagem
                        where lastdata in ({0})
                             and disposition = 'ANSWERED'
                             and tipotelefone = @TipoTelefone
                        group by case
                            when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                            when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                            when time(calldate) between '07:00:00' and '07:29:59' then '07:00'
                            when time(calldate) between '07:30:00' and '07:59:59' then '07:30'
                            when time(calldate) between '08:00:00' and '08:29:59' then '08:00'
                            when time(calldate) between '08:30:00' and '08:59:59' then '08:30'
                            when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                            when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                            when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                            when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                            when time(calldate) between '10:00:00' and '10:29:59' then '10:00'
                            when time(calldate) between '10:30:00' and '10:59:59' then '10:30'
                            when time(calldate) between '11:00:00' and '11:29:59' then '11:00'
                            when time(calldate) between '11:30:00' and '11:59:59' then '11:30'
                            when time(calldate) between '12:00:00' and '12:29:59' then '12:00'
                            when time(calldate) between '12:30:00' and '12:59:59' then '12:30'
                            when time(calldate) between '13:00:00' and '13:29:59' then '13:00'
                            when time(calldate) between '13:30:00' and '13:59:59' then '13:30'
                            when time(calldate) between '14:00:00' and '14:29:59' then '14:00'
                            when time(calldate) between '14:30:00' and '14:59:59' then '14:30'
                            when time(calldate) between '15:00:00' and '15:29:59' then '15:00'
                            when time(calldate) between '15:30:00' and '15:59:59' then '15:30'
                            when time(calldate) between '16:00:00' and '16:29:59' then '16:00'
                            when time(calldate) between '16:30:00' and '16:59:59' then '16:30'
                            when time(calldate) between '17:00:00' and '17:29:59' then '17:00'
                            when time(calldate) between '17:30:00' and '17:59:59' then '17:30'
                            when time(calldate) between '18:00:00' and '18:29:59' then '18:00'
                            when time(calldate) between '18:30:00' and '18:59:59' then '18:30'
                            when time(calldate) between '19:00:00' and '19:29:59' then '19:00'
                            when time(calldate) between '19:30:00' and '19:59:59' then '19:30'
                            when time(calldate) between '20:00:00' and '20:29:59' then '20:00'
                            when time(calldate) between '20:30:00' and '20:59:59' then '20:30'
                            when time(calldate) between '21:00:00' and '21:29:59' then '21:00'
                            when time(calldate) between '21:30:00' and '21:59:59' then '21:30'
                            when time(calldate) between '22:00:00' and '22:29:59' then '22:00'
                            when time(calldate) between '22:30:00' and '22:59:59' then '22:30'
                            when time(calldate) between '23:00:00' and '23:29:59' then '23:00'
                            when time(calldate) between '23:30:00' and '23:59:59' then '23:30'
                        end , disposition", campanhas_formatada);

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@tipotelefone", TipoTelefone);
                var adapter = new MySqlDataAdapter(cmd);

                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public DataTable ListarNaoAtendidosLinhaTempo(long tipotelefone, params string[] campanhas)
        {
            using (var conn = new MySqlConnection(Conexao.strConn))
            {
                string campanhas_formatada = string.Empty;
                foreach (var c in campanhas)
                    campanhas_formatada += string.Format("'{0}',", c);

                campanhas_formatada = campanhas_formatada.Remove(campanhas_formatada.LastIndexOf(','), 1);

                var query = string.Empty;
                query = string.Format(@"select 
                            case
                                when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                                when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                                when time(calldate) between '07:00:00' and '07:29:59' then '07:00'
                                when time(calldate) between '07:30:00' and '07:59:59' then '07:30'
                                when time(calldate) between '08:00:00' and '08:29:59' then '08:00'
                                when time(calldate) between '08:30:00' and '08:59:59' then '08:30'
                                when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                                when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                                when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                                when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                                when time(calldate) between '10:00:00' and '10:29:59' then '10:00'
                                when time(calldate) between '10:30:00' and '10:59:59' then '10:30'
                                when time(calldate) between '11:00:00' and '11:29:59' then '11:00'
                                when time(calldate) between '11:30:00' and '11:59:59' then '11:30'
                                when time(calldate) between '12:00:00' and '12:29:59' then '12:00'
                                when time(calldate) between '12:30:00' and '12:59:59' then '12:30'
                                when time(calldate) between '13:00:00' and '13:29:59' then '13:00'
                                when time(calldate) between '13:30:00' and '13:59:59' then '13:30'
                                when time(calldate) between '14:00:00' and '14:29:59' then '14:00'
                                when time(calldate) between '14:30:00' and '14:59:59' then '14:30'
                                when time(calldate) between '15:00:00' and '15:29:59' then '15:00'
                                when time(calldate) between '15:30:00' and '15:59:59' then '15:30'
                                when time(calldate) between '16:00:00' and '16:29:59' then '16:00'
                                when time(calldate) between '16:30:00' and '16:59:59' then '16:30'
                                when time(calldate) between '17:00:00' and '17:29:59' then '17:00'
                                when time(calldate) between '17:30:00' and '17:59:59' then '17:30'
                                when time(calldate) between '18:00:00' and '18:29:59' then '18:00'
                                when time(calldate) between '18:30:00' and '18:59:59' then '18:30'
                                when time(calldate) between '19:00:00' and '19:29:59' then '19:00'
                                when time(calldate) between '19:30:00' and '19:59:59' then '19:30'
                                when time(calldate) between '20:00:00' and '20:29:59' then '20:00'
                                when time(calldate) between '20:30:00' and '20:59:59' then '20:30'
                                when time(calldate) between '21:00:00' and '21:29:59' then '21:00'
                                when time(calldate) between '21:30:00' and '21:59:59' then '21:30'
                                when time(calldate) between '22:00:00' and '22:29:59' then '22:00'
                                when time(calldate) between '22:30:00' and '22:59:59' then '22:30'
                                when time(calldate) between '23:00:00' and '23:29:59' then '23:00'
                                when time(calldate) between '23:30:00' and '23:59:59' then '23:30'
                            end as Hora,
                            count(clid) as NaoAtendido
                        from
                            bilhetagem
                        where lastdata in ({0})
                             and disposition = 'NO ANSWER'
                             and tipotelefone = @TipoTelefone
                        group by case
                            when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                            when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                            when time(calldate) between '07:00:00' and '07:29:59' then '07:00'
                            when time(calldate) between '07:30:00' and '07:59:59' then '07:30'
                            when time(calldate) between '08:00:00' and '08:29:59' then '08:00'
                            when time(calldate) between '08:30:00' and '08:59:59' then '08:30'
                            when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                            when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                            when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                            when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                            when time(calldate) between '10:00:00' and '10:29:59' then '10:00'
                            when time(calldate) between '10:30:00' and '10:59:59' then '10:30'
                            when time(calldate) between '11:00:00' and '11:29:59' then '11:00'
                            when time(calldate) between '11:30:00' and '11:59:59' then '11:30'
                            when time(calldate) between '12:00:00' and '12:29:59' then '12:00'
                            when time(calldate) between '12:30:00' and '12:59:59' then '12:30'
                            when time(calldate) between '13:00:00' and '13:29:59' then '13:00'
                            when time(calldate) between '13:30:00' and '13:59:59' then '13:30'
                            when time(calldate) between '14:00:00' and '14:29:59' then '14:00'
                            when time(calldate) between '14:30:00' and '14:59:59' then '14:30'
                            when time(calldate) between '15:00:00' and '15:29:59' then '15:00'
                            when time(calldate) between '15:30:00' and '15:59:59' then '15:30'
                            when time(calldate) between '16:00:00' and '16:29:59' then '16:00'
                            when time(calldate) between '16:30:00' and '16:59:59' then '16:30'
                            when time(calldate) between '17:00:00' and '17:29:59' then '17:00'
                            when time(calldate) between '17:30:00' and '17:59:59' then '17:30'
                            when time(calldate) between '18:00:00' and '18:29:59' then '18:00'
                            when time(calldate) between '18:30:00' and '18:59:59' then '18:30'
                            when time(calldate) between '19:00:00' and '19:29:59' then '19:00'
                            when time(calldate) between '19:30:00' and '19:59:59' then '19:30'
                            when time(calldate) between '20:00:00' and '20:29:59' then '20:00'
                            when time(calldate) between '20:30:00' and '20:59:59' then '20:30'
                            when time(calldate) between '21:00:00' and '21:29:59' then '21:00'
                            when time(calldate) between '21:30:00' and '21:59:59' then '21:30'
                            when time(calldate) between '22:00:00' and '22:29:59' then '22:00'
                            when time(calldate) between '22:30:00' and '22:59:59' then '22:30'
                            when time(calldate) between '23:00:00' and '23:29:59' then '23:00'
                            when time(calldate) between '23:30:00' and '23:59:59' then '23:30'
                        end , disposition", campanhas_formatada);

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@tipotelefone", tipotelefone);
                var adapter = new MySqlDataAdapter(cmd);

                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public DataTable ListarOcupadoLinhaTempo(string nome_campanha)
        {
            using (var conn = new MySqlConnection(Conexao.strConn))
            {
                var query = string.Empty;
                query = @"select 
                            case
                                when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                                when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                                when time(calldate) between '07:00:00' and '07:29:59' then '07:00'
                                when time(calldate) between '07:30:00' and '07:59:59' then '07:30'
                                when time(calldate) between '08:00:00' and '08:29:59' then '08:00'
                                when time(calldate) between '08:30:00' and '08:59:59' then '08:30'
                                when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                                when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                                when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                                when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                                when time(calldate) between '10:00:00' and '10:29:59' then '10:00'
                                when time(calldate) between '10:30:00' and '10:59:59' then '10:30'
                                when time(calldate) between '11:00:00' and '11:29:59' then '11:00'
                                when time(calldate) between '11:30:00' and '11:59:59' then '11:30'
                                when time(calldate) between '12:00:00' and '12:29:59' then '12:00'
                                when time(calldate) between '12:30:00' and '12:59:59' then '12:30'
                                when time(calldate) between '13:00:00' and '13:29:59' then '13:00'
                                when time(calldate) between '13:30:00' and '13:59:59' then '13:30'
                                when time(calldate) between '14:00:00' and '14:29:59' then '14:00'
                                when time(calldate) between '14:30:00' and '14:59:59' then '14:30'
                                when time(calldate) between '15:00:00' and '15:29:59' then '15:00'
                                when time(calldate) between '15:30:00' and '15:59:59' then '15:30'
                                when time(calldate) between '16:00:00' and '16:29:59' then '16:00'
                                when time(calldate) between '16:30:00' and '16:59:59' then '16:30'
                                when time(calldate) between '17:00:00' and '17:29:59' then '17:00'
                                when time(calldate) between '17:30:00' and '17:59:59' then '17:30'
                                when time(calldate) between '18:00:00' and '18:29:59' then '18:00'
                                when time(calldate) between '18:30:00' and '18:59:59' then '18:30'
                                when time(calldate) between '19:00:00' and '19:29:59' then '19:00'
                                when time(calldate) between '19:30:00' and '19:59:59' then '19:30'
                                when time(calldate) between '20:00:00' and '20:29:59' then '20:00'
                                when time(calldate) between '20:30:00' and '20:59:59' then '20:30'
                                when time(calldate) between '21:00:00' and '21:29:59' then '21:00'
                                when time(calldate) between '21:30:00' and '21:59:59' then '21:30'
                                when time(calldate) between '22:00:00' and '22:29:59' then '22:00'
                                when time(calldate) between '22:30:00' and '22:59:59' then '22:30'
                                when time(calldate) between '23:00:00' and '23:29:59' then '23:00'
                                when time(calldate) between '23:30:00' and '23:59:59' then '23:30'
                            end as Hora,
                            count(clid) as Ocupado
                        from
                            bilhetagem
                        where lastdata = @nome_campanha
                             and disposition = 'BUSY'
                        group by case
                            when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                            when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                            when time(calldate) between '07:00:00' and '07:29:59' then '07:00'
                            when time(calldate) between '07:30:00' and '07:59:59' then '07:30'
                            when time(calldate) between '08:00:00' and '08:29:59' then '08:00'
                            when time(calldate) between '08:30:00' and '08:59:59' then '08:30'
                            when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                            when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                            when time(calldate) between '06:00:00' and '06:29:59' then '06:00'
                            when time(calldate) between '06:30:00' and '06:59:59' then '06:30'
                            when time(calldate) between '10:00:00' and '10:29:59' then '10:00'
                            when time(calldate) between '10:30:00' and '10:59:59' then '10:30'
                            when time(calldate) between '11:00:00' and '11:29:59' then '11:00'
                            when time(calldate) between '11:30:00' and '11:59:59' then '11:30'
                            when time(calldate) between '12:00:00' and '12:29:59' then '12:00'
                            when time(calldate) between '12:30:00' and '12:59:59' then '12:30'
                            when time(calldate) between '13:00:00' and '13:29:59' then '13:00'
                            when time(calldate) between '13:30:00' and '13:59:59' then '13:30'
                            when time(calldate) between '14:00:00' and '14:29:59' then '14:00'
                            when time(calldate) between '14:30:00' and '14:59:59' then '14:30'
                            when time(calldate) between '15:00:00' and '15:29:59' then '15:00'
                            when time(calldate) between '15:30:00' and '15:59:59' then '15:30'
                            when time(calldate) between '16:00:00' and '16:29:59' then '16:00'
                            when time(calldate) between '16:30:00' and '16:59:59' then '16:30'
                            when time(calldate) between '17:00:00' and '17:29:59' then '17:00'
                            when time(calldate) between '17:30:00' and '17:59:59' then '17:30'
                            when time(calldate) between '18:00:00' and '18:29:59' then '18:00'
                            when time(calldate) between '18:30:00' and '18:59:59' then '18:30'
                            when time(calldate) between '19:00:00' and '19:29:59' then '19:00'
                            when time(calldate) between '19:30:00' and '19:59:59' then '19:30'
                            when time(calldate) between '20:00:00' and '20:29:59' then '20:00'
                            when time(calldate) between '20:30:00' and '20:59:59' then '20:30'
                            when time(calldate) between '21:00:00' and '21:29:59' then '21:00'
                            when time(calldate) between '21:30:00' and '21:59:59' then '21:30'
                            when time(calldate) between '22:00:00' and '22:29:59' then '22:00'
                            when time(calldate) between '22:30:00' and '22:59:59' then '22:30'
                            when time(calldate) between '23:00:00' and '23:29:59' then '23:00'
                            when time(calldate) between '23:30:00' and '23:59:59' then '23:30'
                        end , disposition";

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nome_campanha", nome_campanha);
                var adapter = new MySqlDataAdapter(cmd);

                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public int ChamadasRealizada(long id_campanha)
        {
            using (var conn = new MySqlConnection(Conexao.strConn))
            {
                var query = string.Empty;
                query = @"select count(cargatelefone.id) as total 
                            from cargatelefone inner join carga on carga.id = cargatelefone.idcarga 
                           where cargatelefone.status not in(1,10,12) and cargatelefone.ativo = 1 
                             and carga.idcampanha = @campanha";

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@campanha", id_campanha);

                var adapter = new MySqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);

                List<object[]> retorno = new List<object[]>();
                foreach (DataRow r in dt.Rows)
                    return r["total"].ToInt32();

                return 0;
            }
        }

        public int ChamadasNaoRealizadas(long id_campanha)
        {
            using (var conn = new MySqlConnection(Conexao.strConn))
            {
                var query = string.Empty;
                query = @"select count(cargatelefone.id) as total 
                            from cargatelefone inner join carga on carga.id = cargatelefone.idcarga 
                           where cargatelefone.status in(1, 10) and cargatelefone.ativo = 1 
                             and carga.idcampanha = @campanha";

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@campanha", id_campanha);

                var adapter = new MySqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);

                List<object[]> retorno = new List<object[]>();
                foreach (DataRow r in dt.Rows)
                    return r["total"].ToInt32();

                return 0;
            }
        }

        public int ChamadasPositivas(long id_campanha)
        {
            using (var conn = new MySqlConnection(Conexao.strConn))
            {
                var query = string.Empty;
                query = @"select count(cargatelefone.id) as total 
                            from cargatelefone inner join carga on carga.id = cargatelefone.idcarga 
                           where cargatelefone.status in(2, 9) and cargatelefone.ativo = 1 
                             and carga.idcampanha = @campanha";

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@campanha", id_campanha);

                var adapter = new MySqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);

                List<object[]> retorno = new List<object[]>();
                foreach (DataRow r in dt.Rows)
                    return r["total"].ToInt32();

                return 0;
            }
        }

        public DataTable ListarResultados(params int[] ids_campanhas)
        {
            using (var conn = new MySqlConnection(Conexao.strConn))
            {

                string campanhas = string.Empty;
                foreach (var item in ids_campanhas)
                    campanhas += item + ",";

                campanhas = campanhas.Remove(campanhas.LastIndexOf(','), 1);

                var query = string.Empty;
                query = string.Format(@"select cargatelefonestatus.descricao, count(*) as total
                            from cargatelefone left join cargatelefonestatus on cargatelefonestatus.id = cargatelefone.status
	                             inner join carga on carga.id = cargatelefone.idcarga
                             and carga.idcampanha in({0})
                           group by cargatelefone.status", campanhas);

                var cmd = new MySqlCommand(query, conn);
                var adapter = new MySqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public int ListarQtdLigacoes(int id_campanha, DTO.SilverStatus status)
        {
            using (var conn = new MySqlConnection(Conexao.strConn))
            {
                var query = string.Empty;
                query = @"select count(cargatelefone.id) as total from cargatelefone inner join carga on carga.id = cargatelefone.idcarga where cargatelefone.status =@status and cargatelefone.ativo = 1 and carga.idcampanha = @campanha;";

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@status", (int)status);
                cmd.Parameters.AddWithValue("@campanha", id_campanha);

                var adapter = new MySqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);

                List<object[]> retorno = new List<object[]>();
                foreach (DataRow r in dt.Rows)
                    return r["total"].ToInt32();

                return 0;
            }
        }

        public int ListarQuantidadeChamadas(string nome_campanha, StatusTelefoneDashboard status)
        {
            var status_ligacao = string.Empty;
            switch (status)
            {
                case StatusTelefoneDashboard.Atendido:
                    status_ligacao = "ANSWERED";
                    break;
                case StatusTelefoneDashboard.NaoAtendido:
                    status_ligacao = "NO ANSWER";
                    break;
                case StatusTelefoneDashboard.Ocupado:
                    status_ligacao = "BUSY";
                    break;
                case StatusTelefoneDashboard.NaoExiste:
                    status_ligacao = "";
                    break;
                default:
                    break;
            }

            using (var conn = new MySqlConnection(Conexao.strConn))
            {
                var query = string.Empty;
                query = @"select lastdata as campanha, count(lastdata) as total
                            from
                                bilhetagem
                            where
                                lastdata is not null
                            and disposition = @status
                            and lastdata = @campanha
                            group by lastdata";

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@status", status_ligacao);
                cmd.Parameters.AddWithValue("@campanha", nome_campanha);

                var adapter = new MySqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);

                List<object[]> retorno = new List<object[]>();
                foreach (DataRow r in dt.Rows)
                    return r["total"].ToInt32();

                return 0;
            }
        }

        public int QtdLigacaoStatusCarga(int id_campanha, SilverStatus status)
        {

            using (var conn = new MySqlConnection(Conexao.strConn))
            {

                var query = string.Empty;
                query = @"select count(cargatelefone.idtipo) total
                            from campanha, carga, cargatelefone
                           where carga.id = cargatelefone.idcarga
                             and campanha.id = carga.idcampanha
                             and campanha.id = @campanha
                             and idtipo = @status
                           group by cargatelefone.idtipo";

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@status", (int)status);
                cmd.Parameters.AddWithValue("@campanha", id_campanha);

                conn.Open();
                var reader = cmd.ExecuteReader();
                int total = 0;
                while (reader.Read())
                    total = reader["total"].ToInt32();

                reader.Close();
                conn.Close();
                return total;
            }
        }

        public string[] ListarUsuariosCampanha(params string[] campanhas)
        {
            List<string> operadores = new List<string>();

            using (var conn = new MySqlConnection(Conexao.strConn))
            {
                string filtro = string.Empty;
                foreach (string v in campanhas)
                    filtro += "'" + v + "'" + ", ";

                filtro = filtro.Remove(filtro.LastIndexOf(','), 1);

                var query = string.Empty;
                query = string.Format(@"select mid(dstchannel,5,4) as ramal
                                          from bilhetagem
                                         where lastdata in({0})
                                           and mid(dstchannel,5,4)  
                                         group by mid(dstchannel,5,4) ", filtro);

                conn.Open();

                var cmd = new MySqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    operadores.Add(reader["ramal"].ToString());

                reader.Close();
                conn.Close();
                return operadores.ToArray();
            }
        }

        public int TotalNumerosDiscados(string ramal, params  string[] campanhas)
        {

            using (var conn = new MySqlConnection(Conexao.strConn))
            {
                string campanhas_formatada = string.Empty;
                foreach (var c in campanhas)
                    campanhas_formatada += string.Format("'{0}',", c);

                campanhas_formatada = campanhas_formatada.Remove(campanhas_formatada.LastIndexOf(','), 1);

                var query = string.Empty;
                query = string.Format(@"select count(id) as total
                                          from bilhetagem
                                         where lastdata in({0})
                                           and mid(dstchannel,5,4) = @ramal  
                                         group by mid(dstchannel,5,4)",
                                                        campanhas_formatada);

                conn.Open();

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ramal", ramal);

                var reader = cmd.ExecuteReader();
                int total = 0;
                while (reader.Read())
                    total = reader["total"].ToInt32();

                reader.Close();
                conn.Close();
                return total;
            }
        }

        public int TotalChamadas(string nome_campanha)
        {
            using (var conn = new MySqlConnection(Conexao.strConn))
            {

                var query = string.Empty;
                query = @"select count(id) as total
                            from bilhetagem
                           where lastdata = @campanha
                             and mid(dstchannel,5,4) = @ramal  
                           group by mid(dstchannel,5,4)";

                conn.Open();

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@campanha", nome_campanha);

                var reader = cmd.ExecuteReader();
                int total = 0;
                while (reader.Read())
                    total = reader["total"].ToInt32();

                reader.Close();
                conn.Close();
                return total;
            }
        }

        public DataTable ListarTempoLogado(params int[] ids_campanhas)
        {
            string campanhas = string.Empty;
            foreach (var item in ids_campanhas)
                campanhas += item + ",";

            campanhas = campanhas.Remove(campanhas.LastIndexOf(','), 1);

            using (var conn = new MySqlConnection(Conexao.strConn))
            {
                var query = string.Empty;
                query = @"select count(cargatelefone.id) as total 
                            from cargatelefone inner join carga on carga.id = cargatelefone.idcarga 
                           where cargatelefone.status not in(1,10,12) and cargatelefone.ativo = 1 
                             and carga.idcampanha = @campanha";

                var cmd = new MySqlCommand(query, conn);
                // cmd.Parameters.AddWithValue("@campanha", id_campanha);

                var adapter = new MySqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);

                return dt;
            }
        }

        public DataTable ListarTempoEspera(int[] ids_campanhas)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}