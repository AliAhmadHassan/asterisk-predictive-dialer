using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Configuration;
using Silver.Common;
using System.IO;
using System.Text.RegularExpressions;

namespace Silver.AsteriskClient
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 22/10/2013
    /// </summary>
    public abstract class AsteriskBase : IDisposable
    {
        /// <summary>
        /// Verifica se a instância está autenticada no asterisk ami
        /// </summary>
        public bool Autenticado { get; set; }

        /// <summary>
        /// Quantidade de erros ocorrida na conexão com o servidor asterisk
        /// </summary>
        public int QtdErrosInstancia { get; set; }

        private string usuario_asterisk;
        /// <summary>
        /// Usuário para conexão com o Asterisk (Manager)
        /// </summary>
        public string UsuarioAsterisk
        {
            get { return usuario_asterisk; }
            set { usuario_asterisk = value; }
        }

        private string senha_asterisk;
        /// <summary>
        /// Senha do Usuário do Manager do Asterisk
        /// </summary>
        public string SenhaAsterisk
        {
            get { return senha_asterisk; }
            set { senha_asterisk = value; }
        }

        private string ip_asterisk;
        /// <summary>
        /// Ip de conexão com telnet asterisk
        /// </summary>
        public string IpAsterisk
        {
            get { return ip_asterisk; }
            set { ip_asterisk = value; }
        }

        private int porta_asterisk;
        /// <summary>
        /// Porta de conexão com o telnet asterisk
        /// </summary>
        public int PortaAsterisk
        {
            get { return porta_asterisk; }
            set { porta_asterisk = value; }
        }

        // Delegates para escuta assincrona com o Asterisk
        protected delegate void readStreamDelegate(NetworkStream stream);

        //Objeto que se conectará ao telnet do Asterisk
        public TcpClient Socket_Asterisk { get; set; }

        //Objeto que fará a transferencia dos comandos da aplicação para o Asterisk
        public NetworkStream Stream_Asterisk { get; set; }

        /// <summary>
        /// Carrega as configurações padrão de conexão com o asterisk
        /// </summary>
        public AsteriskBase()
        {
            usuario_asterisk = ConfigurationManager.AppSettings["application.asterisk.user"];
            senha_asterisk = ConfigurationManager.AppSettings["application.asterisk.secret"];
            ip_asterisk = ConfigurationManager.AppSettings["application.asterisk.ip"];
            porta_asterisk = ConfigurationManager.AppSettings["application.asterisk.porta"].ToInt32();

            IniciarSocket();
        }

        /// <summary>
        /// Inicia a classe com outras configuraçõoes de usuário do Manager do asterisk
        /// </summary>
        /// <param name="usuario_asterisk">Usuário para Telnet Manager Asterisk</param>
        /// <param name="senha_asterisk">Senha para Telent Manager Asterisk</param>
        /// <param name="ip_asterisk">Ip do servidor Asterisk</param>
        /// <param name="porta_asterisk">Porta do servidor Asterisk</param>
        public AsteriskBase(string usuario_asterisk, string senha_asterisk, string ip_asterisk, int porta_asterisk)
        {
            this.UsuarioAsterisk = usuario_asterisk;
            this.SenhaAsterisk = senha_asterisk;
            this.IpAsterisk = ip_asterisk;
            this.PortaAsterisk = porta_asterisk;

            IniciarSocket();
        }

        /// <summary>
        /// Inicia a conexão com o asterisk no socket e inicia o stream de comando pois será necessário enviar comando de login
        /// </summary>
        protected virtual void IniciarSocket()
        {
            if (Socket_Asterisk != null)
            {
                Socket_Asterisk.Close();
                Socket_Asterisk = null;
            }

            if (Stream_Asterisk != null)
            {
                Stream_Asterisk.Close();
                Stream_Asterisk.Dispose();
                Stream_Asterisk = null;
            }

            //Inicia o socket com uma conexão utilizando as configurações informadas
            Socket_Asterisk = new TcpClient(ip_asterisk, porta_asterisk);
            Socket_Asterisk.ReceiveBufferSize = 1024 * 1024 * 4;
            Socket_Asterisk.ReceiveTimeout = int.MaxValue;
            Socket_Asterisk.SendTimeout = int.MaxValue;

            Socket_Asterisk.Client.DontFragment = true;

            //Socket_Asterisk.Client.EnableBroadcast = true;
            //Socket_Asterisk.Client.MulticastLoopback = false;
            //Socket_Asterisk.NoDelay = true;
            //Socket_Asterisk.Client.Listen(1000);
            //Inicia o stream para envio do comando de login no asterisk
            Stream_Asterisk = Socket_Asterisk.GetStream();

            Login();
        }

        /// <summary>
        /// Envia o comando de login do asterisk mas não aguarda o Retorno
        /// </summary>
        private void Login()
        {
            Byte[] data = Encoding.ASCII.GetBytes("Action: Login\r\nActionID: [Login###]\r\nUsername: " + usuario_asterisk + "\r\nSecret: " + senha_asterisk + "\n\n");
            Byte[] bytes = new Byte[Socket_Asterisk.ReceiveBufferSize];

            Int32 received;
            String[] lines;
            int offset = 0;

            bool autenticado = false;
            while (!autenticado)
            {
                Stream_Asterisk.Write(data, 0, data.Length);
                received = Stream_Asterisk.Read(bytes, offset, bytes.Length);

                string command = Encoding.ASCII.GetString(bytes, 0, received);

                lines = Regex.Split(command, "\\r\\n\\r\\n", RegexOptions.Multiline);

                foreach (var l in lines)
                {
                    if (l.Contains("Message: Authentication accepted"))
                    {
                        autenticado = true;
                        Autenticado = autenticado;
                        break;
                    }
                }

                if (autenticado)
                    break;
            }
        }

        public void LogOff()
        {
            Byte[] data = Encoding.ASCII.GetBytes("Action: Logoff\n\n");
            Stream_Asterisk.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Saida de mensagens da classe
        /// </summary>
        /// <param name="mensagem"></param>
        /// <param name="tipolog"></param>
        protected virtual void SaidaLogs(string mensagem, Common.LoggerType tipolog)
        {
            string mensagem_formatada = string.Format("[{0}] - [{1}] - {2}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff"), tipolog, mensagem);

            string path_logs = Path.Combine(ConfigurationManager.AppSettings["application.path.logs"], string.Format("Logs-{0}_{1}.log", "Discador.AsteriskBase", DateTime.Now.ToString("ddMMyyyyHH")));
            Silver.Common.Logger.LoggerAsync.pathLog = path_logs;
            Silver.Common.Logger.LoggerAsync.WriteLog(new Common.Logger.LoggerMessage { MessageLogger = mensagem_formatada, TypeLogger = tipolog });
            Console.Out.WriteLine(mensagem_formatada);
        }

        /// <summary>
        /// Finaliza o objeto de conexão com o asterisk
        /// </summary>
        public virtual void Dispose()
        {
            try
            {
                if (Socket_Asterisk != null)
                {
                    if (Socket_Asterisk.Connected)
                        LogOff();

                    if (Socket_Asterisk.Client.Connected)
                    {
                        Socket_Asterisk.Client.Shutdown(SocketShutdown.Both);
                        Socket_Asterisk.Client.Disconnect(false);
                        Socket_Asterisk.Client.Close();
                        Socket_Asterisk.Client.Dispose();
                    }

                    Socket_Asterisk = null;
                    if (Stream_Asterisk != null)
                    {
                        Stream_Asterisk.Close();
                        Stream_Asterisk.Dispose();
                    }
                }
            }
            catch { }
        }
    }
}
