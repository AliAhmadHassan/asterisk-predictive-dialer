using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net.Sockets;
using Silver.Common;
using System.IO;

namespace Silver.Discador
{
    public class AsteriskCommand : IDisposable
    {
        private string usuario_asterisk = ConfigurationManager.AppSettings["application.asterisk.user"];
        public string UsuarioAsterisk
        {
            get { return usuario_asterisk; }
            set { usuario_asterisk = value; }
        }

        private string senha_asterisk = ConfigurationManager.AppSettings["application.asterisk.secret"];
        public string SenhaAsterisk
        {
            get { return senha_asterisk; }
            set { senha_asterisk = value; }
        }

        private string ip_asterisk = ConfigurationManager.AppSettings["application.asterisk.ip"];
        public string IpAsterisk
        {
            get { return ip_asterisk; }
            set { ip_asterisk = value; }
        }

        private int porta_asterisk = ConfigurationManager.AppSettings["application.asterisk.porta"].ToInt32();
        public int PortaAsterisk
        {
            get { return porta_asterisk; }
            set { porta_asterisk = value; }
        }

        private TcpClient socket_asterisk;
        private NetworkStream net_stream;

        public AsteriskCommand() { }

        public void Discar(DTO.Discagem discagem)
        {
            Conectar();
            var comando = Login() + discagem.ToString() + Logoff();
            EnviarComando(comando);
        }

        private void Conectar()
        {
            try
            {
                socket_asterisk = new TcpClient(ip_asterisk, porta_asterisk);
                net_stream = socket_asterisk.GetStream();
            }
            catch (Exception ex)
            {
                SaidaMensagem(ex.Message, LoggerType.ERRO);
                throw;
            }
        }

        private string Login()
        {
            return string.Format("Action: Login\r\nActionID: [Login###]\r\nUsername: {0}\r\nSecret: {1}\r\n\r\n", usuario_asterisk, senha_asterisk);
        }

        private string Logoff()
        {
            return "Action:Logoff\r\n\r\n";
        }

        private void EnviarComando(String message)
        {
            try
            {
                Byte[] data = Encoding.ASCII.GetBytes(message);
                net_stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                SaidaMensagem(ex.Message, LoggerType.ERRO);
                throw;
            }
        }

        public void Dispose()
        {
            try
            {
                if (net_stream != null)
                {
                    net_stream.Close();
                    net_stream.Dispose();
                }

                if (socket_asterisk != null)
                    socket_asterisk.Close();
            }
            catch (Exception ex)
            {
                SaidaMensagem(ex.Message, LoggerType.ERRO);
            }
        }

        public void SaidaMensagem(string mensagem, Silver.Common.LoggerType tipo_log)
        {
            string mensagem_formatada = string.Format("[{0}] - {1}", DateTime.Now.ToString(), mensagem);
            string path_logs = Path.Combine(ConfigurationManager.AppSettings["application.path.logs"], string.Format("Logs-{0}_Hora-{1}.log", "ServerEscuta", DateTime.Now.ToString("ddMMyyyyHH")));

            Silver.Common.Logger.LoggerAsync.pathLog = path_logs;
            Silver.Common.Logger.LoggerAsync.WriteLog(new Common.Logger.LoggerMessage { MessageLogger = mensagem_formatada, TypeLogger = tipo_log });

            Console.Out.WriteLine(mensagem_formatada);
        }
    }
}
