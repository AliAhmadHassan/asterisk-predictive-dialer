using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net;
using System.Runtime.Remoting.Messaging;

namespace Silver.ProxyDiscagem.Teste
{
    public class ProxyServerTcp
    {
        public int ProtocoloConexao { get; set; }
        public int TotalConexoesAtivas { get; set; }

        public delegate void SolicitacaoDiscagem(Silver.DTO.SolicitacaoDiscagem solicitacao);
        public event SolicitacaoDiscagem OnSolicitacaoDiscagem;

        public delegate void ClienteConectado(TcpClient cliente);

        private TcpListener servidor;
        private Boolean em_execucao;
        private int porta_conexao = 9595;

        public ProxyServerTcp()
        {
            
        }

        public ProxyServerTcp(int porta)
            : base()
        {
            porta_conexao = porta;

        }

        public void IniciarServico()
        {
            servidor = new TcpListener(IPAddress.Any, porta_conexao);
            servidor.Start();

            em_execucao = true;
            AguardarConexao();
        }

        public void AguardarConexao()
        {
            while (em_execucao)
            {
                TcpClient novo_cliente = servidor.AcceptTcpClient();
                novo_cliente.NoDelay = true;

                ProtocoloConexao++;
                TotalConexoesAtivas++;

                Console.Out.WriteLine(Environment.NewLine + "[{0}] - [INFO] - Cliente Conectado: Número da conexão: {1}".ToUpper(),
                    DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff"), ProtocoloConexao.ToString("000000"));

                ClienteConectado cliente_conectado = new ClienteConectado(ProcessarConexao);
                cliente_conectado.BeginInvoke(novo_cliente, new AsyncCallback(EndNovaConexaoAsync), ProtocoloConexao.ToString("000000"));
            }
        }

        public void BeginNovaConexao(TcpClient cliente)
        {
            String dados_transferencia = null;
            string ip_cliente = IpCliente(ref cliente);
            try
            {
                while (cliente.Client.Connected)
                {
                    dados_transferencia = new StreamReader(cliente.GetStream(), Encoding.ASCII).ReadLine();
                    if (OnSolicitacaoDiscagem != null)
                    {
                        var valores = dados_transferencia.Split('|');
                        OnSolicitacaoDiscagem(new Silver.DTO.SolicitacaoDiscagem() { DDD = valores[0], Telefone = valores[1], Campanha = valores[2], IdTelefone = valores[3], TipoTelefone = valores[4], IdCampanha = valores[5], Cliente = cliente });
                    }
                }
            }
            catch (NullReferenceException null_ex) { Console.Out.WriteLine("[{0}] - Mensagem: {1}. CLIENTE DESCONECTADO!", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff"), null_ex.Message); }
            catch (IOException) { Console.Out.WriteLine("[{0}] - Cliente Desconectado: {1}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff"), ip_cliente); }
            catch (Exception ex) { Console.Out.WriteLine("[{0}] - {1}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff"), ex.Message); }
            finally { }
        }

        private void EndNovaConexao(IAsyncResult resultado_final)
        {
            AsyncResult resultado = (AsyncResult)resultado_final;
            ClienteConectado chamador = (ClienteConectado)resultado.AsyncDelegate;

            string formatString = (string)resultado_final.AsyncState;

            chamador.EndInvoke(resultado_final);
            TotalConexoesAtivas--;

            Console.Out.WriteLine(Environment.NewLine + "[{0}] - [INFO] - Cliente Desconectado: Número da conexão: {1}".ToUpper(),
                  DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff"), formatString);

            Console.Out.WriteLine(Environment.NewLine + "[{0}] - [INFO] - Total de clientes conectados: {1}".ToUpper(),
                  DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff"), TotalConexoesAtivas.ToString("000000"));

            Console.Out.WriteLine("*".PadRight(80, '*'));
        }

        private string IpCliente(ref TcpClient cliente)
        {
            IPEndPoint ep = cliente.Client.RemoteEndPoint as IPEndPoint;
            if (ep == null)
                return "Desconhecido";

            return ep.Address.ToString();
        }
    }
}
