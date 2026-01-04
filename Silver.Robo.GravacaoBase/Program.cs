using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.IO;
using System.Data;

namespace Silver.Robo.GravacaoBase
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Out.WriteLine("[{0}] - Iniciando processo de importação de gravações", DateTime.Now);
            new ImportarGravacao(ConfigurationManager.AppSettings["silver.pathgravacoes"]).Importar();
            Console.WriteLine("A importação dos arquivos foi finalizada com sucesso");
            Console.Read();
        }
    }

    public class ImportarGravacao
    {
        public string PathGravacao { get; set; }

        public ImportarGravacao()
        {

        }

        public ImportarGravacao(string path_gravacoes)
        {
            PathGravacao = path_gravacoes;
        }

        public void Importar()
        {
            if (String.IsNullOrEmpty(PathGravacao))
                throw new ArgumentException("O path das gravações não foi encontrado!");

            if (!Directory.Exists(PathGravacao))
                throw new ArgumentException("Diretório informada não encontrado!");

            DataTable tabela_retorno = new DataTable("bilhetagem");
            string[] files = Directory.GetFiles(PathGravacao);

            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStringSilver"].ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand("SPSpregravacao", conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                DateTime filtro_busca = DateTime.Now; //-1

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("inInicio", new DateTime(filtro_busca.Year, filtro_busca.Month, filtro_busca.Day, 0, 0, 0));
                cmd.Parameters.AddWithValue("inFim", new DateTime(filtro_busca.Year, filtro_busca.Month, filtro_busca.Day, 23, 59, 59));

                adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(tabela_retorno);
            }

            foreach (DataRow row in tabela_retorno.Rows)
            {
                string path_gravacao_base = row["PathGravacao"].ToString();
                Console.Out.WriteLine("Importando gravação da bilhetagem {0}", row["id"].ToString());
                if (string.IsNullOrEmpty(path_gravacao_base))
                {
                    Console.Out.WriteLine("[{0}] - Não foi encontrado um path para a bilhetagem: {1}", DateTime.Now, row["id"].ToString());
                    continue;
                }

                using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStringSilver"].ConnectionString))
                {
                    MySqlCommand cmd_insert = new MySqlCommand("SPIgravacao", conn);
                    cmd_insert.CommandType = CommandType.StoredProcedure;
                    cmd_insert.Parameters.AddWithValue("inId", row["Id"].ToString());

                    string arquivo_atual = files.Where(c => c.Equals(row["PathGravacao"].ToString())).FirstOrDefault();

                    //Verifica se o nome do arquivo está na lista de arquivos encontrados no diretório
                    if (string.IsNullOrEmpty(arquivo_atual))
                    {
                        Console.Out.WriteLine("[{0}] - O arquivo não foi encontrado no diretório informado para a bilhetagem: {1}", DateTime.Now, row["id"].ToString());
                        continue;
                    }

                    using (FileStream fs = new FileStream(arquivo_atual, FileMode.Open, FileAccess.Read))
                    {
                        using (BinaryReader br = new BinaryReader(fs))
                        {
                            byte[] gravacao = br.ReadBytes(Convert.ToInt32(fs.Length));
                            cmd_insert.Parameters.AddWithValue("inGravacao", gravacao);
                        }
                    }
                    conn.Open();
                    cmd_insert.ExecuteScalar();
                    conn.Close();
                }
            }

        }
    }
}
