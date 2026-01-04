using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;

namespace Silver.ProxyDiscagem.Cliente
{
    /// <summary>
    /// Desenvolvido por: Francisco Silva   
    ///             Data: 23/06/2014
    /// </summary>
    public class ProxyCliente : IDisposable
    {
        public delegate void RespostaServidorDiscagem(Silver.DTO.RespostaSolicitacaoDiscagem resposta);
        public event RespostaServidorDiscagem OnRespostaServidorDiscagem;

        private TcpClient cliente;
        private StreamWriter stream_writer;
        private Boolean conectado;

        public bool SolicitacaoRespondida { get; set; }
        public Task TaskEscutaResposta { get; set; }

        public ProxyCliente(String endereco_ip, int numero_porta)
        {
            cliente = new TcpClient();
            cliente.Connect(endereco_ip, numero_porta);
            stream_writer = new StreamWriter(cliente.GetStream(), Encoding.ASCII);

            TaskEscutaResposta = System.Threading.Tasks.Task.Factory.StartNew(() => { RespostaSolicitacao(cliente); });
        }

        public void SolicitaDiscagem(Silver.DTO.SolicitacaoDiscagem solicitacao)
        {
            stream_writer.WriteLine(solicitacao.ToString());
            stream_writer.AutoFlush = true;
            stream_writer.Flush();

            SolicitacaoRespondida = false;
        }

        public void RespostaSolicitacao(object conexao_tcp)
        {
            var cliente = (TcpClient)conexao_tcp;
            StreamReader stream_reader = new StreamReader(cliente.GetStream(), Encoding.ASCII);
            string resposta = stream_reader.ReadToEnd();

            if (OnRespostaServidorDiscagem != null)
            {
                var resposta_campos = resposta.Split('|');
                OnRespostaServidorDiscagem(new DTO.RespostaSolicitacaoDiscagem()
                {
                    MotivoResposta = resposta_campos[6],
                    RespostaSolicitacao = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", resposta_campos[0], resposta_campos[1], resposta_campos[2], resposta_campos[3], resposta_campos[4], resposta_campos[5])
                });

                SolicitacaoRespondida = true;
            }
        }

        public void Dispose()
        {
            //if (stream_reader != null)
            //{
            //    stream_reader.Close();
            //    stream_reader.Dispose();
            //}

            if (stream_writer != null)
            {
                stream_writer.Close();
                stream_writer.Dispose();
            }

            if (cliente != null)
                cliente.Close();
        }
    }
}
