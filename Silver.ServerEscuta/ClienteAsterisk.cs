using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Net;
using System.Diagnostics;
using System.IO;

namespace Silver.ServerEscuta
{
    public class ClienteAsterisk : IDisposable
    {
        string usuario_asterisk = ConfigurationManager.AppSettings["application.asterisk.user"];
        string senha_asterisk = ConfigurationManager.AppSettings["application.asterisk.secret"];
        string ip_asterisk = ConfigurationManager.AppSettings["application.asterisk.ip"];
        int porta_asterisk = Convert.ToInt32(ConfigurationManager.AppSettings["application.asterisk.porta"]);
        int porta_server = Convert.ToInt32(ConfigurationManager.AppSettings["application.server.porta"]);

        private TcpClient socket_asterisk;
        private TcpClient cliente_conectado;
        private TcpListener socket_Server;

        // Private Delegates
        private delegate void readStreamDelegate(NetworkStream stream);
        private delegate void readSocketDelegate(Socket socket);

        //Stream para conexão com asterisk
        private NetworkStream stream_asterisk;

        private void IniciarEscutaAsterisk()
        {
            IniciarEscritaAsterisk();

            readStreamDelegate readSteam = new readStreamDelegate(readStream);
            readSteam.BeginInvoke(stream_asterisk, new AsyncCallback(readStreamEnd), null);
        }

        private void IniciarEscritaAsterisk()
        {
            try
            {
                if (socket_asterisk != null)
                {
                    socket_asterisk.Close();
                    socket_asterisk = null;

                    stream_asterisk.Close();
                    stream_asterisk.Dispose();
                    stream_asterisk = null;
                }

                socket_asterisk = new TcpClient(ip_asterisk, porta_asterisk);
                stream_asterisk = socket_asterisk.GetStream();
            }
            catch (Exception ex)
            {
                SaidaLogs(ex.Message, Common.LoggerType.ERRO);
            }
        }

        public void IniciarConexao()
        {
            #region Asterisk
            SaidaLogs("Iniciando conexão com servidor Asterisk*", Common.LoggerType.INFO);
            IniciarEscutaAsterisk();

            #endregion

            #region Server
            //IPAddress localAddr = IPAddress.Any;
            //socket_Server = new TcpListener(localAddr, porta_server);

            //listenForConnectionsDelegate listenForConnections = new listenForConnectionsDelegate(AceitarConexoes);
            //listenForConnections.BeginInvoke(new AsyncCallback(endListen), null);
            #endregion
        }

        System.Timers.Timer timer_checkout;

        public void IniciarTimerCheckout()
        {
            timer_checkout = null;
            timer_checkout = new System.Timers.Timer(Convert.ToDouble(ConfigurationManager.AppSettings["application.time.checkout"]));
            timer_checkout.Elapsed += timer_checkout_Elapsed;
        }

        private void readStream(NetworkStream stream)
        {
            String command = "";
            Int32 errorCount = 0;

            SaidaLogs("Conexão com servidor Asterisk* estabelecida com sucesso.", Common.LoggerType.INFO);
            SaidaLogs("Escutando servidor Asterisk*.", Common.LoggerType.INFO);

            while (true)
            {
                try
                {
                    errorCount += 1;
                    Byte[] bytes = new Byte[1024];
                    Int32 received;
                    String[] lines;
                    //timer_checkout.Start();
                    received = stream.Read(bytes, 0, 1024);
                    command += Encoding.ASCII.GetString(bytes, 0, received);

                    if (Regex.Match(command, "(Asterisk Call Manager/)([0-9]*).([0-9]*)\\r\\n", RegexOptions.IgnoreCase).Success)
                    {
                        command = "";
                        Login();
                    }

                    lines = Regex.Split(command, "\\r\\n\\r\\n", RegexOptions.Multiline);
                    for (Int32 i = 0; i < lines.Length - 1; i++)
                        ExecutarSBBufferGravacao(lines[i]);

                    command = lines[lines.Length - 1];
                    errorCount -= 1;
                }
                catch (Exception e)
                {
                    SaidaLogs(e.Message + Environment.NewLine + "NOTA DO DESENVOLVEDOR: Verificar a necessidade de restaura o status do socket", Common.LoggerType.ERRO);
                    IniciarEscutaAsterisk();
                }
                finally
                {
                    //if (timer_checkout != null)
                    //    timer_checkout.Stop();
                    //else
                    //    IniciarTimerCheckout();
                }

                if (errorCount == 10)
                {
                    SaidaLogs("A quantidade de tentativas de conexão com o servidor Asterisk excedeu a 10x", Common.LoggerType.ERRO);
                    return;
                }

            }
        }

        void timer_checkout_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            IniciarEscutaAsterisk();
        }

        private void ExecutarSBBufferGravacao(string bloco)
        {
            if ((bloco.Contains('\n')) && (bloco.Contains(':')))
            {
                var Linhas = bloco.Split('\n');
                switch (Linhas[0].Split(':')[1].Trim())
                {
                    case "Newstate":
                    case "Bridge":
                    case "Hangup":
                    case "NewCallerId":
                    case "QueueMemberPaused":
                    case "Error":
                        new BLL.SaidaAsterisk().Cadastrar(bloco);
                        break;
                }
            }
        }

        private void SaidaLogs(string mensagem, Common.LoggerType tipolog)
        {
            string path_logs = Path.Combine(ConfigurationManager.AppSettings["application.path.logs"], string.Format("Logs-{0}_Hora-{1}.log", "ServerEscuta", DateTime.Now.ToString("ddMMyyyyHH")));
            Silver.Common.Logger.LoggerAsync.pathLog = path_logs;
            Silver.Common.Logger.LoggerAsync.WriteLog(new Common.Logger.LoggerMessage { MessageLogger = mensagem, TypeLogger = tipolog });
        }

        private void ExecutarSBBuffer(string bloco)
        {
            try
            {
                if ((bloco.Contains('\n')) && (bloco.Contains(':')))
                {
                    var Linhas = bloco.Split('\n');

                    switch (Linhas[0].Split(':')[1].Trim())
                    {
                        case "Bridge":
                            RamalAtendeu(Linhas);
                            break;

                        default:
                            break;
                    }
                }

            }
            catch { }
            finally { }
        }

        private void RamalAtendeu(string[] Linhas)
        {
            try
            {
                SaidaLogs("O evento RamalAtedeu foi acionado.", Common.LoggerType.INFO);
                var Channel2 = string.Empty; //Ramal
                var CallerID1 = string.Empty; //Destino

                foreach (string Linha in Linhas)
                {
                    var linhaSeparado = Linha.Split(':');

                    var Chave = linhaSeparado[0].Trim();
                    var Valor = linhaSeparado[1].Trim();

                    switch (Chave)
                    {
                        case "Channel2":
                            Channel2 = Valor;
                            break;

                        case "CallerID1":
                            CallerID1 = Valor;
                            break;
                    }
                }

                long Ramal = Convert.ToInt64(Channel2.Substring(4, Channel2.IndexOf('-') - 4));
                SaidaLogs(string.Format("O ramal {0} atendeu", Ramal), Common.LoggerType.INFO);

                string telefone = CallerID1;
                SaidaLogs("Número do cliente: " + telefone, Common.LoggerType.INFO);
                DTO.Carga carga = new BLL.Carga().SelectPeloTelefone(telefone.Substring(telefone.Length - 8));
                string CPF = string.Empty;
                if (carga != null)
                {
                    CPF = carga.Chave1;
                }
                else
                {
                    carga = new BLL.Carga().SelectPeloTelefone(telefone.Substring(telefone.Length - 9));
                    if (carga == null)
                    {

                    }
                    else
                        CPF = carga.Chave1;
                }

                if (CPF == "")
                {
                    SaidaLogs("O número do cpf está vazio.", Common.LoggerType.INFO);
                    SaidaLogs(string.Format("O ramal {0} desligou", Ramal), Common.LoggerType.INFO);
                    return;
                }

                var url = string.Format(ConfigurationManager.AppSettings["application.url.crm"], CPF);
                SaidaLogs("Url Informada: " + url, Common.LoggerType.INFO);
                new BLL.UsuarioLogado().Atualizar(new DTO.UsuarioLogado { Ramal = Ramal, Url = url });
            }
            catch (Exception ex)
            {
                SaidaLogs(ex.Message, Common.LoggerType.ERRO);
            }
        }

        private void readStreamEnd(IAsyncResult result)
        {
        }

        private void endListen(IAsyncResult result)
        {

        }

        private void Login()
        {
            SendMessageToAsterisk("Action: Login\r\nActionID: [Login###]\r\nUsername: " + usuario_asterisk + "\r\nSecret: " + senha_asterisk + "\r\n\r\n");
        }

        private void SendMessageToAsterisk(String message)
        {
            try
            {
                Byte[] data = Encoding.ASCII.GetBytes(message);
                stream_asterisk.Write(data, 0, data.Length);
            }
            catch (Exception e)
            {
                SaidaLogs(e.Message, Common.LoggerType.ERRO);
            }
        }

        public void Dispose()
        {

        }
    }

    public class AsteriskClientException : Exception
    {
        public AsteriskClientException(string mensagem)
            : base(mensagem)
        {
        }
    }

    public static class RandomFactory
    {
        private static Random globalRandom = new Random();

        public static Random Create()
        {
            lock (globalRandom)
            {
                Random newRandom = new Random(globalRandom.Next());
                return newRandom;
            }
        }
    }
}
