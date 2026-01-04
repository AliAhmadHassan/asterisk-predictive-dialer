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
        private TcpListener socket_Server;
        Socket newConnection;

        // Private Delegates
        private delegate void readStreamDelegate(NetworkStream stream);
        private delegate void readSocketDelegate(Socket socket);
        private delegate void listenForConnectionsDelegate();

        private NetworkStream netStreamAsterisk;

        public void IniciarConexao()
        {
            #region Asterisk
            socket_asterisk = new TcpClient(ip_asterisk, porta_asterisk);
            netStreamAsterisk = socket_asterisk.GetStream();

            readStreamDelegate readSteam = new readStreamDelegate(readStream);
            readSteam.BeginInvoke(netStreamAsterisk, new AsyncCallback(readStreamEnd), null);
            #endregion

            #region Server
            IPAddress localAddr = IPAddress.Any;
            socket_Server = new TcpListener(localAddr, porta_server);

            

            listenForConnectionsDelegate listenForConnections = new listenForConnectionsDelegate(listen);
            listenForConnections.BeginInvoke(new AsyncCallback(endListen), null);
            #endregion
        }

        //Escutando do Asterisk
        private void readStream(NetworkStream stream)
        {
            String command = "";
            Int32 errorCount = 0;

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
                        ExecutarSBBuffer(lines[i]);

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
        private void listen()
        {
            socket_Server.Start();
            newConnection = socket_Server.AcceptSocket();

            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    newConnection.Receive(buffer);

                    string RecebidoCliente = Encoding.ASCII.GetString(buffer);
                    if (!string.IsNullOrEmpty(RecebidoCliente))
                        SendMessageToAsterisk(RecebidoCliente);
                }
                catch (Exception e)
                {
                    evt.WriteEntry(e.Message);
                    break;
                }
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
                        case "Bridge":
                        case "Hangup":
                        case "NewCallerId":
                        case "QueueMemberPaused":
                        case "QueueMember":
                        case "QueueParams":
                        case "Follows":
                        case "PeerStatus":
                        case "Error":
                            SendMessageToClient(bloco);
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
            SendMessageToAsterisk("Action: Login\nActionID: [Login###]\nUsername: " + usuario_asterisk + "\nSecret: " + senha_asterisk);
        }

        private void SendMessageToAsterisk(String message)
        {
            if (!message.Contains("ActionID:"))
                message += "\nActionID: #" + RandomFactory.Create().Next(100000).ToString() + "#";

            try
            {
                Byte[] data = Encoding.ASCII.GetBytes(message + "\n\n");
                netStreamAsterisk.Write(data, 0, data.Length);
            }
            catch (Exception e)
            {
                evt.WriteEntry(e.Message);
            }
        }

        private void SendMessageToClient(String message)
        {
            if (!message.Contains("ActionID:"))
                message += "\nActionID: #" + RandomFactory.Create().Next(100000).ToString() + "#";

            try
            {
                Byte[] data = Encoding.ASCII.GetBytes(message + "\n\n");
                newConnection.Send(data);
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
