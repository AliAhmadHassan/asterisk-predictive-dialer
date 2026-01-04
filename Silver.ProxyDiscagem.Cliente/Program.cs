using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.ProxyDiscagem.Cliente
{
    class Program
    {
        static ProxyCliente conexao;

        static void Main(string[] args)
        {
            string nova_ligacao = "sim";
            while (nova_ligacao.ToLower().Equals("sim"))
            {
                Console.Out.Write("\n[{0}] - Digite Sim para uma nova ligação: ", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff"));
                nova_ligacao = Console.ReadLine();

                if (nova_ligacao.ToLower().Equals("sim"))
                {
                    conexao = new ProxyCliente("192.168.21.122", 9595);
                    conexao.OnRespostaServidorDiscagem += conexao_OnRespostaServidorDiscagem;
                    conexao.SolicitaDiscagem(new Silver.DTO.SolicitacaoDiscagem() { TipoTelefone = "Comercial", IdTelefone = "666", IdCampanha = "18", Campanha = "Campanha_Itau", Telefone = "32952004", DDD = "11" });
                    while (!conexao.SolicitacaoRespondida)
                    {
                        Console.Out.WriteLine("[{0}] - Aguardando resposta do Servidor de Discagem.", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff"));
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }
        }

        static void conexao_OnRespostaServidorDiscagem(DTO.RespostaSolicitacaoDiscagem resposta)
        {
            Console.Out.Write("\n[{0}] - Resposta da solicitação: {1}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff"), resposta.ToString());
            conexao.SolicitacaoRespondida = true;
        }
    }
}
