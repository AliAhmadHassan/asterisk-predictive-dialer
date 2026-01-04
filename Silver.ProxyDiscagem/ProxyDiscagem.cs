using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Silver.AsteriskClient;
using Silver.Common;
using Silver.Common.Logger;
using Silver.DTO;
using System.Xml.Serialization;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using Silver.AsteriskClient.DTO;
using System.Text;


namespace Silver.ProxyDiscagem
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 22/07/2014
    /// </summary>
    public class ProxyDiscagem
    {
        /// <summary>
        /// Recebe as solicitações de discagem oriundas das conexões tcp
        /// </summary>
        public delegate void SolicitacaoNovaLigacao();

        /// <summary>
        /// Evento que será disparado quando uma nova solicitação de ligação iniciar
        /// </summary>
        public event SolicitacaoNovaLigacao OnSolicitacaoNovaLigacao;

        /// <summary>
        /// Atualiza os canais livres a cada tempo x
        /// </summary>
        public delegate void AtualizarCanaisLivres();

        /// <summary>
        /// Delegate de atualização das quantidades de canais
        /// </summary>
        private AtualizarCanaisLivres servico_atualizacao_canais_livres;

        /// <summary>
        /// Recebe e gerencia conexões tcp dos discadores
        /// </summary>
        private ProxyServerTcp servidor_tcp;

        /// <summary>
        /// Escuta Asterisk
        /// </summary>
        private AsteriskListener asterisk_escuta_dgv;

        /// <summary>
        /// Comandos Asterisk
        /// </summary>
        private AsteriskCommand asterisk_comando_dgv;

        /// <summary>
        /// Fila de discagem, todas solicitações de discagem
        /// </summary>
        private Queue<Silver.DTO.SolicitacaoDiscagem> FilaDeSolicitacoes;

        /// <summary>
        /// Grupo e quantidade de canais livres em cada grupo
        /// </summary>
        private List<DTO.Channels> GruposCanaisLivres;

        /// <summary>
        /// Lista de canais e seus status
        /// </summary>
        private List<Silver.DTO.RelChannels> CanaisDGV = new List<Silver.DTO.RelChannels>();

        /// <summary>
        /// Tempo de inclusão na base de dados da situação dos canais
        /// </summary>
        private DateTime AtualizaRelChannels = new DateTime();

        /// <summary>
        /// Verifica se o metodo DGVShowChannel já retornou
        /// </summary>
        private bool RelChannelsAtualizado = false;

        /// <summary>
        /// Verifica se o serviço já está em execução com as solicitações
        /// </summary>
        private bool processando_solicitacoes = false;

        /// <summary>
        /// Lista dos prefixos que necessitam a inserção da Operadora e DDD
        /// </summary>
        private List<string> ListaDDD11 = new List<string>();

        /// <summary>
        /// Configurar e iniciar o serviço Proxy de Discagem
        /// </summary>
        public void IniciarServico()
        {
            ConfigurarComandosAsterisk();

            AtualizaRelChannels = DateTime.Now;

            servico_atualizacao_canais_livres = new AtualizarCanaisLivres(BeginAtualizarCanais);
            servico_atualizacao_canais_livres.BeginInvoke(new AsyncCallback(EndAtualizarCanais), null);

            CarregarListaDDD11();

            FilaDeSolicitacoes = new Queue<SolicitacaoDiscagem>();

            servidor_tcp = new ProxyServerTcp();
            servidor_tcp.OnSolicitacaoDiscagem += servidor_tcp_OnSolicitacaoDiscagem;
            servidor_tcp.IniciarServico();

            MensagemLog("Aguardando requisições", Silver.Common.LoggerType.INFO, ConsoleColor.White);
        }

        /// <summary>
        /// Finaliza o serviço e libera os recursos utilizados
        /// </summary>
        public void PararServico()
        {

        }


        private void AdicionarSolicitacao(Silver.DTO.SolicitacaoDiscagem solicitacao)
        {
            Monitor.Enter(FilaDeSolicitacoes);
            try
            {
                FilaDeSolicitacoes.Enqueue(solicitacao);

                if (OnSolicitacaoNovaLigacao != null)
                    OnSolicitacaoNovaLigacao();
            }
            catch (Exception ex) { MensagemLog("Falha no servidor. A mensagem do sistema foi:" + ex.Message, Common.LoggerType.ERRO, ConsoleColor.Red); }
            finally { Monitor.Exit(FilaDeSolicitacoes); }
        }

        private SolicitacaoDiscagem ProximaSolicitacao()
        {
            Monitor.Enter(FilaDeSolicitacoes);
            try
            {
                SolicitacaoDiscagem solicitacao = FilaDeSolicitacoes.Dequeue();
                return solicitacao;
            }
            catch (Exception ex) { MensagemLog("ProximaSolicitacao - Falha no servidor. A mensagem do sistema foi:" + ex.Message, Common.LoggerType.ERRO, ConsoleColor.Red); }
            finally { Monitor.Exit(FilaDeSolicitacoes); }
            return null;
        }

        private void Discar()
        {
            try
            {
                while (FilaDeSolicitacoes.Count > 0)
                {
                    Silver.DTO.SolicitacaoDiscagem solicitacaoDiscagem = ProximaSolicitacao();

                    List<Silver.DTO.Rota> prioridadesRota = DefineRotas(solicitacaoDiscagem);
                    Protocolo protocolo = Protocolo.NaoDefinido_SemCanais;

                    DTO.Channels grupo = null;
                    foreach (Silver.DTO.Rota prioridade in prioridadesRota)
                    {
                        Protocolo auxprotocolo = Protocolo.Telefonica;
                        switch (prioridade.IdOperadora)
                        {
                            case 4: auxprotocolo = Protocolo.Telefonica; break;
                            case 5: auxprotocolo = Protocolo.GVT; break;
                            case 6: auxprotocolo = Protocolo.Transit; break;
                            case 7: auxprotocolo = Protocolo.Mahatel; break;
                            case 8: auxprotocolo = Protocolo.Algar; break;
                            case 9: auxprotocolo = Protocolo.Nexus; break;
                            case 10: auxprotocolo = Protocolo.Pontal; break;
                        }

                        grupo = GruposCanaisLivres.Where(g => g.Grupo.Equals((long)auxprotocolo)).FirstOrDefault();
                        if (grupo.Quantidade.Equals(0))
                        {
                            asterisk_comando_dgv.DGVShowChannels();
                            while (RelChannelsAtualizado == false)
                                System.Threading.Thread.Sleep(250);
                        }

                        if (grupo.Quantidade == 0)
                        {
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
                        SolicitarDiscagemAsterisk(solicitacaoDiscagem, protocolo);
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

                processando_solicitacoes = false;
            }
            catch (Exception ex) { MensagemLog("Discar() - Falha no processamento das Discagem. A mensagem do sistema foi: " + ex.Message, Silver.Common.LoggerType.ERRO, ConsoleColor.Red); }
            finally
            {
                processando_solicitacoes = false;
                MensagemLog("Requisições processadas! Aguardando novas Solicitações de Discagem.", Silver.Common.LoggerType.INFO, ConsoleColor.Yellow);
            }
        }

        private void SolicitarDiscagemAsterisk(Silver.DTO.SolicitacaoDiscagem solicitacaoDiscagem, Protocolo protocolo)
        {
            using (AsteriskCommand asterisk_comando_discagem = new AsteriskCommand())
            {
                AsteriskListener asterisk_escuta_discagem = new AsteriskListener();

                asterisk_escuta_discagem.SaidaPadrao = SaidaPadraoAsterisk.Delegate;
                asterisk_escuta_discagem.IniciarEscuta(asterisk_comando_discagem.Stream_Asterisk);

                asterisk_comando_discagem.Discar(new Discagem()
                {
                    Protocolo = ((int)protocolo).ToString(),
                    Telefone = solicitacaoDiscagem.Telefone,
                    IdCampanha = solicitacaoDiscagem.IdCampanha.ToString(),
                    Campanha = solicitacaoDiscagem.Campanha.Trim().Replace(' ', '_'),
                    IdTelefone = solicitacaoDiscagem.IdTelefone.ToString(),
                    TipoTelefone = solicitacaoDiscagem.TipoTelefone.ToString()
                });

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
                        solicitacaoDiscagem.Cliente.Client.Disconnect(false);
                        solicitacaoDiscagem.Cliente.Client.Close();
                        solicitacaoDiscagem.Cliente.Client.Dispose();
                    }
                }
            }
        }

        private List<Rota> DefineRotas(Silver.DTO.SolicitacaoDiscagem solicitacaoDiscagem)
        {
            var rotas = new BLL.Rota().Obter(true);

            List<Silver.DTO.Rota> prioridadeRota = new List<Silver.DTO.Rota>();
            if ("56789".Contains(solicitacaoDiscagem.Telefone[0])) //Celular
            {
                if (solicitacaoDiscagem.DDD.ToInt16() >= 20) //VC3
                    prioridadeRota = rotas.Where(c => c.IdTarifaTipo.Equals(5)).OrderBy(c => c.Prioridade).ToList();
                else if (solicitacaoDiscagem.DDD.ToInt16() >= 11) //VC2
                    prioridadeRota = rotas.Where(c => c.IdTarifaTipo.Equals(4)).OrderBy(c => c.Prioridade).ToList();
                else //VC1
                    prioridadeRota = rotas.Where(c => c.IdTarifaTipo.Equals(6)).OrderBy(c => c.Prioridade).ToList();
            }
            else //Fixo
            {
                if (solicitacaoDiscagem.DDD.ToInt16() >= 20) //Inter
                    prioridadeRota = rotas.Where(c => c.IdTarifaTipo.Equals(3)).OrderBy(c => c.Prioridade).ToList();
                else if (solicitacaoDiscagem.DDD.ToInt16() > 11) //Intra
                    prioridadeRota = rotas.Where(c => c.IdTarifaTipo.Equals(2)).OrderBy(c => c.Prioridade).ToList();
                else if ((solicitacaoDiscagem.DDD.ToInt16() == 11) && (ListaDDD11.Contains(solicitacaoDiscagem.Telefone.ToString().Substring(0, 4)))) //Intra
                    prioridadeRota = rotas.Where(c => c.IdTarifaTipo.Equals(2)).OrderBy(c => c.Prioridade).ToList();
                else //Local
                    prioridadeRota = rotas.Where(c => c.IdTarifaTipo.Equals(1)).OrderBy(c => c.Prioridade).ToList();

            }
            return prioridadeRota;
        }

        private void servidor_tcp_OnSolicitacaoDiscagem(SolicitacaoDiscagem solicitacao)
        {
            AdicionarSolicitacao(solicitacao);
        }

        /// <summary>
        /// Atualiza assincronamente as quantidades de canais
        /// </summary>
        private void BeginAtualizarCanais()
        {
            int tempo_sleep = 1000 * 15;

            if (asterisk_comando_dgv == null)
                ConfigurarComandosAsterisk();

            asterisk_comando_dgv.DGVShowChannels();

            MensagemLog("Iniciando atualização de canais livres", Silver.Common.LoggerType.INFO, ConsoleColor.Magenta);
            System.Threading.Thread.Sleep(tempo_sleep);
        }

        /// <summary>
        /// Informa quando o método BeginAtualizarCanais foi finalizado
        /// </summary>
        /// <param name="resultado_final"></param>
        private void EndAtualizarCanais(IAsyncResult resultado_final)
        {
            AsyncResult resultado = (AsyncResult)resultado_final;
            AtualizarCanaisLivres chamador = (AtualizarCanaisLivres)resultado.AsyncDelegate;
            string formatString = (string)resultado_final.AsyncState;
            chamador.EndInvoke(resultado_final);

            MensagemLog("Finalizando atualização de canais livres", Silver.Common.LoggerType.INFO, ConsoleColor.Magenta);
            servico_atualizacao_canais_livres.BeginInvoke(new AsyncCallback(EndAtualizarCanais), null);
        }

        /// <summary>
        /// Configuração de Comandos do asterisk
        /// </summary>
        private void ConfigurarComandosAsterisk()
        {
            if (asterisk_comando_dgv != null)
                asterisk_comando_dgv.Dispose();

            asterisk_comando_dgv = new AsteriskCommand();

            ConfigurarEscutaAsterisk();
        }

        /// <summary>
        /// Configurações iniciais de escuta como Asterisk
        /// </summary>
        private void ConfigurarEscutaAsterisk()
        {
            if (asterisk_escuta_dgv != null)
                asterisk_escuta_dgv.Dispose();

            asterisk_escuta_dgv = new AsteriskListener();
            asterisk_escuta_dgv.SaidaPadrao = SaidaPadraoAsterisk.Delegate;
            asterisk_escuta_dgv.IniciarEscuta(asterisk_comando_dgv.Stream_Asterisk);

            asterisk_escuta_dgv.OnDGVShowChannels = SincronizarCanais;
        }

        private void SincronizarCanais(string valores)
        {
            CanaisDGV.Clear();

            // Varre os canais separando os campos e inserindo na lista
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

                CanaisDGV.Add(channel);
            }

            var grupos = CanaisDGV.GroupBy(c => c.Grp);
            foreach (var g in grupos)
            {
                if (GruposCanaisLivres.Where(y => y.Grupo.Equals(g.Key)).FirstOrDefault() == null)
                    GruposCanaisLivres.Add(new DTO.Channels() { Grupo = g.Key, Quantidade = 0 });

                DTO.Channels grupo = GruposCanaisLivres.Where(x => x.Grupo.Equals(g.Key)).FirstOrDefault();
                int quantidade = CanaisDGV.Where(c => c.Grp.Equals(g.Key)).Where(d => d.Rsrvd.Equals(0)).Count();

                grupo.Quantidade = quantidade;

                MensagemLog(string.Format("Canais Atualizados: Grupo: {0}, Canais: {1}", g.Key.ToString("00"), quantidade.ToString("00")), Silver.Common.LoggerType.INFO, ConsoleColor.Magenta);
            }

            if (AtualizaRelChannels.AddSeconds(5) >= DateTime.Now)
            {
                XmlSerializer xml = new XmlSerializer(typeof(List<Silver.DTO.RelChannels>));
                StringWriter strWriter = new StringWriter();
                xml.Serialize(strWriter, CanaisDGV);
                new Silver.DAL.RelChannels().Inserir(strWriter.ToString());

                AtualizaRelChannels = DateTime.Now;
            }

            RelChannelsAtualizado = true;
        }

        private void CarregarListaDDD11()
        {
            #region Lista para utilizar DDD 11
            ListaDDD11.Add("4136");
            ListaDDD11.Add("4204");
            ListaDDD11.Add("2123");
            ListaDDD11.Add("2144");
            ListaDDD11.Add("2187");
            ListaDDD11.Add("2198");
            ListaDDD11.Add("2202");
            ListaDDD11.Add("2588");
            ListaDDD11.Add("3799");
            ListaDDD11.Add("4653");
            ListaDDD11.Add("4654");
            ListaDDD11.Add("4655");
            ListaDDD11.Add("2119");
            ListaDDD11.Add("2427");
            ListaDDD11.Add("3402");
            ListaDDD11.Add("4012");
            ListaDDD11.Add("4411");
            ListaDDD11.Add("4412");
            ListaDDD11.Add("4413");
            ListaDDD11.Add("4414");
            ListaDDD11.Add("4415");
            ListaDDD11.Add("4417");
            ListaDDD11.Add("4418");
            ListaDDD11.Add("4494");
            ListaDDD11.Add("4012");
            ListaDDD11.Add("4891");
            ListaDDD11.Add("2277");
            ListaDDD11.Add("2473");
            ListaDDD11.Add("3404");
            ListaDDD11.Add("4031");
            ListaDDD11.Add("4032");
            ListaDDD11.Add("4033");
            ListaDDD11.Add("4035");
            ListaDDD11.Add("4481");
            ListaDDD11.Add("4603");
            ListaDDD11.Add("4409");
            ListaDDD11.Add("4528");
            ListaDDD11.Add("4529");
            ListaDDD11.Add("4711");
            ListaDDD11.Add("4717");
            ListaDDD11.Add("4658");
            ListaDDD11.Add("3408");
            ListaDDD11.Add("4487");
            ListaDDD11.Add("4534");
            ListaDDD11.Add("4538");
            ListaDDD11.Add("4894");
            ListaDDD11.Add("2136");
            ListaDDD11.Add("4592");
            ListaDDD11.Add("4593");
            ListaDDD11.Add("4409");
            ListaDDD11.Add("4529");
            ListaDDD11.Add("4017");
            ListaDDD11.Add("4539");
            ListaDDD11.Add("2136");
            ListaDDD11.Add("2152");
            ListaDDD11.Add("2434");
            ListaDDD11.Add("2816");
            ListaDDD11.Add("2882");
            ListaDDD11.Add("3308");
            ListaDDD11.Add("3378");
            ListaDDD11.Add("3379");
            ListaDDD11.Add("3395");
            ListaDDD11.Add("3446");
            ListaDDD11.Add("3963");
            ListaDDD11.Add("3964");
            ListaDDD11.Add("4491");
            ListaDDD11.Add("4492");
            ListaDDD11.Add("4497");
            ListaDDD11.Add("4521");
            ListaDDD11.Add("4522");
            ListaDDD11.Add("4523");
            ListaDDD11.Add("4525");
            ListaDDD11.Add("4526");
            ListaDDD11.Add("4527");
            ListaDDD11.Add("4531");
            ListaDDD11.Add("4532");
            ListaDDD11.Add("4535");
            ListaDDD11.Add("4581");
            ListaDDD11.Add("4583");
            ListaDDD11.Add("4584");
            ListaDDD11.Add("4587");
            ListaDDD11.Add("4589");
            ListaDDD11.Add("4599");
            ListaDDD11.Add("4601");
            ListaDDD11.Add("4815");
            ListaDDD11.Add("4816");
            ListaDDD11.Add("4014");
            ListaDDD11.Add("4406");
            ListaDDD11.Add("4597");
            ListaDDD11.Add("4895");
            ListaDDD11.Add("4037");
            ListaDDD11.Add("4018");
            ListaDDD11.Add("4896");
            ListaDDD11.Add("4036");
            ListaDDD11.Add("4405");
            ListaDDD11.Add("4021");
            ListaDDD11.Add("4029");
            ListaDDD11.Add("4602");
            ListaDDD11.Add("4714");
            ListaDDD11.Add("4716");
            ListaDDD11.Add("4711");
            ListaDDD11.Add("4713");
            ListaDDD11.Add("4719");
            ListaDDD11.Add("4784");
            ListaDDD11.Add("2434");
            ListaDDD11.Add("3378");
            ListaDDD11.Add("3395");
            ListaDDD11.Add("4493");
            ListaDDD11.Add("4595");
            #endregion
        }

        /// <summary>
        /// Saida de logs e Debug da classe
        /// </summary>
        /// <param name="mensagem">Mensagem de saída</param>
        /// <param name="tipo_log">Tipo do Log: [INFO], [ERRO]</param>
        /// <param name="color">Cor de saída no console</param>
        private void MensagemLog(string mensagem, Common.LoggerType tipo_log = Common.LoggerType.INFO, ConsoleColor color = ConsoleColor.White)
        {
            string path_logs = Path.Combine(
                    ConfigurationManager.AppSettings["application.path.logs"],
                    string.Format("Logs-{0}-{1}.silver", "ServicoProxyDiscagem", DateTime.Now.ToString("ddMMyyyyHH"))
                );

            LoggerAsync.pathLog = path_logs;
            LoggerAsync.WriteLog(new Common.Logger.LoggerMessage { MessageLogger = mensagem, TypeLogger = tipo_log }, color);
        }

    }
}
