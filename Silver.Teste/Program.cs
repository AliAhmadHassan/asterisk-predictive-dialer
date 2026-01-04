using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Silver.AsteriskClient;
using Silver.AsteriskClient.DTO;
using Silver.Common;
using Silver.DTO;
using System.Runtime.InteropServices;
using Silver.Discador.DTO;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Remoting.Messaging;

namespace Silver.Discador
{
    class Program
    {
        #region Variáveis

        public delegate void SolicitacaoNovaLigacao();
        public static event SolicitacaoNovaLigacao OnSolicitacaoNovaLigacao;

        static bool processando_solicitacoes = false;
        static bool PrimeiroCiclo = true;
        static bool RamalOnline = false;
        static bool tentar_processar = false;

        static SortedList<string, int> TelefonesPercursos = new SortedList<string, int>();
        static SortedList<string, string> StatusTelefoneAux = new SortedList<string, string>();
        static SortedList<string, int> ControleDiscagem = new SortedList<string, int>();
        static List<RamalIdle> ControleIdleRamais = new List<RamalIdle>();

        static Queue<CargaTelefoneRobo> fila_discagem = new Queue<CargaTelefoneRobo>();

        static Silver.AsteriskClient.AsteriskCommand comandos_asterisk;
        static Silver.AsteriskClient.AsteriskListener escuta_asterisk;
        static Silver.BLL.SaidaAsterisk servico_db_asterisk;

        static BLL.ProcessoCampanha processoCampanha = null;
        static List<string> LDDD11 = new List<string>();
        static List<Silver.AsteriskClient.DTO.RelCampanhaQueueStatus> lstRelCampanhaQueueStatus = new List<Silver.AsteriskClient.DTO.RelCampanhaQueueStatus>();

        static BLL.SaidaAsterisk servico_leituraasterisk = new BLL.SaidaAsterisk();

        #region Protocolos
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

        static System.Timers.Timer timer_atualiza_andamento = new System.Timers.Timer(10000);
        static System.Timers.Timer timerForcarRetorno = new System.Timers.Timer(10000);

        /// <summary>
        /// Atualização da Agressividade automaticamente
        /// </summary>
        static System.Timers.Timer timer_agressividade = new System.Timers.Timer(60000);
        static string titulo_console = string.Empty;

        #endregion

        #region Variáveis
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);
        private delegate bool EventHandler(CtrlType sig);

        static EventHandler _handler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        enum OperacaoDiscador : int
        {
            InicioOperacaoDiscador = 1,
            FimOperacaoDiscador = 2,
            Parado = 3,
            Erro = 4
        }

        static bool Handler(CtrlType sig)
        {
            switch (sig)
            {
                case CtrlType.CTRL_C_EVENT:
                case CtrlType.CTRL_BREAK_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                default:

                    if (controle_sistema_execucao.Situacao != (int)SitucaoEventoControleSistema.Finalizado)
                    {
                        Silver.BLL.ControleSistema.AtualizarStatusExecucao(SitucaoEventoControleSistema.Parado, controle_sistema_execucao.Id);
                        Silver.BLL.ControleSistema.IncluirEvento(EventoControleSistema.Parar_Campanha, new ControleSistema { Valor = processoCampanha.Campanha.Id.ToString(), Campanha = processoCampanha.Campanha.Id, Situacao = (int)SitucaoEventoControleSistema.Aguardando, Solicitante = 36 });
                        new BLL.LogDiscador().Incluir(new LogDiscador() { DataHora = DateTime.Now, Evento = (int)OperacaoDiscador.Parado, IdCampanha = (int)processoCampanha.Campanha.Id });
                        Silver.BLL.MensagemSistema.Cadastrar(new MensagemSistema()
                        {
                            DataHora = DateTime.Now,
                            IdCampanha = processoCampanha.Campanha.Id,
                            Mensagem = String.Format("Fim das Discagens na campanha:<br><b>{0}</b>", processoCampanha.Campanha.Nome),
                            Visualizada = false,
                            TipoMensagem = (int)Silver.DTO.TipoMensagemSistema.Discador_Fim
                        });
                    }

                    break;
            }
            return true;
        }

        static void CabecalhoDiscador()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Out.WriteLine("".PadRight(100, '*'));
            Console.Out.WriteLine("* Aplicação: Discador Silver®\t\tVersão: {0}\t\tBuild: {1}", "1.0.0", "1000");
            Console.Out.WriteLine("* Usuario Serviço: {0}\t\tInício: {1}\t\t", Environment.UserName, DateTime.Now.ToString("d-M-yy HH:mm"));
            Console.Out.WriteLine("".PadRight(100, '*'));
            Console.Out.WriteLine("* Id Campanha: \t\t\t\t{0}", processoCampanha.Campanha.Id.ToString("0000"));
            Console.Out.WriteLine("* Nome Campanha: \t\t\t{0}", processoCampanha.Campanha.Nome);
            Console.Out.WriteLine("* Total Operadores: \t\t\t{0}", processoCampanha.Operadores.Count.ToString("000"));
            Console.Out.WriteLine("* Total Carga: \t\t\t\t{0}", processoCampanha.Cargas.Count.ToString("0000"));
            Console.Out.WriteLine("* Total Telefones: \t\t\t{0}", processoCampanha.Telefones.Count.ToString("00000"));
            Console.Out.WriteLine("".PadRight(100, '*'));
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Out.WriteLine("[{0}] - Discador Iniciado com sucesso...", DateTime.Now);
            Console.Out.WriteLine("[{0}] - Aguardando 30 segundos antes de iniciar a aplicação", DateTime.Now);
            Console.Title += " - Campanha: " + processoCampanha.Campanha.Descricao;
            MensagemLog(string.Format("Parâmetros de inicialização do discador: Nome Campanha: {0}, Id Controle Sistema: {1}", processoCampanha.Campanha.Nome, controle_sistema_execucao.Id), Common.LoggerType.INFO, ConsoleColor.Yellow);
            titulo_console = Console.Title;
        }

        static ControleSistema controle_sistema_execucao = new ControleSistema();
        #endregion

        #region Leitura da base de dados Asterisk

        //static void timer_agressividade_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    timer_agressividade.Stop();
        //    timer_agressividade.Enabled = false;

        //    var campanha = new BLL.Campanha().Obter(processoCampanha.Campanha.Id);
        //    var agressividade_atual = processoCampanha.Campanha.Agressividade;

        //    if (agressividade_atual != campanha.Agressividade)
        //    {
        //        if (campanha != null)
        //        {
        //            processoCampanha.Campanha.Agressividade = campanha.Agressividade;
        //            MensagemLog("A agressividade da campanha foi atualizada para: " + processoCampanha.Campanha.Agressividade.ToString("000"), Common.LoggerType.INFO, ConsoleColor.DarkYellow);
        //        }

        //        Console.Title = titulo_console + ", Agressividade: " + processoCampanha.Campanha.Agressividade.ToString("000") + ", ";
        //    }

        //    timer_agressividade.Enabled = true;
        //    timer_agressividade.Start();
        //}

        static long posicao_leitura = servico_leituraasterisk.ObterMaiorId();
        static System.Timers.Timer timer_leituraDB = new System.Timers.Timer(250);
        static void timer_leituraDB_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer_leituraDB.Stop();
            timer_leituraDB.Enabled = false;

            var valores = new BLL.SaidaAsterisk().Listar(posicao_leitura);

            foreach (var item in valores)
            {
                escuta_asterisk.ExecutarSBBufferDB(item.Valor);
                posicao_leitura = item.Id;

                if (timerForcarRetorno.Enabled == true)
                {
                    timerForcarRetorno.Stop();
                    timerForcarRetorno.Enabled = false;
                }
            }

            if ((RamalOnline) && (valores.Count == 0))
            {
                if (timerForcarRetorno.Enabled == false)
                {
                    timerForcarRetorno.Enabled = true;
                    timerForcarRetorno.Start();
                }
            }

            if (contador_leitura % 101 == 0)
            {
                MensagemLog("Posição da leitura: " + posicao_leitura);
                contador_leitura = 1;
            }
            else
                contador_leitura++;

            timer_leituraDB.Enabled = true;
            timer_leituraDB.Start();


        }

        public delegate void LeituraDbAsterisk();

        static LeituraDbAsterisk OnLeituraDbAsterisk;
        static int contador_leitura = 1;
        static int tempo_espera_leitura_db = 1000 / 8; //# 120ms/leitura
        static void InicioLeituraBaseDados()
        {
            System.Threading.Thread.Sleep(tempo_espera_leitura_db);
            var valores = new BLL.SaidaAsterisk().Listar(posicao_leitura);

            foreach (var item in valores)
            {
                escuta_asterisk.ExecutarSBBufferDB(item.Valor);
                posicao_leitura = item.Id;

                if (timerForcarRetorno.Enabled == true)
                {
                    timerForcarRetorno.Stop();
                    timerForcarRetorno.Enabled = false;
                }
            }

            if ((RamalOnline) && (valores.Count == 0))
            {
                if (timerForcarRetorno.Enabled == false)
                {
                    timerForcarRetorno.Enabled = true;
                    timerForcarRetorno.Start();
                }
            }

            if (contador_leitura % 101 == 0)
            {
                var campanha = new BLL.Campanha().Obter(processoCampanha.Campanha.Id);
                var agressividade_atual = processoCampanha.Campanha.Agressividade;

                if (agressividade_atual != campanha.Agressividade)
                {
                    if (campanha != null)
                    {
                        processoCampanha.Campanha.Agressividade = campanha.Agressividade;
                        Console.Title = titulo_console + ", Agressividade: " + processoCampanha.Campanha.Agressividade.ToString("000") + ", ";
                        MensagemLog("A agressividade da campanha foi atualizada para: " + processoCampanha.Campanha.Agressividade.ToString("000"), Common.LoggerType.INFO, ConsoleColor.DarkYellow);
                    }
                }
                contador_leitura = 1;
            }
            else
                contador_leitura++;
        }

        static void FimLeituraBaseDados(IAsyncResult resultado_final)
        {
            AsyncResult resultado = (AsyncResult)resultado_final;
            LeituraDbAsterisk chamador = (LeituraDbAsterisk)resultado.AsyncDelegate;

            string formatString = (string)resultado_final.AsyncState;

            chamador.EndInvoke(resultado_final);
            OnLeituraDbAsterisk.BeginInvoke(new AsyncCallback(FimLeituraBaseDados), null);
        }

        #endregion

        static void Main(string[] args)
        {
            #region Código Removido
            //timer_agressividade.Elapsed += new System.Timers.ElapsedEventHandler(timer_agressividade_Elapsed);
            //timer_agressividade.Start();

            //timer_leituraDB.Elapsed += new System.Timers.ElapsedEventHandler(timer_leituraDB_Elapsed);
            //timer_leituraDB.Start();

            //timerForcarRetorno.Elapsed += new System.Timers.ElapsedEventHandler(timerForcarRetorno_Elapsed);
            #endregion

            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

            try
            {
                if (args == null)
                    throw new ArgumentNullException("Não foi encontrado o código da campanha para este discador");
                else
                {
                    processoCampanha = new BLL.ProcessoCampanha(new BLL.Campanha().Obter(args[0].ToInt64()));
                    controle_sistema_execucao = Silver.BLL.ControleSistema.ObterControleSistema(args[1].ToInt64());
                    Silver.BLL.MensagemSistema.Cadastrar(new MensagemSistema()
                    {
                        DataHora = DateTime.Now,
                        IdCampanha = processoCampanha.Campanha.Id,
                        Mensagem = String.Format("Fim das Discagens na campanha:<br><b>{0}</b>", processoCampanha.Campanha.Nome),
                        Visualizada = false,
                        TipoMensagem = (int)Silver.DTO.TipoMensagemSistema.Discador_Fim
                    });
                    Task.Factory.StartNew(() => Silver.BLL.ControleSistema.AtualizarStatusExecucao(SitucaoEventoControleSistema.Executando, controle_sistema_execucao.Id));
                    controle_sistema_execucao.Situacao = (int)SitucaoEventoControleSistema.Executando;
                }

                MensagemLog("Componentes iniciados", Common.LoggerType.INFO, ConsoleColor.Gray);
                ProcessoDiscador();

                processoCampanha.InicializarOperadores();
                MensagemLog("Carregando Operadores", Common.LoggerType.INFO, ConsoleColor.Gray);

                foreach (UsuarioRobo ur in processoCampanha.Operadores)
                    ControleIdleRamais.Add(new RamalIdle() { Ramal = ur.Ramal, ExistePausa = true });

                processoCampanha.InicializarCarga();
                MensagemLog("Processando Cargas", Common.LoggerType.INFO, ConsoleColor.Gray);

                TrataTelefone();
                MensagemLog("Tratamento dos telefones finalizado com sucesso", Common.LoggerType.INFO, ConsoleColor.Gray);

                CabecalhoDiscador();

                OnLeituraDbAsterisk = new LeituraDbAsterisk(InicioLeituraBaseDados);
                OnLeituraDbAsterisk.BeginInvoke(FimLeituraBaseDados, null);

                ConfigurarComunicacaoAsterisk();
                comandos_asterisk.StatusFila(processoCampanha.Campanha.Nome.Trim().Replace(' ', '_'));
                comandos_asterisk.StatusFila(processoCampanha.Campanha.Nome.Trim().Replace(' ', '_'));
                System.Threading.Thread.Sleep(1000);

                OnSolicitacaoNovaLigacao += Program_OnSolicitacaoNovaLigacao;

                new BLL.LogDiscador().Incluir(new LogDiscador() { DataHora = DateTime.Now, Evento = (int)OperacaoDiscador.InicioOperacaoDiscador, IdCampanha = (int)processoCampanha.Campanha.Id });
                Console.Title = titulo_console + ", Agressividade: " + processoCampanha.Campanha.Agressividade.ToString("000") + ", ";

                Silver.BLL.MensagemSistema.Cadastrar(new MensagemSistema()
                {
                    DataHora = DateTime.Now,
                    IdCampanha = processoCampanha.Campanha.Id,
                    Mensagem = String.Format("Início das discagens na campanha:<br><b>{0}</b>", processoCampanha.Campanha.Nome),
                    Visualizada = false,
                    TipoMensagem = (int)Silver.DTO.TipoMensagemSistema.Discador_Inicio
                });
                Console.Read();
            }
            catch (Exception ex)
            {
                MensagemLog("Main - " + ex.Message, Common.LoggerType.ERRO);
                Console.Read();
            }
            finally
            {
                if (comandos_asterisk != null)
                    comandos_asterisk.Dispose();

                if (escuta_asterisk != null)
                    escuta_asterisk.Dispose();
            }
        }

        private static void ConfigurarComunicacaoAsterisk()
        {
            if (comandos_asterisk != null)
                comandos_asterisk.Dispose();

            if (escuta_asterisk != null)
                escuta_asterisk.Dispose();

            comandos_asterisk = new AsteriskCommand();
            escuta_asterisk = new AsteriskListener();

            escuta_asterisk = new AsteriskListener();
            escuta_asterisk.SaidaPadrao = SaidaPadraoAsterisk.Delegate;

            servico_db_asterisk = new BLL.SaidaAsterisk();
            ConfigurarEscuta();

            servico_db_asterisk = new BLL.SaidaAsterisk();

            ConfigurarEscuta();
        }

        static void AtualizarAndamento()
        {
            if (processoCampanha == null) return;
            if (processoCampanha.Telefones.Count <= 0) return;

            decimal quantidade_total = processoCampanha.Telefones.Count;
            decimal quantidade_aguardando = processoCampanha.Telefones.Where(t => t.Status.Equals(1)).Count();
            decimal quantidade_andamento = quantidade_total - quantidade_aguardando;

            decimal porcentagem = (quantidade_andamento * 100) / quantidade_total;

            //Atualizar na base de dados
            Silver.BLL.ControleSistema.AtualizarPorcentagem(controle_sistema_execucao.Id, porcentagem);

            string porcentagem_string = "0";
            if (porcentagem <= 0)
                porcentagem_string = porcentagem.ToString().PadRight(10, '0');
            else
                porcentagem_string = porcentagem.ToString();

            MensagemLog(string.Format("Atualizado andamento da discagem. Porcentagem atual: {0}", porcentagem_string.Substring(0, 6)), Common.LoggerType.INFO);

        }

        static void ConfigurarEscuta()
        {
            escuta_asterisk.GravaSaidasAsterisk = servico_db_asterisk.Cadastrar;
            escuta_asterisk.OnErro = cliente_asterisk_onError;

            escuta_asterisk.SaidaPadrao = SaidaPadraoAsterisk.Delegate;
            escuta_asterisk.IniciarEscuta(comandos_asterisk.Stream_Asterisk);

            escuta_asterisk.OnQueueParams = cliente_asterisk_onQueueParams;
            escuta_asterisk.OnQueueMemberStatus = cliente_asterisk_onQueueMember;
            escuta_asterisk.OnQueueEntry = cliente_asterisk_onQueueEntry;
            escuta_asterisk.OnQueueStatusComplete = cliente_asterisk_onQueueStatusComplete;

            escuta_asterisk.OnSaiuDePausa = cliente_asterisk_onSaiuDePausa;
            escuta_asterisk.OnEntrouEmPausa = cliente_asterisk_onEntrouEmPausa;

            escuta_asterisk.OnRamalOcupado = cliente_asterisk_onRamalOcupado;
            escuta_asterisk.OnRamalNaoAtendeu = cliente_asterisk_onRamalNaoAtendeu;
            escuta_asterisk.OnRamalDiscando = cliente_asterisk_onRamalDiscando;
            escuta_asterisk.OnRamalChamando = cliente_asterisk_onRamalChamando;
            escuta_asterisk.OnRamalDesligou = cliente_asterisk_onRamalDesligou;
            escuta_asterisk.OnRamalAtendeu = cliente_asterisk_onRamalAtendeu;

            escuta_asterisk.OnDestinoAtendeu = cliente_asterisk_onDestinoAtendeu;
            escuta_asterisk.OnDestinoChamando = cliente_asterisk_onDestinoChamando;
            escuta_asterisk.OnDestinoDesligou = cliente_asterisk_onDestinoDesligou;
            escuta_asterisk.OnDestinoDiscando = cliente_asterisk_onDestinoDiscando;
            escuta_asterisk.OnDestinoNaoAtendeu = cliente_asterisk_onDestinoNaoAtendeu;
            escuta_asterisk.OnDestinoOcupado = cliente_asterisk_onDestinoOcupado;
        }

        #region Métodos Timer

        static void timerForcarRetorno_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timerForcarRetorno.Stop();
            timerForcarRetorno.Enabled = false;
            comandos_asterisk.StatusFila(processoCampanha.Campanha.Nome.Trim().Replace(' ', '_'));
            MensagemLog("Atualização Forçada do StatusFila", Common.LoggerType.INFO, ConsoleColor.DarkGreen);
        }

        #endregion

        #region Event's

        public static void cliente_asterisk_onQueueParams(AsteriskClient.DTO.QueueParamsEntity queueParamsEntity)
        {
            try
            {
                if (queueParamsEntity.Queue != processoCampanha.Campanha.Nome.Trim().Replace(" ", "_"))
                    return;

                int QtdOperadoresAguardando = processoCampanha.Operadores.Where(c => c.Status.Equals(EstadoUsuario.Aguardando)).Count();
                if (QtdOperadoresAguardando > 0)
                    RamalOnline = true;

                int QtdLigacoesAguardando = queueParamsEntity.Calls;
                int NovasLigacoes = QtdOperadoresAguardando - QtdLigacoesAguardando;

                if (NovasLigacoes > 0)
                {
                    if (PrimeiroCiclo)
                    {
                        long qtd = 2;
                        long qtd_ligacoes = processoCampanha.Campanha.Agressividade * NovasLigacoes;

                        for (long i = 0; i < qtd_ligacoes; i += qtd)
                        {
                            Task.Factory.StartNew(() => GerarLigacoesCampanha(qtd));
                            System.Threading.Thread.Sleep(2000);
                        }

                        PrimeiroCiclo = false;
                    }
                    else
                    {
                        Task.Factory.StartNew(() => GerarLigacoesCampanha(processoCampanha.Campanha.Agressividade));
                    }
                }

                comandos_asterisk.Dispose();
            }
            catch (Exception ex) { MensagemLog(string.Format("cliente_asterisk_onQueueParams - Falha no discador da campanha {0}. Método: {1} Mensagem: {2}", processoCampanha.Campanha.Nome.ToUpper(), "clienteAsterisk_onQueueMember(long,string, QueueMemberStatus)".ToUpper(), ex.Message)); }
            finally
            {
                lstRelCampanhaQueueStatus.Clear();
                comandos_asterisk.Dispose();
            }
        }

        public static void cliente_asterisk_onQueueMember(long Ramal, string fila, QueueMemberStatus status)
        {
            try
            {
                UsuarioRobo operador = null;
                switch (status)
                {
                    case QueueMemberStatus.AST_DEVICE_UNKNOWN:
                    case QueueMemberStatus.AST_DEVICE_INVALID:
                    case QueueMemberStatus.AST_DEVICE_UNAVAILABLE:
                        operador = processoCampanha.Operadores.Where(c => c.Ramal.Equals(Ramal)).FirstOrDefault();
                        if (operador != null)
                            operador.Status = EstadoUsuario.Logoff;
                        break;
                    case QueueMemberStatus.AST_DEVICE_NOT_INUSE:
                    case QueueMemberStatus.AST_DEVICE_ONHOLD:
                        operador = processoCampanha.Operadores.Where(c => c.Ramal.Equals(Ramal)).FirstOrDefault();
                        if (operador != null)
                            operador.Status = EstadoUsuario.Aguardando;
                        break;
                    case QueueMemberStatus.AST_DEVICE_INUSE:
                    case QueueMemberStatus.AST_DEVICE_BUSY:
                        operador = processoCampanha.Operadores.Where(c => c.Ramal.Equals(Ramal)).FirstOrDefault();
                        if (operador != null)
                            operador.Status = EstadoUsuario.Atendimento;

                        break;
                    case QueueMemberStatus.AST_DEVICE_RINGING:
                    case QueueMemberStatus.AST_DEVICE_RINGINUSE:
                        operador = processoCampanha.Operadores.Where(c => c.Ramal.Equals(Ramal)).FirstOrDefault();
                        if (operador != null)
                            operador.Status = EstadoUsuario.Andamento;

                        break;
                    case QueueMemberStatus.AST_DEVICE_PAUSED:
                        operador = processoCampanha.Operadores.Where(c => c.Ramal.Equals(Ramal)).FirstOrDefault();
                        if (operador != null)
                            operador.Status = EstadoUsuario.Pausa;

                        break;
                }

                comandos_asterisk.Dispose();

            }
            catch (Exception ex)
            {
                MensagemLog(string.Format("cliente_asterisk_onQueueMember - Falha no discador da campanha {0}. Método: {1} Mensagem: {2}", processoCampanha.Campanha.Nome.ToUpper(), "clienteAsterisk_onQueueMember(long,string, QueueMemberStatus)".ToUpper(), ex.Message), Common.LoggerType.ERRO);
            }
            finally { }
        }

        public static void cliente_asterisk_onQueueEntry(Silver.AsteriskClient.DTO.RelCampanhaQueueStatus _RelCampanhaQueueStatus)
        {
            lstRelCampanhaQueueStatus.Add(_RelCampanhaQueueStatus);
        }

        public static void cliente_asterisk_onQueueStatusComplete()
        {
            RelCampanhaDB relCampanhaDB = new BLL.RelCampanha(ref comandos_asterisk, ref escuta_asterisk).Obter(processoCampanha.Campanha.Id);

            if (relCampanhaDB == null)
            {
                relCampanhaDB = new RelCampanhaDB();
                relCampanhaDB.IdCampanha = processoCampanha.Campanha.Id;

                new BLL.RelCampanha(ref comandos_asterisk, ref escuta_asterisk).Inserir(relCampanhaDB);
            }

            XmlSerializer xml = new XmlSerializer(typeof(List<Silver.AsteriskClient.DTO.RelCampanhaQueueStatus>), "RelCampanhaQueueStatus");
            StringWriter strWriter = new StringWriter();
            xml.Serialize(strWriter, lstRelCampanhaQueueStatus);
            relCampanhaDB.QueueStatus = strWriter.ToString();

            new BLL.RelCampanha(ref comandos_asterisk, ref escuta_asterisk).Alterar(relCampanhaDB);
        }

        public static void cliente_asterisk_onSaiuDePausa(long Ramal)
        {
            try
            {
                var x = processoCampanha.Operadores.Where(c => c.Ramal.Equals(Ramal)).FirstOrDefault();
                if (x != null)
                {
                    if (x.Status == EstadoUsuario.Pausa)
                    {
                        x.Status = EstadoUsuario.Aguardando;

                        ConfigurarComunicacaoAsterisk();

                        comandos_asterisk.StatusFila(processoCampanha.Campanha.Nome.Trim().Replace(' ', '_'));
                        var msg = String.Format("clienteAsterisk_onSaiuDePausa".PadRight(20) + ": Ramal - {0}", Ramal);
                        MensagemLog(msg);
                    }

                    RamalOnline = true;
                }
            }
            catch (Exception ex)
            {
                MensagemLog(string.Format("cliente_asterisk_onSaiuDePausa - Falha no discador da campanha {0}. Método: {1} Mensagem: {2}", processoCampanha.Campanha.Nome.ToUpper(), "clienteAsterisk_onSaiuDePausa(long)".ToUpper(), ex.Message));
            }
            finally
            {
            }
        }

        public static void cliente_asterisk_onRamalOcupado(long Ramal)
        {
            MensagemLog(string.Format("cliente_asterisk_onRamalOcupado".PadRight(20) + ": Ramal - {0}", Ramal.ToString()));
        }

        public static void cliente_asterisk_onRamalNaoAtendeu(long Ramal)
        {
            MensagemLog(string.Format("cliente_asterisk_onRamalNaoAtendeu".PadRight(20) + ": Ramal - {0}", Ramal.ToString()));
        }

        public static void cliente_asterisk_onRamalDiscando(long Ramal)
        {
            MensagemLog(string.Format("cliente_asterisk_onRamalDiscando".PadRight(20) + ": Ramal - {0}", Ramal.ToString()));
        }

        public static void cliente_asterisk_onRamalDesligou(long Ramal, string Causa)
        {
            try
            {
                var x = processoCampanha.Operadores.Where(c => c.Ramal.Equals(Ramal)).FirstOrDefault();
                if (x != null)
                {
                    x.Status = EstadoUsuario.Aguardando;
                    ConfigurarComunicacaoAsterisk();
                    comandos_asterisk.StatusFila(processoCampanha.Campanha.Nome.Trim().Replace(' ', '_'));
                    new BLL.LogRamal().Cadastrar(new LogRamal { Evento = (int)EventoRamal.RamalDesligou, Ramal = Ramal });

                    var ramal_idle = ControleIdleRamais.Where(r => r.Ramal.Equals(x.Ramal)).FirstOrDefault();
                    if (ramal_idle != null)
                    {
                        ramal_idle.Desligou = DateTime.Now;
                        ramal_idle.ExistePausa = false;
                    }

                    var msg = String.Format("clienteAsterisk_onRamalDesligou".PadRight(20) + ": Ramal - {0}", Ramal);
                    MensagemLog(msg);
                }
            }
            catch (Exception ex)
            {
                MensagemLog(string.Format("cliente_asterisk_onRamalDesligou - Falha no discador da campanha {0}. Método: {1} Mensagem: {2}", processoCampanha.Campanha.Nome.ToUpper(), "clienteAsterisk_onRamalDesligou(long,string)".ToUpper(), ex.Message));
            }
        }

        public static void cliente_asterisk_onRamalChamando(long Ramal)
        {
            MensagemLog(string.Format("cliente_asterisk_onRamalChamando".PadRight(20) + ": Ramal - {0}", Ramal.ToString()));
        }

        public static void cliente_asterisk_onRamalAtendeu(long Ramal, string LinhaConectada)
        {
            try
            {
                var operador = processoCampanha.Operadores.Where(c => c.Ramal.Equals(Ramal)).FirstOrDefault();
                if (operador == null) return;

                operador.Status = EstadoUsuario.Atendimento;

                var msg = String.Format("clienteAsterisk_onRamalAtendeu".PadRight(20) + ": Ramal - {0}".PadRight(10) + ": Telefone - {1}", Ramal, LinhaConectada);
                MensagemLog(msg, Common.LoggerType.INFO, ConsoleColor.DarkMagenta);

                var telefones = processoCampanha.Telefones.Where(c => c.TelefoneTratado.Equals(LinhaConectada)).FirstOrDefault();
                AlteraStatusTelefone(telefones.TelefoneTratado, SilverStatus.Finalizado, Ramal);

                Carga cpf_carga = null;
                cpf_carga = processoCampanha.Cargas.Where(x => x.Id.Equals(telefones.IdCarga)).FirstOrDefault();

                var ramal_idle = ControleIdleRamais.Where(r => r.Ramal.Equals(operador.Ramal)).FirstOrDefault();
                if (ramal_idle != null)
                {
                    ramal_idle.Atendeu = DateTime.Now;
                    if (!ramal_idle.ExistePausa)
                        Task.Factory.StartNew(() => new Silver.BLL.Idle().Inserir(new Idle() { Ramal = ramal_idle.Ramal, Atendeu = ramal_idle.Atendeu, Desligou = ramal_idle.Desligou, IdHistorico = processoCampanha.IdHistorico }));

                    ramal_idle.ExistePausa = false;
                }

                if (cpf_carga != null)
                {
                    var CPF = cpf_carga.Chave1;
                    if (CPF != null)
                    {
                        var url = string.Format(ConfigurationManager.AppSettings["application.url.crm"] + "&Telefone={1}" + "&IdHistorico={2}" + "&IdCarga={3}" + "&IdTelefone={4}", CPF, LinhaConectada, processoCampanha.IdHistorico, cpf_carga.Id, telefones.TelId);
                        new BLL.UsuarioLogado().Atualizar(new UsuarioLogado { Ramal = Ramal, Url = url, Contato = cpf_carga.Chave2 });
                        new BLL.LogRamal().Cadastrar(new LogRamal { Evento = (int)EventoRamal.RamalAtendeu, Ramal = Ramal, TelId = telefones.TelId.ToInt32() });

                        //Caso o cliente atenda neste telefone, os demais serão atualizados para outro status
                        var outros_telefones = processoCampanha.Telefones.Where(c => c.Id.Equals(cpf_carga.Id)).ToList<CargaTelefoneRobo>().Where(t => t.Status.Equals(1));
                        foreach (var t in outros_telefones)
                            AlteraStatusTelefone(t.Telefone, SilverStatus.Cliente_Atendeu_Em_Outro_Numero);

                        return;
                    }
                    else if (CPF == null)
                    {
                        cpf_carga = new BLL.Carga().SelectPeloTelefone(LinhaConectada.Substring(LinhaConectada.Length - 8));
                        if (cpf_carga.Chave1 != null)
                            CPF = cpf_carga.Chave1;
                        else
                        {
                            cpf_carga = new BLL.Carga().SelectPeloTelefone(LinhaConectada.Substring(LinhaConectada.Length - 9));
                            if (cpf_carga == null)
                                CPF = "000000000000000";
                            else
                                CPF = cpf_carga.Chave1;

                            var url = string.Format(ConfigurationManager.AppSettings["application.url.crm"] + "&Telefone={1}" + "&IdHistorico={2}" + "&IdCarga={3}" + "&IdTelefone={4}", CPF, LinhaConectada, processoCampanha.IdHistorico, cpf_carga.Id, telefones.TelId);
                            new BLL.UsuarioLogado().Atualizar(new UsuarioLogado { Ramal = Ramal, Url = url });
                            new BLL.LogRamal().Cadastrar(new LogRamal { Evento = (int)EventoRamal.RamalAtendeu, Ramal = Ramal, TelId = telefones.TelId.ToInt32() });

                            //Caso o cliente atenda neste telefone, os demais serão atualizados para outro status
                            var outros_telefones = processoCampanha.Telefones.Where(c => c.Id.Equals(cpf_carga.Id)).ToList<CargaTelefoneRobo>().Where(t => t.Status.Equals(1));
                            foreach (var t in outros_telefones)
                                AlteraStatusTelefone(t.Telefone, SilverStatus.Cliente_Atendeu_Em_Outro_Numero);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MensagemLog(string.Format("cliente_asterisk_onRamalAtendeu - Falha no discador da campanha {0}. Método: {1} Mensagem: {2}", processoCampanha.Campanha.Nome.ToUpper(), "clienteAsterisk_onRamalAtendeu(long,string)".ToUpper(), ex.Message));
            }
            finally
            {
            }
        }

        public static void cliente_asterisk_onError(AsteriskClientException Erro)
        {
            try
            {
                MensagemLog(string.Format("cliente_asterisk_onError - ERRO NA APLICAÇÃO! ", Erro.Message));
            }
            catch (Exception ex)
            {
                MensagemLog(string.Format("cliente_asterisk_onError - Falha no discador da campanha {0}. Método: {1} Mensagem: {2}", processoCampanha.Campanha.Nome.ToUpper(), "clienteAsterisk_onError(AsteriskClientException)".ToUpper(), ex.Message));
            }
            finally
            {
            }
        }

        public static void cliente_asterisk_onEntrouEmPausa(long Ramal)
        {
            try
            {
                var u = processoCampanha.Operadores.Where(o => o.Ramal.Equals(Ramal)).FirstOrDefault();
                if (u != null)
                {
                    if (u != null)
                        u.Status = EstadoUsuario.Pausa;

                    var msg = String.Format("clienteAsterisk_onEntrouEmPausa".PadRight(20) + ": Ramal - {0}", Ramal);
                    MensagemLog(msg);

                    if (processoCampanha.Operadores.Where(c => c.Status.Equals(EstadoUsuario.Aguardando)).Count() > 0)
                        RamalOnline = true;
                    else
                        RamalOnline = false;

                    var ramal_idle = ControleIdleRamais.Where(r => r.Ramal.Equals(u.Ramal)).FirstOrDefault();
                    if (ramal_idle != null)
                        ramal_idle.ExistePausa = true;
                }

            }
            catch (Exception ex)
            {
                MensagemLog(string.Format("cliente_asterisk_onEntrouEmPausa - Falha no discador da campanha {0}. Método: {1} Mensagem: {2}", processoCampanha.Campanha.Nome.ToUpper(), "clienteAsterisk_onEntrouEmPausa(long)".ToUpper(), ex.Message));
            }
        }

        public static void cliente_asterisk_onDestinoOcupado(string Telefone)
        {
            try
            {
                if (!TelefonesPercursos.ContainsKey(Telefone))
                    return;

                Task.Factory.StartNew(() => AlteraStatusTelefone(Telefone, SilverStatus.Ocupado));
                Task.Factory.StartNew(() => GerarLigacoesCampanha(1));

                var msg = String.Format("clienteAsterisk_onDestinoOcupado".PadRight(20) + ": Telefone - {0}", Telefone);
                MensagemLog(msg, color: ConsoleColor.Red);

                if (StatusTelefoneAux.ContainsKey(Telefone))
                    StatusTelefoneAux[Telefone] = "Ocupado";
            }
            catch (Exception ex)
            {
                MensagemLog(string.Format("cliente_asterisk_onDestinoOcupado - Falha no discador da campanha {0}. Método: {1} Mensagem: {2}", processoCampanha.Campanha.Nome.ToUpper(), "clienteAsterisk_onDestinoOcupado(string)".ToUpper(), ex.Message));
            }
            finally
            {
            }
        }

        public static void cliente_asterisk_onDestinoNaoAtendeu(string Telefone)
        {
            try
            {
                if (!TelefonesPercursos.ContainsKey(Telefone))
                    return;

                AlteraStatusTelefone(Telefone, SilverStatus.NaoAtende);
                Task.Factory.StartNew(() => GerarLigacoesCampanha(1));

                //DiscarMaisUmTelefone(Telefone);

                var msg = String.Format("clienteAsterisk_onDestinoNaoAtendeu".PadRight(20) + ": Telefone - {0}", Telefone);
                MensagemLog(msg, color: ConsoleColor.Red);

                if (StatusTelefoneAux.ContainsKey(Telefone))
                    StatusTelefoneAux[Telefone] = "Não Atende";
            }
            catch (Exception ex)
            {
                MensagemLog(string.Format("cliente_asterisk_onDestinoNaoAtendeu - Falha no discador da campanha {0}. Método: {1} Mensagem: {2}", processoCampanha.Campanha.Nome.ToUpper(), "clienteAsterisk_onDestinoNaoAtendeu(string)".ToUpper(), ex.Message));
            }
            finally
            {
            }
        }

        public static void cliente_asterisk_onDestinoDiscando(string Telefone)
        {
            try
            {
                if (!TelefonesPercursos.ContainsKey(Telefone))
                    return;

                var message = "clienteAsterisk_onDestinoDiscando";
                var msg = String.Format(message.PadLeft(20, '*') + ": Telefone - {0}", Telefone);
                MensagemLog(msg, color: ConsoleColor.Yellow);

                if (StatusTelefoneAux.ContainsKey(Telefone))
                    StatusTelefoneAux[Telefone] = "Discando";
            }
            catch (Exception ex)
            {
                MensagemLog(string.Format("cliente_asterisk_onDestinoDiscando - Falha no discador da campanha {0}. Método: {1} Mensagem: {2}", processoCampanha.Campanha.Nome.ToUpper(), "clienteAsterisk_onDestinoDiscando(string)".ToUpper(), ex.Message));
            }
            finally
            {
            }
        }

        public static void cliente_asterisk_onDestinoDesligou(string Telefone, string Causa)
        {
            try
            {
                if (!TelefonesPercursos.ContainsKey(Telefone))
                    return;

                AlteraStatusTelefone(Telefone, SilverStatus.Finalizado);

                var msg = String.Format("clienteAsterisk_onDestinoDesligou".PadRight(20) + ": Telefone - {0}".PadRight(10) + ": Causa - {1}", Telefone, Causa);
                MensagemLog(msg, color: ConsoleColor.Red);

                if (StatusTelefoneAux.ContainsKey(Telefone))
                    StatusTelefoneAux[Telefone] = "Desligou - " + Causa;
            }
            catch (Exception ex)
            {
                MensagemLog(string.Format("cliente_asterisk_onDestinoDesligou - Falha no discador da campanha {0}. Método: {1} Mensagem: {2}", processoCampanha.Campanha.Nome.ToUpper(), "clienteAsterisk_onDestinoDesligou(string,string)".ToUpper(), ex.Message));
            }
            finally
            {
            }
        }

        public static void cliente_asterisk_onDestinoChamando(string Telefone)
        {
            try
            {
                if (!TelefonesPercursos.ContainsKey(Telefone))
                    return;

                AlteraStatusTelefone(Telefone, SilverStatus.EmAndamento);
                var msg = String.Format("clienteAsterisk_onDestinoChamando".PadRight(20) + ": Telefone - {0}", Telefone);
                MensagemLog(msg, color: ConsoleColor.Blue);

                if (StatusTelefoneAux.ContainsKey(Telefone))
                    StatusTelefoneAux[Telefone] = "Chamando";
            }
            catch (Exception ex)
            {
                MensagemLog(string.Format("cliente_asterisk_onDestinoChamando - Falha no discador da campanha {0}. Método: {1} Mensagem: {2}", processoCampanha.Campanha.Nome.ToUpper(), "clienteAsterisk_onDestinoChamando(string)".ToUpper(), ex.Message));
            }
            finally
            {
            }
        }

        public static void cliente_asterisk_onDestinoAtendeu(string Telefone)
        {
            try
            {
                if (!TelefonesPercursos.ContainsKey(Telefone))
                    return;

                if (StatusTelefoneAux.ContainsKey(Telefone))
                    StatusTelefoneAux[Telefone] = "Atendeu";

                CargaTelefoneRobo telefone = AlteraStatusTelefone(Telefone, SilverStatus.Atendido);

                if (telefone == null) return;
                Carga carga = processoCampanha.Cargas.Where(c => c.Id.Equals(telefone.IdCarga)).FirstOrDefault();
                carga.Status = (int)SilverStatus.Atendido;

                var msg = String.Format("clienteAsterisk_onDestinoAtendeu".PadRight(20) + ": Telefone - {0}", Telefone);
                MensagemLog(msg, color: ConsoleColor.Cyan);
            }
            catch (Exception ex)
            {
                MensagemLog(string.Format("cliente_asterisk_onDestinoAtendeu - Falha no discador da campanha {0}. Método: {1} Mensagem: {2}", processoCampanha.Campanha.Nome.ToUpper(), "clienteAsterisk_onDestinoAtendeu(string)".ToUpper(), ex.Message));
            }
            finally
            {
            }
        }

        #endregion Event's

        #region Regras de Discagem

        static int contador_do_andamento = 1;

        static void GerarLigacoesCampanha(long Agressividade)
        {
            Agressividade = Agressividade == 0 ? processoCampanha.Campanha.Agressividade : Agressividade;
            List<CargaTelefoneRobo> consultaTelefones = ConsultaTelefones(Agressividade);

            foreach (var c in consultaTelefones)
                fila_discagem.Enqueue(c);

            if (OnSolicitacaoNovaLigacao != null)
                OnSolicitacaoNovaLigacao();

            if (contador_do_andamento == 50)
            {
                Task.Factory.StartNew(() => AtualizarAndamento());
                contador_do_andamento = 1;
            }
            else
                contador_do_andamento++;
        }

        static void TrataTelefone()
        {
            foreach (var c in processoCampanha.Telefones)
                c.TelefoneTratado = TrataTelefone(c);
        }

        static string TrataTelefone(CargaTelefone Tel)
        {
            var Retorno = string.Empty;

            try
            {
                if (Tel.Ddd == string.Empty)
                    return string.Empty;

                if ((int.Parse(Tel.Ddd) == 11) && (!LDDD11.Contains(Tel.Telefone.ToString().Substring(0, 4))))
                {
                    Retorno = Tel.Telefone;
                }
                else
                {
                    var protocolo = DefineRotas(new CargaTelefoneRobo()
                    {
                        Ddd = Tel.Ddd,
                        Telefone = Tel.Telefone
                    });

                    Retorno = OperadoraDDD[protocolo] + Tel.Ddd + Tel.Telefone;
                }
            }
            catch (Exception ex)
            {
                MensagemLog(string.Format("TrataTelefone - Telefone não pode ser tratado. DDD: {0}, Número: {1}, Id Tipo: {2}, Id Carga:{3}, Mensagem do Sistema: {4}", Tel.Ddd, Tel.Telefone, Tel.IdTipo, Tel.IdCarga, ex.Message));
                return string.Empty;
            }

            return Retorno;
        }

        static Protocolo DefineRotas(CargaTelefoneRobo Tel)
        {
            //TODO - Montar Rota com prioridade menor quanto todo o canal estiver ocupado.

            var rotas = new BLL.Rota().Obter(true);
            List<Rota> prioridadeRota = new List<Rota>();
            if ("6789".Contains(Tel.Telefone[0])) //Celular
            {
                if (Tel.Ddd.ToInt16() >= 20) //VC3
                    prioridadeRota = rotas.Where(c => c.IdTarifaTipo.Equals(5)).OrderBy(c => c.Prioridade).ToList();
                else if (Tel.Ddd.ToInt16() >= 11) //VC2
                    prioridadeRota = rotas.Where(c => c.IdTarifaTipo.Equals(4)).OrderBy(c => c.Prioridade).ToList();
                else //VC1
                    prioridadeRota = rotas.Where(c => c.IdTarifaTipo.Equals(6)).OrderBy(c => c.Prioridade).ToList();
            }
            else //Fixo
            {
                if (Tel.Ddd.ToInt16() >= 20) //Inter
                    prioridadeRota = rotas.Where(c => c.IdTarifaTipo.Equals(3)).OrderBy(c => c.Prioridade).ToList();
                else if (Tel.Ddd.ToInt16() > 11) //Intra
                    prioridadeRota = rotas.Where(c => c.IdTarifaTipo.Equals(2)).OrderBy(c => c.Prioridade).ToList();
                else if ((Tel.Ddd.ToInt16() == 11) && (LDDD11.Contains(Tel.Telefone.ToString().Substring(0, 4)))) //Intra
                    prioridadeRota = rotas.Where(c => c.IdTarifaTipo.Equals(2)).OrderBy(c => c.Prioridade).ToList();
                else //Local
                    prioridadeRota = rotas.Where(c => c.IdTarifaTipo.Equals(1)).OrderBy(c => c.Prioridade).ToList();

            }

            Protocolo Retorno = Protocolo.Telefonica;
            switch (prioridadeRota[0].IdOperadora)
            {
                case 4: Retorno = Protocolo.Telefonica;
                    break;
                case 5: Retorno = Protocolo.GVT;
                    break;
                case 6: Retorno = Protocolo.Transit;
                    break;
                case 7: Retorno = Protocolo.Mahatel;
                    break;
                case 8: Retorno = Protocolo.Algar;
                    break;
                case 9: Retorno = Protocolo.Nexus;
                    break;
                case 10: Retorno = Protocolo.Pontal;
                    break;
            }
            return Retorno;
        }

        static List<CargaTelefoneRobo> ConsultaTelefones(long Agressividade)
        {
            var Retorno = new List<CargaTelefoneRobo>();
            var Auxcargas = processoCampanha.Cargas.Where(c => (c.IdCampanha.Equals(processoCampanha.Campanha.Id)) &&
                                                               (c.Status == (int)SilverStatus.Aguardando ||
                                                                c.Status == (int)SilverStatus.Importado)).OrderByDescending(w => w.Id).ToList();
            #region Fim Mailling
            if (Auxcargas.Count <= 0)
            {
                var historico = new BLL.HistoricoCarga().Obter(processoCampanha.IdHistorico);
                historico.Status = (int)SilverStatus.Discagem_Concluida;
                new BLL.HistoricoCarga().Cadastrar(historico);

                MensagemLog("Processo de discagem finalizado com sucesso. Carga Finalizada.");

                Silver.BLL.ControleSistema.AtualizarPorcentagem(controle_sistema_execucao.Id, 100.00M);
                Silver.BLL.ControleSistema.AtualizarStatusExecucao(SitucaoEventoControleSistema.Finalizado, controle_sistema_execucao.Id);
                Silver.BLL.ControleSistema.IncluirEvento(EventoControleSistema.Parar_Campanha, new ControleSistema { Valor = processoCampanha.Campanha.Id.ToString(), Campanha = processoCampanha.Campanha.Id, Situacao = (int)SitucaoEventoControleSistema.Aguardando, Solicitante = 36 });

                new BLL.LogDiscador().Incluir(new LogDiscador() { DataHora = DateTime.Now, Evento = (int)OperacaoDiscador.FimOperacaoDiscador, IdCampanha = (int)processoCampanha.Campanha.Id });

                controle_sistema_execucao.Situacao = (int)SitucaoEventoControleSistema.Finalizado;
                Environment.Exit(0); //Finaliza o console
            }
            #endregion

            foreach (var carga in Auxcargas)
            {
                var Auxtelefones = (from t in processoCampanha.Telefones
                                    where t.IdCarga.Equals(carga.Id) && t.Status.Equals((int)SilverStatus.Aguardando)
                                    select t).Take(1).ToList<CargaTelefoneRobo>();


                if (Auxtelefones.Count == 0)
                {
                    carga.Status = (int)SilverStatus.Finalizado;
                    new Silver.BLL.Carga().Cadastrar(carga);
                    continue;
                }


                Retorno.Add(Auxtelefones.FirstOrDefault() as CargaTelefoneRobo);
                Auxtelefones.FirstOrDefault().Status = (int)SilverStatus.EmAndamento;

                carga.Status = (int)SilverStatus.EmAndamento;
                new Silver.BLL.Carga().Cadastrar(carga);


                if (Retorno.Count >= Agressividade)
                    break;

            };

            return Retorno;
        }

        static CargaTelefoneRobo AlteraStatusTelefone(string Telefone, SilverStatus status, long ramal = 0)
        {
            try
            {
                BLL.CargaTelefone servico_carga_telefone = new BLL.CargaTelefone();
                //TODO - Possível problema: Se existirem telefones com o mesmo número e com ddd diferente, apenas o primeiro telefone será atualizado,
                //Se o telefone a ser atualizado for o posterior ao primeiro, este não será atualizado. Analisar junto ao Ali
                CargaTelefoneRobo telefone = processoCampanha.Telefones.Where(c => c.TelefoneTratado.Equals(Telefone)).FirstOrDefault();
                if (telefone == null)
                {
                    MensagemLog("Alerta!- Telefone não encontrado para alterar status", Common.LoggerType.ERRO, ConsoleColor.DarkGreen);
                    return telefone;
                }

                telefone.Status = (int)status;
                if (ramal > 0)
                    telefone.Ramal = ramal;

                var task = System.Threading.Tasks.Task.Factory.StartNew(() => { new BLL.CargaTelefone().Cadastrar(telefone); });
                task.Wait();


                switch (status)
                {
                    case SilverStatus.Ocupado:
                    case SilverStatus.NaoAtende:
                    case SilverStatus.AMD:
                    case SilverStatus.Desconhecido:
                    case SilverStatus.SemSucessoNaDiscagem:
                    case SilverStatus.Invalido:
                        Carga carga = processoCampanha.Cargas.Where(c => c.Id.Equals(telefone.IdCarga)).FirstOrDefault();
                        if (carga.Status == (int)SilverStatus.EmAndamento)
                            carga.Status = (int)SilverStatus.Aguardando;
                        break;
                    default:
                        break;
                }

                return telefone;
            }
            catch (Exception ex)
            {
                MensagemLog(string.Format("Falha no discador da campanha {0}. Método: {1} Mensagem: {2}", processoCampanha.Campanha.Nome.ToUpper(), "AlteraStatusTelefone(string,TelefoneStatus)".ToUpper(), ex.Message));
            }
            finally { }
            return null;
        }

        static void ProcessoDiscador()
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

        #region Conexão Serviço Discagem

        static ProxyCliente conexao;

        static bool discar_proximo_solicitacao = true;

        static void Program_OnSolicitacaoNovaLigacao()
        {
            if (processando_solicitacoes) return;

            processando_solicitacoes = true;
            SolicitarDiscagem();
        }

        static CargaTelefoneRobo ProximaDiscagem()
        {
            Monitor.Enter(fila_discagem);
            try
            {
                return fila_discagem.Dequeue();
            }
            catch { }
            finally { Monitor.Exit(fila_discagem); }
            return null;
        }

        static void SolicitarDiscagem()
        {
            while (fila_discagem.Count > 0)
            {
                try
                {
                    conexao = new ProxyCliente(ConfigurationManager.AppSettings["application.servidordiscagem.ip"], Convert.ToInt32(ConfigurationManager.AppSettings["application.servidordiscagem.porta"]));
                    conexao.OnRespostaServidorDiscagem += conexao_OnRespostaServidorDiscagem;

                    CargaTelefoneRobo cargaTelefoneRobo = ProximaDiscagem();
                    discar_proximo_solicitacao = false;

                    if (cargaTelefoneRobo == null) continue;

                    SolicitacaoDiscagem solicitacao = new SolicitacaoDiscagem();

                    solicitacao.Campanha = processoCampanha.Campanha.Nome.Trim().Replace(' ', '_');
                    solicitacao.Telefone = cargaTelefoneRobo.TelefoneTratado;
                    solicitacao.DDD = cargaTelefoneRobo.Ddd;
                    solicitacao.IdCampanha = processoCampanha.Campanha.Id.ToString();
                    solicitacao.IdTelefone = cargaTelefoneRobo.TelId;
                    solicitacao.TipoTelefone = cargaTelefoneRobo.IdTipo.ToString();

                    conexao.SolicitaDiscagem(solicitacao);

                    if (!TelefonesPercursos.ContainsKey(cargaTelefoneRobo.TelefoneTratado))
                        TelefonesPercursos.Add(cargaTelefoneRobo.TelefoneTratado, 0);

                    if (!ControleDiscagem.ContainsKey(cargaTelefoneRobo.TelefoneTratado))
                        ControleDiscagem.Add(cargaTelefoneRobo.TelefoneTratado, 0);

                    while (!conexao.SolicitacaoRespondida)
                    {
                        System.Threading.Thread.Sleep(50);
                        MensagemLog("Aguardando Resposta...", Common.LoggerType.INFO, ConsoleColor.White);
                    }
                }
                catch (Exception ex)
                {
                    MensagemLog("SolicitarDiscagem() - Falha na aplicação: " + ex.Message + "\n\n" + ex.StackTrace, Common.LoggerType.ERRO, ConsoleColor.Red);
                }
                finally
                {
                    discar_proximo_solicitacao = true;
                }
            }

            processando_solicitacoes = false;
        }

        static void conexao_OnRespostaServidorDiscagem(RespostaSolicitacaoDiscagem resposta)
        {
            if (resposta.MotivoResposta.ToLower().Equals("ok"))
                tentar_processar = false;
            else
            {
                //TODO - Gravar mensagem log discador
                MensagemLog("Resposta da solicitação: " + resposta.MotivoResposta, Common.LoggerType.INFO, ConsoleColor.Red);

                discar_proximo_solicitacao = true;
                tentar_processar = true;
                GerarLigacoesCampanha(1);

                string[] campos = resposta.RespostaSolicitacao.Split('|');
                SolicitacaoDiscagem solicitacao = new SolicitacaoDiscagem()
                {
                    DDD = campos[0],
                    Telefone = campos[1],
                    Campanha = campos[2],
                    IdTelefone = campos[3],
                    TipoTelefone = campos[4],
                    IdCampanha = campos[5]
                };

                var telefone = processoCampanha.Telefones.Where(t => t.TelefoneTratado.Equals(solicitacao.Telefone)).FirstOrDefault();
                if (telefone != null)
                {
                    var carga = processoCampanha.Cargas.Where(c => c.Id.Equals(telefone.IdCarga)).FirstOrDefault();
                    if (carga != null)
                    {
                        if (carga.Status == (int)SilverStatus.Finalizado)
                        {
                            MensagemLog(string.Format("Alterando status da 'Carga/Cliente':Status Anterior: {0}, Status Atual: {1}", Enum.GetName(typeof(SilverStatus), carga.Status), Enum.GetName(typeof(SilverStatus), SilverStatus.Aguardando)), Common.LoggerType.INFO, ConsoleColor.Green);
                            carga.Status = (int)SilverStatus.Aguardando;
                        }


                        MensagemLog(string.Format("Alterando status do telefone:Status Anterior: {0}, Status Atual: {1}", Enum.GetName(typeof(SilverStatus), telefone.Status), Enum.GetName(typeof(SilverStatus), SilverStatus.Aguardando)), Common.LoggerType.INFO, ConsoleColor.Green);
                        telefone.Status = (int)SilverStatus.Aguardando;
                    }
                }
            }

            discar_proximo_solicitacao = true;
            conexao.SolicitacaoRespondida = true;
        }

        #endregion

        static void MensagemLog(string mensagem, Common.LoggerType tipo_log = Common.LoggerType.INFO, ConsoleColor color = ConsoleColor.White)
        {
            string path_logs = Path.Combine(ConfigurationManager.AppSettings["application.path.logs"], string.Format("Logs-{0}-{1}.silver", processoCampanha.Campanha.Nome.Trim().Replace(' ', '_'), DateTime.Now.ToString("ddMMyyyyHH")));

            Silver.Common.Logger.LoggerAsync.pathLog = path_logs;
            Silver.Common.Logger.LoggerAsync.WriteLog(new Common.Logger.LoggerMessage { MessageLogger = mensagem, TypeLogger = tipo_log }, color);
        }
    }
}