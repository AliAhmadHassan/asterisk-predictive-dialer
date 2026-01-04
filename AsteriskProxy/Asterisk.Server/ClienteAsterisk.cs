using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Net;

namespace Silver.AsteriskClient
{
    public class ClienteAsterisk : IDisposable
    {
        System.Diagnostics.EventLog evt = new System.Diagnostics.EventLog();

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
        private delegate void listenForConnectionsDelegate();

        //Stream para conexão com asterisk
        private NetworkStream stream_asterisk;

        //Stream para conexão com cliente
        private NetworkStream stream_cliente;

        public void IniciarConexao()
        {
            #region Asterisk
            Console.Out.WriteLine("Iniciando conexão com servidor Asterisk*");
            socket_asterisk = new TcpClient(ip_asterisk, porta_asterisk);
            stream_asterisk = socket_asterisk.GetStream();

            readStreamDelegate readSteam = new readStreamDelegate(readStream);
            readSteam.BeginInvoke(stream_asterisk, new AsyncCallback(readStreamEnd), null);
            #endregion

            #region Server
            IPAddress localAddr = IPAddress.Any;
            socket_Server = new TcpListener(localAddr, porta_server);

            listenForConnectionsDelegate listenForConnections = new listenForConnectionsDelegate(AceitarConexoes);
            listenForConnections.BeginInvoke(new AsyncCallback(endListen), null);
            #endregion
        }

        //Escutando do Asterisk
        private void readStream(NetworkStream stream)
        {
            String command = "";
            Int32 errorCount = 0;

            Console.Out.WriteLine("Conexão com servidor Asterisk* estabelecida com sucesso.");
            Console.Out.WriteLine("Escutando servidor Asterisk*.");

            while (true)
            {
                try
                {
                    errorCount += 1;
                    Byte[] bytes = new Byte[1024];
                    Int32 received;
                    String[] lines;
                    received = stream.Read(bytes, 0, 1024);
                    command += Encoding.ASCII.GetString(bytes, 0, received);

                    if (Regex.Match(command, "(Asterisk Call Manager/)([0-9]*).([0-9]*)\\r\\n", RegexOptions.IgnoreCase).Success)
                    {
                        command = "";
                        Login();
                    }

                    lines = Regex.Split(command, "\\r\\n\\r\\n", RegexOptions.Multiline);

                    for (Int32 i = 0; i < lines.Length - 1; i++)
                    {
                        if (lines[i].ToUpper().Contains("SIP/2002"))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Out.WriteLine(lines[i]);
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        else
                        {
                            Console.Out.WriteLine(lines[i]);
                        }
                        Console.Out.WriteLine("*******************************************************");

                        ExecutarSBBuffer(lines[i]);
                    }

                    command = lines[lines.Length - 1];
                    errorCount -= 1;
                }
                catch (Exception e)
                {
                    evt.WriteEntry(e.Message);
                }

                if (errorCount == 10)
                {
                    evt.WriteEntry("AMIProxy is no longer accepting messages from the asterisk server so will commence shutdown.");
                    return;
                }

            }
        }

        //Escutando os Discadores
        private void AceitarConexoes()
        {
            try
            {
                byte[] buffer = new byte[1024];
                String dados_recebidos = null;

                socket_Server.Start();

                Console.WriteLine("Aguardando novas conexões... ");
                cliente_conectado = socket_Server.AcceptTcpClient();

                Console.WriteLine("[{0}] - Conexão estabelecida com cliente!", DateTime.Now);
                stream_cliente = cliente_conectado.GetStream();

                while (true)
                {
                    Int32 received;
                    received = stream_cliente.Read(buffer, 0, buffer.Length);
                    //Dados Recebidos do Cliente (Solicitações)
                    dados_recebidos = System.Text.Encoding.ASCII.GetString(buffer, 0, received);
                    SendMessageToAsterisk(dados_recebidos);
                }
            }
            catch (Exception e)
            {
                cliente_conectado = null;
                stream_cliente = null;
                AceitarConexoes();
            }
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
                        case "Newstate":
                            SendMessageToClient(bloco + "\r\n\r\n");
                            break;

                        case "Bridge":
                            SendMessageToClient(bloco + "\r\n\r\n");
                            break;

                        case "Hangup":
                        case "NewCallerId":
                        case "QueueMemberPaused":
                        case "QueueMember":
                        case "QueueParams":
                        case "Follows":
                        case "PeerStatus":
                        case "Error":
                            SendMessageToClient(bloco + "\r\n\r\n");
                            break;
                        default:
                            break;
                    }
                }

            }
            catch { }
            finally { }
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
                evt.WriteEntry(e.Message);
            }
        }

        private void SendMessageToClient(String message)
        {
            try
            {
                Byte[] msg = Encoding.ASCII.GetBytes(message);
                stream_cliente.Write(msg, 0, msg.Length);
            }
            catch (Exception e)
            {
                evt.WriteEntry(e.Message);
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
