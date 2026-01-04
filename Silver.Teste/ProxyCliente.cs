using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using Silver.DTO;

namespace Silver.Discador
{
    /// <summary>
    /// Desenvolvido por: Francisco Silva   
    ///             Data: 23/06/2014
    /// </summary>
    public class ProxyCliente : IDisposable
    {
        public delegate void RespostaServidorDiscagem(Silver.DTO.RespostaSolicitacaoDiscagem resposta);
        public event RespostaServidorDiscagem OnRespostaServidorDiscagem;

        public delegate void EscutarRespostaDiscagem(TcpClient cliente_tcp);

        private TcpClient cliente;
        private StreamWriter stream_writer;

        public bool SolicitacaoRespondida { get; set; }
        public Task TaskEscutaResposta { get; set; }

        public ProxyCliente(String endereco_ip, int numero_porta)
        {
            cliente = new TcpClient();
            cliente.Connect(endereco_ip, numero_porta);
        }

        static int qtd_tentativa_discagem = 1;

        public void SolicitaDiscagem(Silver.DTO.SolicitacaoDiscagem solicitacao)
        {
            using (stream_writer = new StreamWriter(cliente.GetStream(), Encoding.ASCII))
            {
                stream_writer.AutoFlush = true;
                stream_writer.WriteLine(solicitacao.ToString());
                stream_writer.Flush();

                RespostaSolicitacaoDiscagem resposta = null;
                using (StreamReader stream_reader = new StreamReader(cliente.GetStream(), Encoding.ASCII))
                {
                    string resposta_servidor = stream_reader.ReadToEnd();
                    var resposta_campos = resposta_servidor.Split('|');

                    resposta = new Silver.DTO.RespostaSolicitacaoDiscagem()
                    {
                        MotivoResposta = resposta_campos[6],
                        RespostaSolicitacao = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", resposta_campos[0], resposta_campos[1], resposta_campos[2], resposta_campos[3], resposta_campos[4], resposta_campos[5])
                    };

                    if (OnRespostaServidorDiscagem != null)
                        OnRespostaServidorDiscagem(resposta);
                    
                    Desconectar(cliente);
                }

                this.SolicitacaoRespondida = true;
            }
        }

        public void BeginRespostaSolicitacao(TcpClient conexao_tcp)
        {
            var cliente = (TcpClient)conexao_tcp;
            try
            {
                using (StreamReader stream_reader = new StreamReader(cliente.GetStream(), Encoding.ASCII))
                {
                    string resposta = stream_reader.ReadToEnd();

                    if (OnRespostaServidorDiscagem != null)
                    {
                        var resposta_campos = resposta.Split('|');
                        OnRespostaServidorDiscagem(new Silver.DTO.RespostaSolicitacaoDiscagem()
                        {
                            MotivoResposta = resposta_campos[6],
                            RespostaSolicitacao = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", resposta_campos[0], resposta_campos[1], resposta_campos[2], resposta_campos[3], resposta_campos[4], resposta_campos[5])
                        });
                    }

                    Desconectar(cliente);
                    SolicitacaoRespondida = true;
                }
            }
            catch (IOException io_ex)
            {
                Console.Out.WriteLine("{0}", io_ex.Message);
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("{0}", ex.Message);
            }
            finally { }
        }

        public void EndRespostaSolicitacao(IAsyncResult resultado_final)
        {
            AsyncResult resultado = (AsyncResult)resultado_final;
            EscutarRespostaDiscagem chamador = (EscutarRespostaDiscagem)resultado.AsyncDelegate;

            string formatString = (string)resultado_final.AsyncState;
            chamador.EndInvoke(resultado_final);
        }

        public void Desconectar(TcpClient conexao_tcp)
        {
            if (conexao_tcp != null)
            {
                if (conexao_tcp.Connected)
                {
                    if (conexao_tcp.Client.Connected)
                    {
                        conexao_tcp.Client.Disconnect(false);
                        conexao_tcp.Client.Close();
                        conexao_tcp.Client.Dispose();
                        conexao_tcp.Close();
                    }
                }
            }

            Console.Out.WriteLine("[{0}] - [INFO] - SOLICITAÇÃO RESPONDIDA - Cliente Desconectado", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff"));
        }

        public void Dispose()
        {

        }
    }
}
