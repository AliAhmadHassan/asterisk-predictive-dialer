using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Configuration;
using Silver.AsteriskClient.DTO;
using System.Threading.Tasks;
using System.IO;

namespace Silver.AsteriskClient
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 22/10/2013
    /// </summary>
    public class AsteriskListener : AsteriskBase
    {
        #region Delegate

        /// <summary>
        /// Define onde a escuta armazenara o que está ouvindo, se no banco de dados ou  em memória
        /// </summary>
        public SaidaPadraoAsterisk SaidaPadrao { get; set; }

        public delegate void LogoffAsterisk();

        public event LogoffAsterisk OnLogoffAsterisk;

        /// <summary>
        /// Informamos o método que fará a atualização dos telefones
        /// </summary>
        public Func<string, string> ConsultaTelefone { get; set; }

        /// <summary>
        /// Método que grava as saidas do asterisk na base de dados
        /// </summary>
        public Action<string> GravaSaidasAsterisk { get; set; }

        /// <summary>
        /// Delegate para erros com asterisk
        /// </summary>
        public Action<AsteriskClientException> OnErro { get; set; }

        /// <summary>
        /// Delegate para o evento QueueMemberStatus
        /// </summary>
        public Action<long, string, QueueMemberStatus> OnQueueMemberStatus { get; set; }

        /// <summary>
        /// Delegate para o evento QueueParam 
        /// </summary>
        public Action<QueueParamsEntity> OnQueueParams { get; set; }

        /// <summary>
        /// Delegate para o evento OnQueueEntry
        /// </summary>
        public Action<RelCampanhaQueueStatus> OnQueueEntry { get; set; }

        /// <summary>
        /// Delegate para o evento OnQueueStatusComplete
        /// </summary>
        public Action OnQueueStatusComplete { get; set; }

        /// <summary>
        /// Delegate para o evento OnEntrouEmPausa
        /// </summary>
        public Action<long> OnEntrouEmPausa { get; set; }

        /// <summary>
        /// Delegate para o evento OnSaiuDePausa
        /// </summary>
        public Action<long> OnSaiuDePausa { get; set; }

        /// <summary>
        /// Delegate para o evento OnRamalChamando
        /// </summary>
        public Action<long> OnRamalChamando { get; set; }

        /// <summary>
        /// Delegate para o evento OnRamalDiscando
        /// </summary>
        public Action<long> OnRamalDiscando { get; set; }

        /// <summary>
        /// Delegate para o evento OnDestinoAtendeu
        /// </summary>
        public Action<string> OnDestinoAtendeu { get; set; }

        /// <summary>
        /// Delegate para o evento OnDestinoChamando;
        /// </summary>
        public Action<string> OnDestinoChamando { get; set; }

        /// <summary>
        /// Delegate para o evento OnDestionDiscando;
        /// </summary>
        public Action<string> OnDestinoDiscando { get; set; }

        /// <summary>
        /// Delegate para o evento OnRamalAtendeu; 
        /// </summary>
        public Action<long, string> OnRamalAtendeu { get; set; }

        /// <summary>
        /// Delegate para o evento OnRamalOcupado;
        /// </summary>
        public Action<long> OnRamalOcupado { get; set; }

        /// <summary>
        /// Delegate para o evento OnRamalNaoAtendeu
        /// </summary>
        public Action<long> OnRamalNaoAtendeu { get; set; }

        /// <summary>
        /// Delegate para o evento OnDestinoOcupado;
        /// </summary>
        public Action<string> OnDestinoOcupado { get; set; }

        /// <summary>
        /// Delegate para o evento OnDestinoNaoAtendeu;
        /// </summary>
        public Action<string> OnDestinoNaoAtendeu { get; set; }

        /// <summary>
        /// Delegate para o evento OnDestinoDesligou;
        /// </summary>
        public Action<string, string> OnDestinoDesligou { get; set; }

        /// <summary>
        /// Delegate para o evento OnRamalDesligou;
        /// </summary>
        public Action<long, string> OnRamalDesligou { get; set; }

        /// <summary>
        /// Delegate para o evento OnQueueShow
        /// </summary>
        public Action<string> OnQueueShow { get; set; }

        /// <summary>
        /// Delegate para o evento OnDGVShowChannels
        /// </summary>
        public Action<string> OnDGVShowChannels { get; set; }

        #endregion

        public AsteriskListener()
            : base()
        {

        }

        public AsteriskListener(string usuario_asterisk, string senha_asterisk, string ip_asterisk, int porta_asterisk, NetworkStream stream_socket)
            : base(usuario_asterisk, senha_asterisk, ip_asterisk, porta_asterisk)
        {
            base.Stream_Asterisk = stream_socket;
        }

        /// <summary>
        /// Definição do Delegate para escuta assíncrona com o Asterisk
        /// </summary>
        /// <param name="stream"></param>
        private delegate void DelegateStreamLeitura(NetworkStream stream);

        /// <summary>
        /// Instância do delegate para execução assincrona
        /// </summary>
        private DelegateStreamLeitura StreamLeitura;

        /// <summary>
        /// 
        /// </summary>
        public void IniciarEscuta()
        {
            try
            {
                base.IniciarSocket();
                if (StreamLeitura != null)
                    StreamLeitura = null;

                StreamLeitura = new DelegateStreamLeitura(EscutarSocket);
                StreamLeitura.BeginInvoke(Stream_Asterisk, new AsyncCallback(EscutaSocketFinalizada), null);

            }
            catch (Exception e)
            {
                SaidaLogs("IniciarEscuta - Falha ao iniciar escuta com o Asterisk, a mensagem do erro foi: " + e.Message, Common.LoggerType.ERRO);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void IniciarEscuta(NetworkStream stream_socket)
        {
            base.Stream_Asterisk = stream_socket;
            StreamLeitura = new DelegateStreamLeitura(EscutarSocket);
            StreamLeitura.BeginInvoke(Stream_Asterisk, new AsyncCallback(EscutaSocketFinalizada), null);
        }

        private void EscutaSocketFinalizada(IAsyncResult result)
        {
        }

        private void DebugOutputTelentAsterisk(string text_saida)
        {
            using (MySql.Data.MySqlClient.MySqlConnection ctx = new MySql.Data.MySqlClient.MySqlConnection("Server=192.168.21.122;Database=silver;Uid=root;Pwd=Admin357/;"))
            {
                string query = @"insert into saidaasterisk_c(valor) values(@Valor)";
                ctx.Open();

                using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, ctx))
                {
                    cmd.Parameters.AddWithValue("@Valor", text_saida);
                    cmd.ExecuteNonQuery();
                }
                ctx.Close();
            }
        }

        private void EscutarSocket(NetworkStream stream)
        {
            String command = "";
            Int32 errorCount = 0;

            while (true)
            {
                try
                {
                    errorCount += 1;

                    Byte[] buffer = new Byte[Socket_Asterisk.ReceiveBufferSize];
                    Int32 received;
                    String[] lines;
                    int offset = 0;

                    received = stream.Read(buffer, offset, buffer.Length);
                    command += Encoding.ASCII.GetString(buffer, offset, received);

                    using (StreamWriter sw = new StreamWriter(string.Format(@"C:\Silver\Logs\output_asterisk_console.txt", DateTime.Now.ToString("ddMMyyyyHH")), true))
                        sw.Write(command);

                    //Task.Factory.StartNew(() => DebugOutputTelentAsterisk(command));

                    if (command.ToLower().Contains("goodbye") || command.ToLower().Contains("thanks for all"))
                    {
                        if (OnLogoffAsterisk != null)
                            OnLogoffAsterisk();

                        return;
                    }

                    if (Regex.Match(command, "(Asterisk Call Manager/)([0-9]*).([0-9]*)\\r\\n", RegexOptions.IgnoreCase).Success)
                    {
                        Autenticado = true;
                        command = "";
                    }

                    lines = Regex.Split(command, "\\r\\n\\r\\n", RegexOptions.Multiline);
                    for (Int32 i = 0; i < lines.Length - 1; i++)
                    {
                        switch (SaidaPadrao)
                        {
                            case SaidaPadraoAsterisk.Delegate:
                                ExecutarSBBufferTelNet(lines[i]);
                                break;
                            case SaidaPadraoAsterisk.BancoDados:
                                ExecutarSBBufferGravacao(lines[i]);
                                break;
                            case SaidaPadraoAsterisk.Debug:
                                Console.Out.WriteLine(lines[i]);
                                break;
                            case SaidaPadraoAsterisk.BancoDadosDelegate:
                                ExecutarSBBufferTelNet(lines[i]);
                                ExecutarSBBufferGravacao(lines[i]);
                                break;
                            case SaidaPadraoAsterisk.BancoDadosDebug:
                                Console.Out.WriteLine(lines[i] + "\n");
                                ExecutarSBBufferGravacao(lines[i]);
                                break;
                            case SaidaPadraoAsterisk.Nenhum:
                                break;
                            default:
                                break;
                        }
                    }

                    command = lines[lines.Length - 1];
                    errorCount -= 1;
                }

                catch (ObjectDisposedException e)
                {
                    //SaidaLogs("Objeto NetWorkStream não instanciado. Mensagem do sistema: " + e.Message, Common.LoggerType.INFO);
                    return;
                }
                catch (Exception e)
                {
                    //SaidaLogs(e.Message + Environment.NewLine + "EscutarSocket - NOTA DO DESENVOLVEDOR: Verificar a necessidade de restaura o status do socket", Common.LoggerType.ERRO);
                }
                if (errorCount == 10)
                {
                    //SaidaLogs("EscutarSocket - A quantidade de tentativas de conexão com o servidor Asterisk excedeu a 10x", Common.LoggerType.ERRO);
                    return;
                }
            }
        }

        public void ExecutarSBBufferGravacao(string bloco)
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
                        GravaSaidasAsterisk(bloco);
                        break;
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

                if (bloco.Contains("QueueStatusComplete"))
                    QueueStatusComplete(bloco.Split('\n'));

                else if (bloco.Contains("List of available DigiVoice Channels") && bloco.Contains("--END COMMAND--"))
                {
                    string bloco_string = string.Empty;

                    foreach (string s in bloco.Split('\n'))
                        if (s.Length == 91)
                            bloco_string += s + "\n";

                    OnDGVShowChannels(bloco_string);
                }
                else if (bloco.Contains("--END COMMAND--"))
                {
                    string[] bloco_queueshow = bloco.Split('\n');
                    string bloco_string = string.Empty;

                    foreach (string s in bloco_queueshow)
                        if (s.Length > 19)
                            bloco_string += s + "\n";

                    OnQueueShow(bloco_string);
                }
                else if (bloco.Contains("Response: Goodbye"))
                {
                    base.Socket_Asterisk.Client.Disconnect(false);
                }
            }
            catch { throw; }
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
                            OnErro(new AsteriskClientException(Linhas[1].Split(':')[1].Trim()));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch { }
            finally { }
        }

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
                            SaidaLogs("Erro ao Encontrar Canal SIP. CANAL: {0}", Silver.Common.LoggerType.ERRO);
                            continue;
                        }
                        break;

                    case "Queue":
                        Queue = Valor;
                        break;

                    case "Status":
                        switch (Convert.ToInt16(Valor))
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
                                if (OnErro != null)
                                    OnErro(new AsteriskClientException(string.Format("Queue Member Status não definido para {0}", Valor)));

                                break;
                        }
                        break;
                    case "Paused":
                        if (Valor.Equals("1"))
                            Status = QueueMemberStatus.AST_DEVICE_PAUSED;

                        break;
                }
            }

            if (OnQueueMemberStatus != null)
                OnQueueMemberStatus(Location, Queue, Status);
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
                    case "Max": queueParamsEntity.Max = Convert.ToInt32(Valor);
                        break;
                    case "Strategy": queueParamsEntity.Strategy = Valor;
                        break;
                    case "Calls": queueParamsEntity.Calls = Convert.ToInt32(Valor);
                        break;
                    case "Holdtime": queueParamsEntity.Holdtime = Convert.ToInt32(Valor);
                        break;
                    case "TalkTime": queueParamsEntity.TalkTime = Convert.ToInt32(Valor);
                        break;
                    case "Completed": queueParamsEntity.Completed = Convert.ToInt32(Valor);
                        break;
                    case "Abandoned": queueParamsEntity.Abandoned = Convert.ToInt32(Valor);
                        break;
                    case "ServiceLevel": queueParamsEntity.ServiceLevel = Convert.ToInt32(Valor);
                        break;
                    case "ServicelevelPerf": queueParamsEntity.ServicelevelPerf = Convert.ToDecimal(Valor);
                        break;
                    case "Weight": queueParamsEntity.Weight = Convert.ToInt32(Valor);
                        break;
                }
            }

            if (OnQueueParams != null)
                OnQueueParams(queueParamsEntity);
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
                        _RelCampanhaQueueStatus.Wait = Convert.ToInt32(Valor);
                        break;
                }
            }

            if (OnQueueEntry != null)
                OnQueueEntry(_RelCampanhaQueueStatus);
        }

        private void QueueStatusComplete(string[] Linhas)
        {
            if (OnQueueStatusComplete != null)
                OnQueueStatusComplete();
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
                            throw new AsteriskClientException("Erro ao Encontrar Canal SIP");
                        break;
                    case "Paused":
                        if (Valor == "1")
                            Pausa = true;
                        else
                            Pausa = false;
                        break;
                }
            }

            if (Pausa)
            {
                if (OnEntrouEmPausa != null)
                    OnEntrouEmPausa(Location);
            }
            else
            {
                if (OnSaiuDePausa != null)
                    OnSaiuDePausa(Location);
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
                            throw new AsteriskClientException("Erro ao Converter ChannelState");
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
                    throw new AsteriskClientException("Erro ao Encontrar Canal SIP");

                switch (ChannelState)
                {
                    case 6:
                        break;
                    case 5:
                        if (OnRamalChamando != null)
                            OnRamalChamando(Ramal);
                        break;
                    case 4:
                        if (OnRamalChamando != null)
                            OnRamalChamando(Ramal);
                        break;
                    case 3:
                        if (OnRamalDiscando != null)
                            OnRamalDiscando(Ramal);
                        break;
                }
            }
            else
            {
                switch (ChannelState)
                {
                    case 6:
                        if (OnDestinoAtendeu != null)
                            OnDestinoAtendeu(CallerIDNum);
                        break;
                    case 5:
                        if (OnDestinoChamando != null)
                            OnDestinoChamando(CallerIDNum);
                        break;
                    case 4:
                        if (OnDestinoChamando != null)
                            OnDestinoChamando(CallerIDNum);
                        break;
                    case 3:
                        if (OnDestinoDiscando != null)
                            OnDestinoDiscando(CallerIDNum);
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

            if (OnRamalAtendeu != null)
                OnRamalAtendeu(Convert.ToInt64(Channel2.Substring(4, Channel2.IndexOf('-') - 4)), CallerID1);

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
                        if (OnRamalOcupado != null)
                            OnRamalOcupado(Ramal);
                        break;
                    case CausesDisconnection.AST_CAUSE_NO_ANSWER:
                        if (OnRamalNaoAtendeu != null)
                            OnRamalNaoAtendeu(Ramal);
                        break;
                    default:
                        if (OnRamalDesligou != null)
                            OnRamalDesligou(Ramal, Enum.GetName(typeof(CausesDisconnection), Cause));
                        break;
                }
            }
            else
            {
                switch ((CausesDisconnection)Cause)
                {
                    case CausesDisconnection.AST_CAUSE_USER_BUSY:
                        if (OnDestinoOcupado != null)
                            OnDestinoOcupado(CallerIDNum);
                        break;
                    case CausesDisconnection.AST_CAUSE_NO_ANSWER:
                        if (OnDestinoNaoAtendeu != null)
                            OnDestinoNaoAtendeu(CallerIDNum);
                        break;
                    default:
                        if (OnDestinoDesligou != null)
                            OnDestinoDesligou(CallerIDNum, Enum.GetName(typeof(CausesDisconnection), Cause));
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
    }
}
