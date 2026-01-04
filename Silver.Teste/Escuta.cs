using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Configuration;
using System.IO;
using Silver.Common;
using System.Text.RegularExpressions;

namespace Silver.Discador
{
    public class Escuta
    {
        private string usuario_asterisk = ConfigurationManager.AppSettings["application.asterisk.user"];
        private string senha_asterisk = ConfigurationManager.AppSettings["application.asterisk.secret"];
        private string ip_asterisk = ConfigurationManager.AppSettings["application.asterisk.ip"];
        private int porta_asterisk = ConfigurationManager.AppSettings["application.asterisk.porta"].ToInt32();

        private TcpClient socket_asterisk;

        private delegate void readStreamDelegate(NetworkStream stream);
        private delegate void readSocketDelegate(Socket socket);

        private NetworkStream netStream;

        public void IniciarConexao()
        {
            socket_asterisk = new TcpClient(ip_asterisk, porta_asterisk);
            netStream = socket_asterisk.GetStream();

            //readStreamDelegate readSteam = new readStreamDelegate(ReadStream);
            //readSteam.BeginInvoke(netStream, new AsyncCallback(ReadStreamEnd), null);
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
                netStream.Write(data, 0, data.Length);
            }
            catch (Exception e)
            {
                SaidaMensagem(e.Message, LoggerType.ERRO);
            }
        }

        private void ReadStream(NetworkStream stream)
        {
            String command = "";
            while (true)
            {
                try
                {
                    Byte[] bytes = new Byte[1024];
                    Int32 received;
                    received = stream.Read(bytes, 0, 1024);
                    new BLL.SaidaAsterisk().Cadastrar(Encoding.ASCII.GetString(bytes, 0, received));

                    if (Regex.Match(command, "(Asterisk Call Manager/)([0-9]*).([0-9]*)\\r\\n", RegexOptions.IgnoreCase).Success)
                    {
                        command = "";
                        Login();
                    }
                }
                catch (Exception e)
                {
                    SaidaMensagem(e.Message, Common.LoggerType.ERRO);
                }
            }
        }

        private void ReadStreamEnd(IAsyncResult result)
        {
        }

        protected void SaidaMensagem(string mensagem, Common.LoggerType tipo_log = Common.LoggerType.INFO)
        {
            string msg = string.Format("[{0}] - {1}", DateTime.Now.ToString(), mensagem);
            string path_logs = Path.Combine(ConfigurationManager.AppSettings["application.path.logs"], string.Format("Logs-{0}_Hora-{1}.log", "ServerEscuta", DateTime.Now.ToString("ddMMyyyyHH")));

            Silver.Common.Logger.LoggerAsync.pathLog = path_logs;
            Silver.Common.Logger.LoggerAsync.WriteLog(new Common.Logger.LoggerMessage { MessageLogger = msg, TypeLogger = tipo_log });

            Console.Out.WriteLine(msg);
        }
    }
}
