using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Silver.Common;
using System.Configuration;
using Silver.AsteriskClient.DTO;
using Silver.DTO;
using System.Threading.Tasks;
using System.IO;

namespace Silver.Discador
{
    public class ClienteAsterisk : IDisposable
    {
        private string usuario_asterisk = ConfigurationManager.AppSettings["application.asterisk.user"];
        private string senha_asterisk = ConfigurationManager.AppSettings["application.asterisk.secret"];
        private string ip_asterisk = ConfigurationManager.AppSettings["application.asterisk.ip"];
        private int porta_asterisk = ConfigurationManager.AppSettings["application.asterisk.porta"].ToInt32();
        private Escuta escuta_asterisk = new Escuta();
        private TcpClient socket_asterisk;

        // Private Delegates
        private delegate void readStreamDelegate(NetworkStream stream);
        private delegate void readSocketDelegate(Socket socket);

        private NetworkStream netStream;

        public void IniciarConexao()
        {
            socket_asterisk = new TcpClient(ip_asterisk, porta_asterisk);
            netStream = socket_asterisk.GetStream();

            readStreamDelegate readSteam = new readStreamDelegate(readStream);
            readSteam.BeginInvoke(netStream, new AsyncCallback(readStreamEnd), null);
            //escuta_asterisk.IniciarConexao();

        }

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
                    //Console.WriteLine("************************ Quantidade Blocos *********************{0}", lines.Length);
                    Task.Factory.StartNew(() => { SaidaMensagem("Leitura de dados -> Total de blocos lidos: " + lines.Length.ToString("000"), Common.LoggerType.INFO); });
                    for (Int32 i = 0; i < lines.Length - 1; i++)
                        ExecutarSBBufferTelNet(lines[i]);

                    command = lines[lines.Length - 1];
                    errorCount -= 1;
                }
                catch (Exception e)
                {
                    SaidaMensagem(e.Message, Silver.Common.LoggerType.ERRO);
                }

                if (errorCount == 10)
                {
                    SaidaMensagem("AMIProxy is no longer accepting messages from the asterisk server so will commence shutdown.", Silver.Common.LoggerType.ERRO);
                    return;
                }
            }
        }

        public void ExecutarSBBufferTelNet(string bloco)
        {
            try
            {
                if ((bloco.Contains('\n')) && (bloco.Contains(':')))
                {
                    var Linhas = bloco.Split('\n');

                    switch (Linhas[0].Split(':')[1].Trim())
                    {
                        case "QueueMember":
                            QueueMember(Linhas);
                            break;

                        case "QueueEntry":
                            QueueEntry(Linhas);
                            break;

                        case "QueueStatusComplete":
                            QueueStatusComplete(Linhas);
                            break;

                        case "QueueParams":
                            QueueParams(Linhas);
                            break;

                        default:
                            break;
                    }
                }
                else if(bloco.Contains("QueueStatusComplete"))
                    QueueStatusComplete(bloco.Split('\n'));

            }
            catch { }
            finally { }
        }

        public void ExecutarSBBufferDB(string bloco)
        {
            try
            {
                if ((bloco.Contains('\n')) && (bloco.Contains(':')))
                {
                    var Linhas = bloco.Split('\n');

                    switch (Linhas[0].Split(':')[1].Trim())
                    {
                        case "Newstate":
                            NewState(Linhas);
                            break;
                        case "Bridge":
                            Bridge(Linhas);
                            break;
                        case "Hangup":
                            HangUp(Linhas);
                            break;
                        case "NewCallerId":
                            NewCallerId(Linhas);
                            break;
                        case "QueueMemberPaused":
                            QueueMemberPaused(Linhas);
                            break;
                        case "Error":
                            Silver.Teste.Program.cliente_asterisk_onError(new AsteriskClientException(Linhas[1].Split(':')[1].Trim()));
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

        protected void SaidaMensagem(string msg, Silver.Common.LoggerType tipo_log)
        {
            string msg_formatada = string.Format("[{0}]:[{1}] - {2}", DateTime.Now.ToString(), Enum.GetName(typeof(Silver.Common.LoggerType), tipo_log), msg);
            string path_logs = Path.Combine(ConfigurationManager.AppSettings["application.path.logs"], string.Format("Logs-{0}_Hora-{1}.log", "Discador", DateTime.Now.ToString("ddMMyyyyHH")));

            Silver.Common.Logger.LoggerAsync.pathLog = path_logs;
            Silver.Common.Logger.LoggerAsync.WriteLog(new Common.Logger.LoggerMessage { MessageLogger = msg_formatada, TypeLogger = tipo_log });

            Console.Out.WriteLine(msg_formatada);
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
                SaidaMensagem(e.Message, Silver.Common.LoggerType.ERRO);
            }
        }

        #region Command Asterisk

        /// <summary>
        /// Medoto para Recarregar algum Modulo de Configuração
        /// </summary>
        /// <param name="Modulo"> Modulo para Recarregar</param>
        /// <returns></returns>
        public void RecarregarArquivoConfiguracaoFilas(AsteriskModulo Modulo)
        {
            var Command = string.Empty;
            switch (Modulo)
            {
                case AsteriskModulo.Ael:
                    Command = "ael reload";
                    break;
                case AsteriskModulo.Sip:
                    Command = "Sip reload";
                    break;
                case AsteriskModulo.DialPlan:
                    Command = "dialplan reload";
                    break;
                case AsteriskModulo.Queue:
                    Command = "queue reload all";
                    break;
                default:
                    Command = "reload";
                    break;
            }

            var command_asterisk = string.Format("Action: Command\r\nCommand:{0}\r\n\r\n", Command);
            SendMessageToAsterisk(command_asterisk);
        }

        public void RamaisStatus()
        {
            var command_asterisk = string.Format("Action: Command\r\nCommand:{0}\r\n\r\n", "sip show inuse");
            SendMessageToAsterisk(command_asterisk);
        }

        public void Discar(params Discagem[] discagens)
        {
            //var prot = ((int)protocolo).ToString();
            var command_asterisk = new StringBuilder();
            foreach (Discagem discagem in discagens)
                command_asterisk.Append(discagem.ToString());

            //var command_asterisk = string.Format("Action: originate\nChannel: DGV/g{0}/{1}\nContext: interno\nExten: start\nPriority: 1\nVariable: Campanha={2}\nVariable: TelefoneID={3}\nVariable: TipoTelefone={4}\nCallerid: {1}\nVariable: IdCampanha={5}\n\n", prot, Telefone, campanha, IdTelefone, TipoTelefone, id_campanha);
            SendMessageToAsterisk(command_asterisk.ToString());
        }

        public void IniciarPausaNaFila(long ramal, string fila, string razao = "Razão não informada")
        {
            var command_asterisk = string.Format("Action: QueuePause\r\nInterface: SIP/{0}\r\nPaused: yes\r\nQueue:{1}\r\nReason: {2}\r\n\r\n", ramal, fila, razao);
            SendMessageToAsterisk(command_asterisk);
        }

        public void StatusFila(string fila)
        {
            //A resposta deste método saira no método 'QueueMember'
            var command_asterisk = string.Format("Action: QueueStatus\r\nQueue:{0}\r\n\r\n", fila);
            SendMessageToAsterisk(command_asterisk);
        }

        public void FinalizarPausaNaFila(long ramal, string fila, string razao = "Razão não informada")
        {
            var command_asterisk = string.Format("Action: QueuePause\r\nInterface: SIP/{0}\r\nPaused: no\r\nQueue:{1}\r\nReason: {2}\r\n\r\n", ramal, fila, razao);
            SendMessageToAsterisk(command_asterisk);
        }

        public void AdicionarOperadorFila(long ramal, string fila)
        {
            var command_asterisk = string.Format("Action: QueueAdd\r\nInterface: SIP/{0}\r\nPaused: no\r\nQueue:{1}\r\nMemberName: {0}\r\n\r\n", ramal, fila);
            SendMessageToAsterisk(command_asterisk);
        }

        public void RemoverOperadorFila(long ramal, string fila)
        {
            var command_asterisk = string.Format("Action: QueueRemove\r\nQueue: {0}\r\nInterface: sip/{1}\r\n\r\n", fila, ramal);
            SendMessageToAsterisk(command_asterisk);

        }
        #endregion

        #region Eventos
        private void QueueMember(string[] Linhas)
        {
            long Location = 0;
            string Queue = string.Empty;
            QueueMemberStatus Status = QueueMemberStatus.AST_DEVICE_UNKNOWN;

            foreach (string Linha in Linhas)
            {
                var linhaSeparado = Linha.Split(':');

                var Chave = linhaSeparado[0].Trim();
                var Valor = linhaSeparado[1].Trim();

                switch (Chave)
                {
                    case "Location":
                        if (!long.TryParse(Valor.ToUpper().Replace("SIP/", ""), out Location))
                        {
                            SaidaMensagem("Erro ao Encontrar Canal SIP. CANAL: {0}", Silver.Common.LoggerType.ERRO);
                            continue;
                        }
                        break;

                    case "Queue":
                        Queue = Valor;
                        break;

                    case "Status":
                        switch (Valor.ToInt16())
                        {
                            case 0:
                                Status = QueueMemberStatus.AST_DEVICE_UNKNOWN;
                                break;
                            case 1:
                                Status = QueueMemberStatus.AST_DEVICE_NOT_INUSE;
                                break;
                            case 2:
                                Status = QueueMemberStatus.AST_DEVICE_INUSE;
                                break;
                            case 3:
                                Status = QueueMemberStatus.AST_DEVICE_BUSY;
                                break;
                            case 4:
                                Status = QueueMemberStatus.AST_DEVICE_INVALID;
                                break;
                            case 5:
                                Status = QueueMemberStatus.AST_DEVICE_UNAVAILABLE;
                                break;
                            case 6:
                                Status = QueueMemberStatus.AST_DEVICE_RINGING;
                                break;
                            case 7:
                                Status = QueueMemberStatus.AST_DEVICE_RINGINUSE;
                                break;
                            case 8:
                                Status = QueueMemberStatus.AST_DEVICE_ONHOLD;
                                break;

                            default:
                                Silver.Teste.Program.cliente_asterisk_onError(new AsteriskClientException(string.Format("Queue Member Status não definido para {0}", Valor)));
                                break;

                        }
                        break;
                }
            }

            Silver.Teste.Program.cliente_asterisk_onQueueMember(Location, Queue, Status);
        }

        private void QueueParams(string[] Linhas)
        {
            QueueParamsEntity queueParamsEntity = new QueueParamsEntity();
            foreach (string Linha in Linhas)
            {
                var linhaSeparado = Linha.Split(':');

                var Chave = linhaSeparado[0].Trim();
                var Valor = linhaSeparado[1].Trim();

                switch (Chave)
                {
                    case "Queue": queueParamsEntity.Queue = Valor;
                        break;
                    case "Max": queueParamsEntity.Max = Valor.ToInt32();
                        break;
                    case "Strategy": queueParamsEntity.Strategy = Valor;
                        break;
                    case "Calls": queueParamsEntity.Calls = Valor.ToInt32();
                        break;
                    case "Holdtime": queueParamsEntity.Holdtime = Valor.ToInt32();
                        break;
                    case "TalkTime": queueParamsEntity.TalkTime = Valor.ToInt32();
                        break;
                    case "Completed": queueParamsEntity.Completed = Valor.ToInt32();
                        break;
                    case "Abandoned": queueParamsEntity.Abandoned = Valor.ToInt32();
                        break;
                    case "ServiceLevel": queueParamsEntity.ServiceLevel = Valor.ToInt32();
                        break;
                    case "ServicelevelPerf": queueParamsEntity.ServicelevelPerf = Valor.ToDecimal();
                        break;
                    case "Weight": queueParamsEntity.Weight = Valor.ToInt32();
                        break;
                }
            }
            Silver.Teste.Program.cliente_asterisk_onQueueParams(queueParamsEntity);
        }

        private void QueueEntry(string[] Linhas)
        {
            DTO.RelCampanhaQueueStatus _RelCampanhaQueueStatus = new RelCampanhaQueueStatus();

            foreach (string Linha in Linhas)
            {
                var linhaSeparado = Linha.Split(':');

                var Chave = linhaSeparado[0].Trim();
                var Valor = linhaSeparado[1].Trim();

                switch (Chave)
                {
                    case "Channel":
                        _RelCampanhaQueueStatus.Channel = Valor;
                        break;

                    case "CallerIDNum":
                        _RelCampanhaQueueStatus.CallerIDNum = Valor;
                        break;

                    case "Wait":
                        _RelCampanhaQueueStatus.Wait = Valor.ToInt32();
                        break;
                }
            }

            Silver.Teste.Program.cliente_asterisk_onQueueEntry(_RelCampanhaQueueStatus);
        }

        private void QueueStatusComplete(string[] Linhas)
        {
            Silver.Teste.Program.cliente_asterisk_onQueueStatusComplete();
        }

        /// <summary>
        /// Eventos relacionado a QueueMemberPaused
        /// </summary>
        /// <param name="Linhas">Array com as Linhas do Bloco de Evento</param>
        private void QueueMemberPaused(string[] Linhas)
        {
            long Location = 0;
            var Pausa = true;

            foreach (string Linha in Linhas)
            {
                var linhaSeparado = Linha.Split(':');

                var Chave = linhaSeparado[0].Trim();
                var Valor = linhaSeparado[1].Trim();

                switch (Chave)
                {
                    case "Location":
                        if (!long.TryParse(Valor.ToLower().Replace("sip/", ""), out Location))
                        {
                            throw new AsteriskClientException("Erro ao Encontrar Canal SIP");
                        }
                        break;

                    case "Paused":
                        if (Valor == "1")
                        {
                            Pausa = true;
                        }
                        else
                        {
                            Pausa = false;
                        }
                        break;
                }
            }

            if (Pausa)
            {
                Silver.Teste.Program.cliente_asterisk_onEntrouEmPausa(Location);
            }
            else
            {
                Silver.Teste.Program.cliente_asterisk_onSaiuDePausa(Location);
            }
        }

        /// <summary>
        /// Eventos relacionado ao Estado da Ligação
        /// </summary>
        /// <param name="Linhas">Array com as Linhas do Bloco de Evento</param>
        private void NewState(string[] Linhas)
        {
            var Channel = string.Empty;
            var CallerIDNum = string.Empty;
            var ChannelState = 0;
            var ConnectedLineNum = string.Empty;


            foreach (string Linha in Linhas)
            {
                var linhaSeparado = Linha.Split(':');

                var Chave = linhaSeparado[0].Trim();
                var Valor = linhaSeparado[1].Trim();

                switch (Chave)
                {
                    case "Channel":
                        Channel = Valor;
                        break;

                    case "ChannelState":
                        if (!int.TryParse(Valor, out ChannelState))
                        {
                            throw new AsteriskClientException("Erro ao Converter ChannelState");
                        }
                        break;

                    case "CallerIDNum":
                        CallerIDNum = Valor;
                        break;

                    case "ConnectedLineNum":
                        ConnectedLineNum = Valor;
                        break;
                }
            }

            if (Channel.Contains("SIP"))
            {
                long Ramal = 0;
                if (!long.TryParse(Channel.Substring(4, Channel.IndexOf('-') - 4), out Ramal))
                {
                    throw new AsteriskClientException("Erro ao Encontrar Canal SIP");
                }
                switch (ChannelState)
                {
                    case 6:
                        //RamalAtendeu(Ramal, ConnectedLineNum);
                        break;


                    case 5:
                        Silver.Teste.Program.cliente_asterisk_onRamalChamando(Ramal);
                        break;

                    case 4:
                        Silver.Teste.Program.cliente_asterisk_onRamalChamando(Ramal);
                        break;

                    case 3:
                        Silver.Teste.Program.cliente_asterisk_onRamalDiscando(Ramal);
                        break;
                }
            }
            else
            {
                switch (ChannelState)
                {
                    case 6:
                        Silver.Teste.Program.cliente_asterisk_onDestinoAtendeu(CallerIDNum);
                        break;

                    case 5:
                        Silver.Teste.Program.cliente_asterisk_onDestinoChamando(CallerIDNum);
                        break;

                    case 4:
                        Silver.Teste.Program.cliente_asterisk_onDestinoChamando(CallerIDNum);
                        break;

                    case 3:
                        Silver.Teste.Program.cliente_asterisk_onDestinoDiscando(CallerIDNum);
                        break;
                }
            }
        }

        private void Bridge(string[] Linhas)
        {
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

            Silver.Teste.Program.cliente_asterisk_onRamalAtendeu(Channel2.Substring(4, Channel2.IndexOf('-') - 4).ToInt64(), CallerID1);

        }

        /// <summary>
        /// Eventos relacionado ao Desligamento da Ligação
        /// </summary>
        /// <param name="Linhas">Array com as Linhas do Bloco de Evento</param>
        private void HangUp(string[] Linhas)
        {
            var Channel = string.Empty;
            var CallerIDNum = string.Empty;
            var Cause = 0;

            foreach (string Linha in Linhas)
            {
                var linhaSeparado = Linha.Split(':');

                var Chave = linhaSeparado[0].Trim();
                var Valor = linhaSeparado[1].Trim();

                switch (Chave)
                {
                    case "Channel":
                        Channel = Valor;
                        break;

                    case "Cause":
                        if (!int.TryParse(Valor, out Cause))
                        {
                            throw new AsteriskClientException("Erro ao Converter Causa do Desligamento");
                        }
                        break;

                    case "CallerIDNum":
                        CallerIDNum = Valor;
                        break;
                }
            }


            if (Channel.Contains("SIP"))
            {
                long Ramal = 0;
                if (!long.TryParse(Channel.Substring(4, Channel.IndexOf('-') - 4), out Ramal))
                {
                    throw new AsteriskClientException("Erro ao Encontrar Canal SIP");
                }
                switch ((CausesDisconnection)Cause)
                {
                    case CausesDisconnection.AST_CAUSE_USER_BUSY:
                        Silver.Teste.Program.cliente_asterisk_onRamalOcupado(Ramal);
                        break;
                    case CausesDisconnection.AST_CAUSE_NO_ANSWER:
                        Silver.Teste.Program.cliente_asterisk_onRamalNaoAtendeu(Ramal);
                        break;
                    default:
                        Silver.Teste.Program.cliente_asterisk_onRamalDesligou(Ramal, Enum.GetName(typeof(CausesDisconnection), Cause));
                        break;
                }
            }
            else
            {
                switch ((CausesDisconnection)Cause)
                {
                    case CausesDisconnection.AST_CAUSE_USER_BUSY:
                        Silver.Teste.Program.cliente_asterisk_onDestinoOcupado(CallerIDNum);
                        break;
                    case CausesDisconnection.AST_CAUSE_NO_ANSWER:
                        Silver.Teste.Program.cliente_asterisk_onDestinoNaoAtendeu(CallerIDNum);
                        break;
                    default:
                        Silver.Teste.Program.cliente_asterisk_onDestinoDesligou(CallerIDNum, Enum.GetName(typeof(CausesDisconnection), Cause));
                        break;
                }
            }
        }

        /// <summary>
        /// Eventos relacionado ao Id de Ligação
        ///
        /// Não Implementado por falta de Utilidade no Momento
        /// </summary>
        /// <param name="Linhas">Array com as Linhas do Bloco de Evento</param>
        private void NewCallerId(string[] Linhas)
        {
        }
        #endregion

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
}
