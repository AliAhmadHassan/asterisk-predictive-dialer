using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Silver.AsteriskClient;
using Silver.AsteriskClient.DTO;
using System.Threading.Tasks;
using Silver.Common;
using System.Xml.Serialization;
using System.Runtime.Remoting.Messaging;
using Silver.DTO;
using Silver.ProxyDiscagem.Server.DTO;

namespace Silver.ProxyDiscagem.Server
{
    class Program
    {
        public delegate void SolicitacaoNovaLigacao();
        public delegate void AtualizarCanaisLivres();

        /// <summary>
        /// Delegate para atualização dos canais disponíveis
        /// </summary>
        static AtualizarCanaisLivres servico_atualizacao_canais_livres;

        /// <summary>
        /// Evento que informa se existem novas solicitações de discagens
        /// </summary>
        static event SolicitacaoNovaLigacao OnSolicitacaoNovaLigacao;

        /// <summary>
        /// Informa se o discador está discando as solicitações
        /// </summary>
        static bool processando_solicitacoes = false;

        /// <summary>
        /// Servidor de Conexão TCP
        /// </summary>
        static ProxyServer servidor;

        /// <summary>
        /// Objetos para a consulta de canais disponiveis
        /// </summary>
        static AsteriskListener ast_escuta_consulta_canais;
        static AsteriskCommand ast_comando_consulta_canais;

        /// <summary>
        /// Objetos para o envio das discagems ao asterisk
        /// </summary>
        static AsteriskCommand ast_comando_solicita_discagem;
        static AsteriskListener ast_escuta_solicita_discagem;

        /// <summary>
        /// Enfileiramento de campanhas e solicitações
        /// </summary>
        static List<CampanhaDiscagem> FilasDiscagem = new List<CampanhaDiscagem>();

        /// <summary>
        /// Grupos e canais disponíveis
        /// </summary>
        static List<DTO.Channels> GruposCanais = new List<DTO.Channels>();

        static void CabecalhoServidorProxy()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Out.WriteLine("#".PadRight(100, '#'));
            Console.Out.WriteLine("#\tProxy de Discagem Discador Silver".ToUpper().PadRight(99, ' '));
            Console.Out.WriteLine("#\tInicio das atividades: {0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff".PadRight(99, ' ')));
            Console.Out.WriteLine("#\tVersão: 00.00.03".PadRight(99, ' '));
            Console.Out.WriteLine("#".PadRight(100, '#'));
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void Main(string[] args)
        {
            do
            {
                try
                {
                    Mutex mu = null;
                    try
                    {
                        mu = Mutex.OpenExisting("Silver.ProxyDiscagem.Server");
                    }
                    catch (WaitHandleCannotBeOpenedException) { }

                    if (mu == null)
                        mu = new Mutex(true, "Silver.ProxyDiscagem.Server");
                    else
                    {
                        mu.Close();
                        return;
                    }

                    OnSolicitacaoNovaLigacao += Program_OnSolicitacaoNovaLigacao;
                    CabecalhoServidorProxy();

                    AtualizaRelChannels = DateTime.Now;

                    ConfigurarComunicacaoAsterisk();
                    ast_comando_consulta_canais.DGVShowChannels();

                    while (RelChannelsAtualizado == false) Thread.Sleep(50);

                    ListaDDD11();

                    servidor = new ProxyServer();
                    servidor.OnSolicitacaoDiscagem += servidor_OnSolicitacaoDiscagem;
                    servidor.IniciarServico();

                    MensagemLog("Aguardando requisições", Silver.Common.LoggerType.INFO, ConsoleColor.White);

                    servico_atualizacao_canais_livres = new AtualizarCanaisLivres(BeginAtualizarCanais);
                    servico_atualizacao_canais_livres.BeginInvoke(EndAtualizarCanais, null);

                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    MensagemLog(string.Format("Falha na aplicação. A mensagem do sistema foi: {0}. Stack Trace: {1}", ex.Message, ex.StackTrace).ToUpper(), Silver.Common.LoggerType.ERRO, ConsoleColor.White);
                }
            } while (true);
        }

        static void ConfigurarComunicacaoAsterisk()
        {
            int contador_tentativas_conexao = 1;
            bool conexao_estabelecida = false;
            do
            {
                try
                {
                    if (ast_comando_consulta_canais != null)
                        ast_comando_consulta_canais.Dispose();

                    if (ast_escuta_consulta_canais != null)
                        ast_escuta_consulta_canais.Dispose();

                    ast_escuta_consulta_canais = null;
                    ast_comando_consulta_canais = null;

                    ast_escuta_consulta_canais = new AsteriskListener();
                    ast_comando_consulta_canais = new AsteriskCommand();

                    ast_escuta_consulta_canais.SaidaPadrao = SaidaPadraoAsterisk.Delegate;
                    ast_escuta_consulta_canais.OnDGVShowChannels = OnDGVShowChannels;
                    ast_escuta_consulta_canais.IniciarEscuta(ast_comando_consulta_canais.Stream_Asterisk);

                    if (ast_comando_solicita_discagem != null)
                        ast_comando_solicita_discagem.Dispose();

                    if (ast_escuta_solicita_discagem != null)
                        ast_escuta_solicita_discagem.Dispose();

                    ast_escuta_solicita_discagem = null;

                    ast_escuta_solicita_discagem = new AsteriskListener();
                    ast_comando_solicita_discagem = new AsteriskCommand();

                    ast_escuta_solicita_discagem.SaidaPadrao = SaidaPadraoAsterisk.Delegate;
                    ast_escuta_solicita_discagem.IniciarEscuta(ast_comando_solicita_discagem.Stream_Asterisk);

                    ast_comando_solicita_discagem = null;
                    ast_comando_solicita_discagem = new AsteriskCommand();


                    conexao_estabelecida = true;
                }
                catch (Exception ex)
                {
                    MensagemLog(string.Format("Falha na aplicação! A mensagem do sistema foi: {0}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace), Silver.Common.LoggerType.ERRO, ConsoleColor.Red);
                    ast_escuta_consulta_canais = null;
                    ast_comando_consulta_canais = null;
                    conexao_estabelecida = false;
                    contador_tentativas_conexao++;
                    System.Threading.Thread.Sleep(1000);
                }

                if (conexao_estabelecida)
                    break;

                if (contador_tentativas_conexao > 5)
                {
                    MensagemLog(string.Format("Não foi possível estabelecer uma conexão com o Servidor do Asterisk. Tentativas de conexão excedeu a 5x"), Silver.Common.LoggerType.ERRO, ConsoleColor.Red);
                    return;
                }

            } while (true);
        }

        static void Program_OnSolicitacaoNovaLigacao()
        {
            if (processando_solicitacoes) return;

            processando_solicitacoes = true;
            Discar();
        }

        static void AdicionarSolicitacaoNaFila(Silver.DTO.SolicitacaoDiscagem solicitacao)
        {
            Monitor.Enter(FilasDiscagem);
            try
            {
                var fila = FilasDiscagem.Where(c => c.IdCampanha.Equals(solicitacao.IdCampanha.ToInt64())).FirstOrDefault();
                if (fila != null)
                    fila.SolicitacoesCampanha.Enqueue(solicitacao);
                else
                {
                    CampanhaDiscagem nova_fila_campanha = new CampanhaDiscagem();
                    nova_fila_campanha.IdCampanha = solicitacao.IdCampanha.ToInt64();
                    nova_fila_campanha.SolicitacoesCampanha.Enqueue(solicitacao);
                    FilasDiscagem.Add(nova_fila_campanha);
                }

                if (OnSolicitacaoNovaLigacao != null)
                    OnSolicitacaoNovaLigacao();
            }
            catch (Exception ex)
            {
                MensagemLog("Falha no servidor. A mensagem do sistema foi:" + ex.Message, Common.LoggerType.ERRO, ConsoleColor.Red);
            }
            finally
            {
                Monitor.Exit(FilasDiscagem);
            }
        }

        /// <summary>
        /// variável utilizada como cursor de movimentação para
        /// balanceamento de discagens das campanhas
        /// </summary>
        static int posicao_fila_discagem = 0;
        static bool fim_da_fila = false;
        static Silver.DTO.SolicitacaoDiscagem ProximaSolicitacaoDaFila()
        {
            Monitor.Enter(FilasDiscagem);
            try
            {
                SolicitacaoDiscagem solicitacao = null;
                CampanhaDiscagem campanha_solicitacoes = null;

                do
                {
                    if (fim_da_fila)
                    {
                        fim_da_fila = false;
                        return null;
                    }

                    if (FilasDiscagem.Count <= 0)
                        return null;

                    if (posicao_fila_discagem >= FilasDiscagem.Count)
                    {
                        posicao_fila_discagem = 0;
                        fim_da_fila = true;
                    }

                    campanha_solicitacoes = FilasDiscagem[posicao_fila_discagem];
                    posicao_fila_discagem++;

                    if (campanha_solicitacoes.SolicitacoesCampanha.Count <= 0)
                        continue;

                    solicitacao = campanha_solicitacoes.SolicitacoesCampanha.Dequeue();

                } while (solicitacao == null);

                return solicitacao;
            }
            catch (Exception ex)
            {
                MensagemLog("ProximaSolicitacao - Falha no servidor. A mensagem do sistema foi:" + ex.Message, Common.LoggerType.ERRO, ConsoleColor.Red);
            }
            finally
            {
                Monitor.Exit(FilasDiscagem);
            }

            return null;
        }

        static void servidor_OnSolicitacaoDiscagem(Silver.DTO.SolicitacaoDiscagem solicitacao)
        {
            AdicionarSolicitacaoNaFila(solicitacao);
        }

        static void MensagemLog(string mensagem, Common.LoggerType tipo_log = Common.LoggerType.INFO, ConsoleColor color = ConsoleColor.White)
        {
            string path_logs = Path.Combine(ConfigurationManager.AppSettings["application.path.logs"], string.Format("Logs-{0}-{1}.silver", "ServerProxyDiscagem", DateTime.Now.ToString("ddMMyyyyHH")));
            Silver.Common.Logger.LoggerAsync.pathLog = path_logs;
            Silver.Common.Logger.LoggerAsync.WriteLog(new Common.Logger.LoggerMessage { MessageLogger = mensagem, TypeLogger = tipo_log }, color);
        }

        static Silver.AsteriskClient.SaidaLigacao saida_ligacao = (Silver.AsteriskClient.SaidaLigacao)Enum.Parse(typeof(Silver.AsteriskClient.SaidaLigacao), ConfigurationManager.AppSettings["application.saida_ligacao"], true);
        static string path_trabalho = ConfigurationManager.AppSettings["application.path.trabalho"];
        static string path_outgoing = ConfigurationManager.AppSettings["application.path.outgoing"];

        static void Discar()
        {
            try
            {
                Silver.DTO.SolicitacaoDiscagem solicitacaoDiscagem = ProximaSolicitacaoDaFila();
                while (solicitacaoDiscagem != null)
                {
                    int qtd_ligacoes_segundo = ConfigurationManager.AppSettings["application.qtd_ligacoes_segundo"].ToInt32();
                    bool transbordo = false;

                    try
                    {
                        List<Silver.DTO.Rota> prioridadesRota = DefineRotas(solicitacaoDiscagem);
                        Protocolo protocolo = Protocolo.NaoDefinido_SemCanais;

                        DTO.Channels grupo = null;

                        Protocolo protocolo_padrao_transbordo = Protocolo.Nenhum;

                        foreach (Silver.DTO.Rota prioridade in prioridadesRota.OrderBy(c => c.Prioridade))
                        {
                            Protocolo auxprotocolo = Protocolo.Telefonica;

                            #region switch
                            switch (prioridade.IdOperadora)
                            {
                                case 4:
                                    auxprotocolo = Protocolo.Telefonica;
                                    break;
                                case 5:
                                    auxprotocolo = Protocolo.GVT;
                                    break;
                                case 6:
                                    auxprotocolo = Protocolo.Transit;
                                    break;
                                case 7:
                                    auxprotocolo = Protocolo.Mahatel;
                                    break;
                                case 8:
                                    auxprotocolo = Protocolo.Algar;
                                    break;
                                case 9:
                                    auxprotocolo = Protocolo.Nexus;
                                    break;
                                case 10:
                                    auxprotocolo = Protocolo.Pontal;
                                    break;
                            }
                            #endregion

                            grupo = GruposCanais.Where(g => g.Grupo.Equals((long)auxprotocolo)).FirstOrDefault();
                            if (grupo.Quantidade <= 0)
                            {
                                ast_comando_consulta_canais.DGVShowChannels();

                                while (RelChannelsAtualizado == false)
                                    System.Threading.Thread.Sleep(50);
                            }

                            if (grupo.Quantidade <= 0)
                            {
                                #region Mensagem Sistema
                                if (!Silver.BLL.MensagemSistema.ExisteMensagemNaoVisualizada(solicitacaoDiscagem.IdCampanha.ToInt64(), TipoMensagemSistema.E1_SemCanal))
                                {
                                    Silver.BLL.MensagemSistema.Cadastrar(new MensagemSistema()
                                    {
                                        DataHora = DateTime.Now,
                                        IdCampanha = solicitacaoDiscagem.IdCampanha.ToInt64(),
                                        Mensagem = String.Format("Sem canais disponíveis no E1. Grupo: {0}, Campanha:<br><b>{1}</b>", grupo.Grupo, solicitacaoDiscagem.Campanha),
                                        Visualizada = false,
                                        TipoMensagem = (int)Silver.DTO.TipoMensagemSistema.E1_SemCanal
                                    });
                                }
                                #endregion
                                transbordo = true;
                                protocolo_padrao_transbordo = auxprotocolo;
                                MensagemLog(string.Format("Sem canais disponíveis no grupo: {0}", grupo.Grupo), Silver.Common.LoggerType.INFO, ConsoleColor.Red);
                                continue;
                            }
                            else
                            {
                                protocolo = auxprotocolo;
                                MensagemLog(string.Format("DDD: {0}\tTelefone: {1}\tProtocolo: {2}\tCanais Livres: {3}", solicitacaoDiscagem.DDD, solicitacaoDiscagem.Telefone, auxprotocolo, grupo.Quantidade), Silver.Common.LoggerType.INFO, ConsoleColor.Green);
                            }
                        }

                        if (protocolo != Protocolo.NaoDefinido_SemCanais)
                        {
                            if (protocolo_padrao_transbordo == Protocolo.Nenhum)
                                protocolo_padrao_transbordo = protocolo;

                            using (StreamWriter sw = new StreamWriter("C:\\Silver\\Logs\\DiscagensTransbordo.csv", true))
                                sw.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7};{8}",
                                    solicitacaoDiscagem.DDD,
                                    solicitacaoDiscagem.Telefone,
                                    solicitacaoDiscagem.TipoTelefone,
                                    protocolo,
                                    solicitacaoDiscagem.TipoIdentificado,
                                    solicitacaoDiscagem.RotaPadrao,
                                    transbordo.ToInt32(),
                                    protocolo_padrao_transbordo,
                                    DateTime.Now.ToString("dd/MM/yy HH:mm:ss.ffff"));

                            qtd_ligacoes_segundo = ConfigurationManager.AppSettings["application.qtd_ligacoes_segundo"].ToInt32();
                            System.Threading.Thread.Sleep(1000 / qtd_ligacoes_segundo);

                            switch (saida_ligacao)
                            {
                                case SaidaLigacao.Stream:
                                    SolicitarDiscagemAsterisk(solicitacaoDiscagem, protocolo);
                                    break;
                                case SaidaLigacao.FileSystem:

                                    var grupo_filtro = string.Format("grupo_{0}", protocolo);
                                    var arquivos_outgoing = Directory.GetFiles(path_outgoing).Where(a => a.Contains(grupo_filtro));
                                    int qtd_arquivos_outgoing = arquivos_outgoing.Count();
                                    while (qtd_arquivos_outgoing >= grupo.Quantidade)
                                        System.Threading.Thread.Sleep(1000);

                                    string nome_arquivo = string.Format("grupo_{0}_{1}_{2}.call", protocolo, solicitacaoDiscagem.Campanha, DateTime.Now.ToString("ddmmyyyy-HHmmssffff"));
                                    StringBuilder sb_conteudo_arquivo = new StringBuilder();
                                    using (StreamWriter sw = new StreamWriter(Path.Combine(path_trabalho, nome_arquivo)))
                                    {
                                        sb_conteudo_arquivo.AppendLine(string.Format("Channel: DGV/g{0}/{1}", (int)protocolo, solicitacaoDiscagem.Telefone));
                                        sb_conteudo_arquivo.AppendLine(string.Format("WaitTime: 45"));
                                        sb_conteudo_arquivo.AppendLine(string.Format("Context: interno"));
                                        sb_conteudo_arquivo.AppendLine(string.Format("Extension: start"));
                                        sb_conteudo_arquivo.AppendLine(string.Format("AlwaysDelete: yes"));
                                        sb_conteudo_arquivo.AppendLine(string.Format("SetVar: Campanha={0}", solicitacaoDiscagem.Campanha));
                                        sb_conteudo_arquivo.AppendLine(string.Format("SetVar: TelefoneID={0}", solicitacaoDiscagem.IdTelefone));
                                        sb_conteudo_arquivo.AppendLine(string.Format("SetVar: TipoTelefone={0}", solicitacaoDiscagem.TipoTelefone));
                                        sb_conteudo_arquivo.AppendLine(string.Format("SetVar: IdCampanha={0}", solicitacaoDiscagem.IdCampanha));

                                        sw.Write(sb_conteudo_arquivo.ToString());
                                        sw.Flush();
                                        sw.Close();
                                        sw.Dispose();
                                    }

                                    //File.Move(Path.Combine(path_trabalho, nome_arquivo), Path.Combine(path_outgoing, nome_arquivo));
                                    FinalizaConexaoCliente(solicitacaoDiscagem);
                                    break;
                                default:
                                    break;
                            }


                            var cor_atual = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\n\n\t[DEBUG] - Atendendo campanha: {0} - [{1}]\n\n", solicitacaoDiscagem.Campanha, DateTime.Now.ToString("HH:mm:ss.ffff"));
                            Console.ForegroundColor = cor_atual;
                            grupo.Quantidade -= 1;
                        }
                        else
                        {
                            #region Enviando Sem Canais Disponiveis ao solicitante

                            Silver.DTO.RespostaSolicitacaoDiscagem respostaSolicitacaoDiscagem = new Silver.DTO.RespostaSolicitacaoDiscagem();
                            respostaSolicitacaoDiscagem.MotivoResposta = "Sem Canais Disponiveis";
                            respostaSolicitacaoDiscagem.RespostaSolicitacao = solicitacaoDiscagem.ToString();

                            using (StreamWriter stream_writer = new StreamWriter(solicitacaoDiscagem.Cliente.GetStream(), Encoding.ASCII))
                            {
                                stream_writer.Write(respostaSolicitacaoDiscagem.ToString());
                                stream_writer.AutoFlush = true;
                                stream_writer.Flush();
                            }

                            #endregion
                        }

                        if (solicitacaoDiscagem.Cliente.Client.Connected)
                        {
                            solicitacaoDiscagem.Cliente.Client.Disconnect(false);
                            solicitacaoDiscagem.Cliente.Client.Close();
                            solicitacaoDiscagem.Cliente.Client.Dispose();
                            solicitacaoDiscagem.Cliente.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MensagemLog(string.Format
                            (
                                "Não foi possível processar o telefone {0} da solicitação de Discagem na campanha {1}. A mensagem do sistema foi: {2}"
                                , solicitacaoDiscagem.Telefone
                                , solicitacaoDiscagem.Campanha
                                , ex.Message + Environment.NewLine + ex.StackTrace
                             ));
                    }
                    finally
                    {
                        solicitacaoDiscagem = ProximaSolicitacaoDaFila();
                    }
                }

                processando_solicitacoes = false;
            }
            catch (Exception ex) { MensagemLog("Discar() - Falha no processamento das Discagem. A mensagem do sistema foi: " + ex.Message, Silver.Common.LoggerType.ERRO, ConsoleColor.Red); }
            finally
            {
                processando_solicitacoes = false;
                MensagemLog("Requisições processadas! Aguardando novas Solicitações de Discagem.", Silver.Common.LoggerType.INFO, ConsoleColor.Yellow);
            }
        }

        /// <summary>
        /// Envia as solicitações de discagem para o Asterisk
        /// </summary>
        /// <param name="solicitacaoDiscagem">Informações do Originate e suas Variáveis</param>
        /// <param name="protocolo">Operadora</param>

        private static void SolicitarDiscagemAsterisk(Silver.DTO.SolicitacaoDiscagem solicitacaoDiscagem, Protocolo protocolo)
        {
            MensagemLog("Solicitação de discagem enviada com SUCESSO!", Silver.Common.LoggerType.INFO, ConsoleColor.White);
            ast_comando_solicita_discagem.Discar(new Discagem()
            {
                Protocolo = ((int)protocolo).ToString(),
                Telefone = solicitacaoDiscagem.Telefone,
                IdCampanha = solicitacaoDiscagem.IdCampanha.ToString(),
                Campanha = solicitacaoDiscagem.Campanha.Trim().Replace(' ', '_'),
                IdTelefone = solicitacaoDiscagem.IdTelefone.ToString(),
                TipoTelefone = solicitacaoDiscagem.TipoTelefone.ToString()
            });

            FinalizaConexaoCliente(solicitacaoDiscagem);
        }

        private static void FinalizaConexaoCliente(Silver.DTO.SolicitacaoDiscagem solicitacaoDiscagem)
        {
            Silver.DTO.RespostaSolicitacaoDiscagem respostaSolicitacaoDiscagem = new Silver.DTO.RespostaSolicitacaoDiscagem();
            respostaSolicitacaoDiscagem.MotivoResposta = "ok";
            respostaSolicitacaoDiscagem.RespostaSolicitacao = solicitacaoDiscagem.ToString();

            using (StreamWriter stream_writer = new StreamWriter(solicitacaoDiscagem.Cliente.GetStream(), Encoding.ASCII))
            {
                stream_writer.AutoFlush = true;
                stream_writer.Write(respostaSolicitacaoDiscagem.ToString());
                stream_writer.Flush();

                if (solicitacaoDiscagem.Cliente.Client.Connected)
                {
                    solicitacaoDiscagem.Cliente.Client.Shutdown(SocketShutdown.Both);
                    solicitacaoDiscagem.Cliente.Client.Disconnect(false);
                    solicitacaoDiscagem.Cliente.Client.Close();
                    solicitacaoDiscagem.Cliente.Client.Dispose();
                }
            }
        }

        #region DGV Show Channels
        // Time para atualizar no banco de dados
        static DateTime AtualizaRelChannels = new DateTime();

        // Flag para controle de termino
        static bool RelChannelsAtualizado = false;

        // Lista com todos os canais instalados no Asterisk
        static List<Silver.DTO.RelChannels> channels = new List<Silver.DTO.RelChannels>();
        private static void OnDGVShowChannels(string valores)
        {
            //Varre os canais separando os campos e inserindo na lista
            atualizou_grupos = false;
            channels.Clear();

            foreach (string linha in valores.Split('\n'))
            {
                if ((linha == "") || (linha.Contains("Chan Grp Context          PortId")))
                    continue;

                Silver.DTO.RelChannels channel = new Silver.DTO.RelChannels();
                channel.Chan = linha.Substring(0, 5).Trim().ToInt32();
                channel.Grp = linha.Substring(6, 4).Trim().ToInt32();
                channel.Context = linha.Substring(10, 17).Trim();
                channel.PortId = linha.Substring(27, 11).Trim();
                channel.Rsrvd = linha.Substring(38, 8).Trim().ToInt32();
                channel.Alrmd = linha.Substring(46, 6).Trim().ToInt32();
                channel.Lckd = linha.Substring(52, 5).Trim();
                channel.Extension = linha.Substring(57, 10).Trim();
                channel.CardType = linha.Substring(67, 14).Trim();
                channel.Intrf = linha.Substring(81, 9).Trim();
                channels.Add(channel);
            }

            var grupos = channels.GroupBy(c => c.Grp);
            foreach (var g in grupos)
            {
                if (GruposCanais.Where(y => y.Grupo.Equals(g.Key)).FirstOrDefault() == null)
                    GruposCanais.Add(new DTO.Channels() { Grupo = g.Key, Quantidade = 0 });

                DTO.Channels grupo = GruposCanais.Where(x => x.Grupo.Equals(g.Key)).FirstOrDefault();
                int quantidade = channels.Where(c => c.Grp.Equals(g.Key)).Where(d => d.Rsrvd.Equals(0)).Count();

                grupo.Quantidade = quantidade;
                atualizou_grupos = true;
                MensagemLog(string.Format("Canais Atualizados: Grupo: {0}, Canais: {1}", g.Key.ToString("00"), quantidade.ToString("00")).ToUpper(), Silver.Common.LoggerType.INFO, ConsoleColor.Magenta);
            }

            if (!atualizou_grupos)
                MensagemLog("Os grupos de canais não foram atualizados!", Silver.Common.LoggerType.INFO, ConsoleColor.Red);

            // De 3 em 3 segundos atualizar o banco de dados
            if (AtualizaRelChannels.AddSeconds(3) >= DateTime.Now)
            {
                XmlSerializer xml = new XmlSerializer(typeof(List<Silver.DTO.RelChannels>));
                StringWriter strWriter = new StringWriter();
                xml.Serialize(strWriter, channels);

                Task.Factory.StartNew(() => new Silver.DAL.RelChannels().Inserir(strWriter.ToString()));
                AtualizaRelChannels = DateTime.Now;
            }

            // informa que o processo foi finalizado
            RelChannelsAtualizado = true;
        }

        private static void OnLogoffAsterisk()
        {

        }

        #endregion

        #region Regra de Discagem
        #region Protocolos
        //Lista dos protocolos/operadoras existentes no Discador
        static SortedList<Protocolo, string> OperadoraDDD =
          new SortedList<Protocolo, string>() {
            { Protocolo.Algar, "015" },
            { Protocolo.GVT, "015" },
            { Protocolo.Mahatel, "015" },
            { Protocolo.Nexus, "015" },
            { Protocolo.Pontal, "015" },
            { Protocolo.Telefonica, "015" },
            { Protocolo.Transit, "015" }};
        #endregion

        #region Listas dos DDD 11 que precisa utilizar numero da operadora
        //Lista dos prefixos que necessitam a inserção da Operadora e DDD
        static List<string> LDDD11 = new List<string>();

        static void ListaDDD11()
        {
            #region Lista para utilizar DDD 11
            LDDD11.Add("4136");
            LDDD11.Add("4204");
            LDDD11.Add("2123");
            LDDD11.Add("2144");
            LDDD11.Add("2187");
            LDDD11.Add("2198");
            LDDD11.Add("2202");
            LDDD11.Add("2588");
            LDDD11.Add("3799");
            LDDD11.Add("4653");
            LDDD11.Add("4654");
            LDDD11.Add("4655");
            LDDD11.Add("2119");
            LDDD11.Add("2427");
            LDDD11.Add("3402");
            LDDD11.Add("4012");
            LDDD11.Add("4411");
            LDDD11.Add("4412");
            LDDD11.Add("4413");
            LDDD11.Add("4414");
            LDDD11.Add("4415");
            LDDD11.Add("4417");
            LDDD11.Add("4418");
            LDDD11.Add("4494");
            LDDD11.Add("4012");
            LDDD11.Add("4891");
            LDDD11.Add("2277");
            LDDD11.Add("2473");
            LDDD11.Add("3404");
            LDDD11.Add("4031");
            LDDD11.Add("4032");
            LDDD11.Add("4033");
            LDDD11.Add("4035");
            LDDD11.Add("4481");
            LDDD11.Add("4603");
            LDDD11.Add("4409");
            LDDD11.Add("4528");
            LDDD11.Add("4529");
            LDDD11.Add("4711");
            LDDD11.Add("4717");
            LDDD11.Add("4658");
            LDDD11.Add("3408");
            LDDD11.Add("4487");
            LDDD11.Add("4534");
            LDDD11.Add("4538");
            LDDD11.Add("4894");
            LDDD11.Add("2136");
            LDDD11.Add("4592");
            LDDD11.Add("4593");
            LDDD11.Add("4409");
            LDDD11.Add("4529");
            LDDD11.Add("4017");
            LDDD11.Add("4539");
            LDDD11.Add("2136");
            LDDD11.Add("2152");
            LDDD11.Add("2434");
            LDDD11.Add("2816");
            LDDD11.Add("2882");
            LDDD11.Add("3308");
            LDDD11.Add("3378");
            LDDD11.Add("3379");
            LDDD11.Add("3395");
            LDDD11.Add("3446");
            LDDD11.Add("3963");
            LDDD11.Add("3964");
            LDDD11.Add("4491");
            LDDD11.Add("4492");
            LDDD11.Add("4497");
            LDDD11.Add("4521");
            LDDD11.Add("4522");
            LDDD11.Add("4523");
            LDDD11.Add("4525");
            LDDD11.Add("4526");
            LDDD11.Add("4527");
            LDDD11.Add("4531");
            LDDD11.Add("4532");
            LDDD11.Add("4535");
            LDDD11.Add("4581");
            LDDD11.Add("4583");
            LDDD11.Add("4584");
            LDDD11.Add("4587");
            LDDD11.Add("4589");
            LDDD11.Add("4599");
            LDDD11.Add("4601");
            LDDD11.Add("4815");
            LDDD11.Add("4816");
            LDDD11.Add("4014");
            LDDD11.Add("4406");
            LDDD11.Add("4597");
            LDDD11.Add("4895");
            LDDD11.Add("4037");
            LDDD11.Add("4018");
            LDDD11.Add("4896");
            LDDD11.Add("4036");
            LDDD11.Add("4405");
            LDDD11.Add("4021");
            LDDD11.Add("4029");
            LDDD11.Add("4602");
            LDDD11.Add("4714");
            LDDD11.Add("4716");
            LDDD11.Add("4711");
            LDDD11.Add("4713");
            LDDD11.Add("4719");
            LDDD11.Add("4784");
            LDDD11.Add("2434");
            LDDD11.Add("3378");
            LDDD11.Add("3395");
            LDDD11.Add("4493");
            LDDD11.Add("4595");
            #endregion
        }

        #endregion

        /// <summary>
        /// Define a Rota de menos custo, bem como sua prioridade
        /// </summary>
        /// <param name="solicitacaoDiscagem"></param>
        /// <returns></returns>
        static List<Silver.DTO.Rota> DefineRotas(Silver.DTO.SolicitacaoDiscagem solicitacaoDiscagem)
        {
            var rotas = new BLL.Rota().Obter(true);
            List<Silver.DTO.Rota> prioridadeRota = new List<Silver.DTO.Rota>();

            string telefone_solicitacao = new string(solicitacaoDiscagem.Telefone.ToCharArray());
            if (telefone_solicitacao.Length >= 12) //Possui operadora , ddd e 9
                solicitacaoDiscagem.Telefone = telefone_solicitacao.Substring(5);

            if ("56789".Contains(solicitacaoDiscagem.Telefone[0])) //Celular
            {
                solicitacaoDiscagem.TipoIdentificado = "Celular";
                if (solicitacaoDiscagem.DDD.ToInt16() >= 20) //VC3
                {
                    solicitacaoDiscagem.RotaPadrao = "VC3";
                    prioridadeRota = rotas.Where(c => c.IdTarifaTipo.Equals(5)).OrderBy(c => c.Prioridade).ToList();
                }
                else if (solicitacaoDiscagem.DDD.ToInt16() > 11) //VC2
                {
                    solicitacaoDiscagem.RotaPadrao = "VC2";
                    prioridadeRota = rotas.Where(c => c.IdTarifaTipo.Equals(4)).OrderBy(c => c.Prioridade).ToList();
                }
                else //VC1
                {
                    solicitacaoDiscagem.RotaPadrao = "VC1";
                    prioridadeRota = rotas.Where(c => c.IdTarifaTipo.Equals(6)).OrderBy(c => c.Prioridade).ToList();
                }
            }
            else //Fixo
            {
                solicitacaoDiscagem.TipoIdentificado = "Fixo";
                if (solicitacaoDiscagem.DDD.ToInt16() >= 20) //Inter
                {
                    solicitacaoDiscagem.RotaPadrao = "Inter";
                    prioridadeRota = rotas.Where(c => c.IdTarifaTipo.Equals(3)).OrderBy(c => c.Prioridade).ToList();
                }
                else if (solicitacaoDiscagem.DDD.ToInt16() > 11) //Intra
                {
                    solicitacaoDiscagem.RotaPadrao = "Intra";
                    prioridadeRota = rotas.Where(c => c.IdTarifaTipo.Equals(2)).OrderBy(c => c.Prioridade).ToList();
                }
                else if ((solicitacaoDiscagem.DDD.ToInt16() == 11) && (LDDD11.Contains(solicitacaoDiscagem.Telefone.ToString().Substring(0, 4)))) //Intra
                {
                    solicitacaoDiscagem.RotaPadrao = "Intra";
                    prioridadeRota = rotas.Where(c => c.IdTarifaTipo.Equals(2)).OrderBy(c => c.Prioridade).ToList();
                }
                else //Local
                {
                    solicitacaoDiscagem.RotaPadrao = "Local";
                    prioridadeRota = rotas.Where(c => c.IdTarifaTipo.Equals(1)).OrderBy(c => c.Prioridade).ToList();
                }
            }

            solicitacaoDiscagem.Telefone = telefone_solicitacao;
            return prioridadeRota;
        }

        #endregion

        // Evento na escuta do Asterisk para o comando DGV Show Channels
        static bool atualizou_grupos = false;
        static int tempo_sleep = 1000 * 14; //Tempo de espera para a próxima execução

        static void BeginAtualizarCanais()
        {
            System.Threading.Thread.Sleep(tempo_sleep);
            ast_comando_consulta_canais.DGVShowChannels();
            servico_atualizacao_canais_livres.BeginInvoke(EndAtualizarCanais, null);
        }

        static void EndAtualizarCanais(IAsyncResult resultado_final)
        {
            AsyncResult resultado = (AsyncResult)resultado_final;
            AtualizarCanaisLivres chamador = (AtualizarCanaisLivres)resultado.AsyncDelegate;

            string formatString = (string)resultado_final.AsyncState;
            chamador.EndInvoke(resultado_final);

            if (atualizou_grupos)
                MensagemLog("Finalizando atualização de canais livres", Silver.Common.LoggerType.INFO, ConsoleColor.Magenta);
            else
            {
                MensagemLog("Canais não atualizados. ", Silver.Common.LoggerType.INFO, ConsoleColor.Red);
                MensagemLog("Comunicação com o servidor asterisk RECONFIGURADA. ", Silver.Common.LoggerType.INFO, ConsoleColor.Yellow);
            }

            ConfigurarComunicacaoAsterisk();
            servico_atualizacao_canais_livres.BeginInvoke(new AsyncCallback(EndAtualizarCanais), null);
        }
    }
}