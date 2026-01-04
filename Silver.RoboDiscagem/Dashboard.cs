using System;
using System.Collections.Generic;
using System.Configuration;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Silver.AsteriskClient;
using Silver.Common;
using Silver.DTO;
using Silver.RoboDiscagem.BLL;

namespace Silver.RoboDiscagem
{
    public partial class Dashboard : Form
    {
        //Utilizado para identificar quando o discador parar
        EscutaDiretorio ed;

        Silver.BLL.RelChannels requisicao_status_e1 = null;
        Silver.BLL.RelCampanha requisicao_status_fila = null;

        AsteriskCommand comandos_asterisk = null;
        AsteriskListener escuta_asterisk = null;

        //Lista para gerenciar as aplicações que serão utilizadas como escuta para o discador
        List<System.Diagnostics.Process> processos_escuta = new List<System.Diagnostics.Process>();
        List<System.Diagnostics.Process> processos_discagem = new List<System.Diagnostics.Process>();

        //Lista de campanhas ativas
        SortedList<long, System.Diagnostics.Process> processos_campanhas = new SortedList<long, System.Diagnostics.Process>();

        private delegate void ProcessarCargaAsync(Silver.DTO.ControleSistema controle_sistema);

        public bool Discando { get; set; }

        private long contadorLogs = 0;

        public Dashboard()
        {
            InitializeComponent();
        }

        private void ConfigurarConexaoAsterisk()
        {
            comandos_asterisk = new AsteriskClient.AsteriskCommand();
            escuta_asterisk = new AsteriskClient.AsteriskListener();
            escuta_asterisk.Stream_Asterisk = comandos_asterisk.Stream_Asterisk;
            escuta_asterisk.SaidaPadrao = AsteriskClient.SaidaPadraoAsterisk.Delegate;
            escuta_asterisk.IniciarEscuta(comandos_asterisk.Stream_Asterisk);
            requisicao_status_e1 = new Silver.BLL.RelChannels(comandos_asterisk, escuta_asterisk);

        }

        private void EnviarComandoAsterisk(ComandoAsterisk comando_asterisk)
        {
            comandos_asterisk.Comando(comando_asterisk);
        }

        private void GlobalException_onExceptionInForm(object s, ThreadExceptionEventArgs e)
        {
            coreComponent_OnOutputLogs("Falha na aplicação. A mensagem do erro foi: " + e.Exception.Message + ", em: " + s.ToString());
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            lbl_versao.Text = string.Empty;

            tabControl.TabPages.Remove(tabControl.TabPages["tabPage1"]);
            tabControl.TabPages.Remove(tabControl.TabPages["tabPage2"]);

            if (ApplicationDeployment.IsNetworkDeployed)
                lbl_versao.Text += "Versão da Aplicação: " + ApplicationDeployment.CurrentDeployment.CurrentVersion;
            else
                lbl_versao.Text += "Versão da Aplicação: " + Assembly.GetExecutingAssembly().GetName().Version.ToString();

            dgv_requisicoes_historico.AutoGenerateColumns = false;
            dgv_requisicoes.DataSource = Silver.BLL.ControleSistema.ListarControles();

            txt_tempo_requisicoes.LostFocus += txt_atualizacao_grade_LostFocus;
            txt_atualizacao_grade.LostFocus += txt_atualizacao_grade_LostFocus;

            lbl_mensagem_sistema.Text = string.Format("O servidor foi iniciado com sucesso. INÍCIO: {0}", DateTime.Now.ToString("dd/MM/yyy HH:mm:ss"));
            coreComponent_OnOutputLogs(lbl_mensagem_sistema.Text);

            var path_discador = ConfigurationManager.AppSettings["application.path.discador"];
            if (File.Exists("DiscadorOpcao.sil"))
                path_discador = File.ReadAllLines("DiscadorOpcao.sil")[0].Split('|')[1];

            ConfigurarConexaoAsterisk();

            ed = new EscutaDiretorio(path_discador.Substring(0, path_discador.LastIndexOf('\\') + 1));
            ed.OnDiscagemFinalizada += ed_OnDiscagemFinalizada;
            ed.Iniciar();
        }

        private void ed_OnDiscagemFinalizada(long id_campanha)
        {
            var processo_a = processos_campanhas.Where(c => c.Key.Equals(id_campanha)).FirstOrDefault();

            if (processo_a.Value != null)
            {
                try
                {
                    processo_a.Value.Kill();
                }
                catch (Exception ex)
                {
                    coreComponent_OnOutputLogs("Falha na finalização do processo da campanha: " + id_campanha.ToString() + Environment.NewLine + ex.Message);
                }
            }
            else
            {
                lbl_mensagem_sistema.Text = "Não foi possével encontrar a campanha na lista de processos de campanhas ativas.";
                coreComponent_OnOutputLogs(lbl_mensagem_sistema.Text);
            }

            if (list_campanhas_execucao.InvokeRequired)
            {
                list_campanhas_execucao.BeginInvoke((MethodInvoker)delegate()
                {
                    ListViewItem item_campanha = null;
                    foreach (ListViewItem item in list_campanhas_execucao.Items)
                        if (item.Text.Trim().Equals(id_campanha.ToString().Trim()))
                            item_campanha = item;

                    if (item_campanha != null)
                    {
                        list_campanhas_execucao.Items.Remove(item_campanha);
                        list_campanhas_execucao.Refresh();
                        coreComponent_OnOutputLogs(string.Format("O processamento da carga da campanha {0} foi finalizado com sucesso", item_campanha.Text));
                    }
                });
            }
            else
            {
                list_campanhas_execucao.Items.Remove(list_campanhas_execucao.Items.Find(id_campanha.ToString(), true).FirstOrDefault());
                list_campanhas_execucao.Refresh();
            }

            processos_campanhas.Remove(id_campanha);
            lbl_mensagem_sistema.Text = string.Format("Campanha {0} parada com sucesso", id_campanha);
        }

        private void txt_atualizacao_grade_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((sender as ToolStripTextBox).Text.Trim())) return;

            int valor_parse = 0;
            if (!int.TryParse((sender as ToolStripTextBox).Text.Trim(), out valor_parse))
            {
                MessageBox.Show("Valor Inválido!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                (sender as ToolStripTextBox).Text = string.Empty;
                (sender as ToolStripTextBox).Focus();
                return;
            }

            if ((sender as ToolStripTextBox).Name.ToLower().Equals("timer_atualizacao_grade"))
            {
                lbl_mensagem_sistema.Text = "O valor do intervalo do timer de atualização foi atualizado para " + (valor_parse * 1000).ToString();
                coreComponent_OnOutputLogs(lbl_mensagem_sistema.Text);

                timer_atualizacao_grade.Interval = valor_parse * 1000;
                timer_atualizacao_grade.Stop();
                timer_atualizacao_grade.Start();
            }
            else
            {
                lbl_mensagem_sistema.Text = "O valor do intervalo do timer de execução de requisições foi atualizado para " + (valor_parse * 1000).ToString();
                coreComponent_OnOutputLogs(lbl_mensagem_sistema.Text);

                timer_requisicoes.Interval = valor_parse * 1000;
                timer_requisicoes.Stop();
                timer_requisicoes.Start();
            }
        }

        private void coreComponent_OnOutputLogs(string msg)
        {
            string msg_formatada = string.Format("[{0}] - {1}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff"), msg);
            if (listView.InvokeRequired)
            {
                listView.BeginInvoke((MethodInvoker)delegate()
                {
                    contadorLogs++;
                    var item = new ListViewItem(contadorLogs.ToString("0000000000"));

                    if (msg.ToLower().Contains("erro"))
                        item.BackColor = Color.LightCoral;

                    item.SubItems.Add(msg_formatada);
                    item.ImageIndex = 2;
                    listView.Items.Insert(0, item);
                });
            }
            if (Discando && msg.Contains(txt_numero_telefone.Text.Trim()))
            {
                if (lbl_status_discagem.InvokeRequired)
                {
                    lbl_status_discagem.BeginInvoke((MethodInvoker)delegate()
                    {
                        lbl_status_discagem.Text = msg_formatada;
                    });
                }
                else
                {
                    lbl_status_discagem.Text = msg_formatada;
                }
            }
            else
            {
                listView.BeginInvoke((MethodInvoker)delegate()
                {
                    contadorLogs++;
                    var item = new ListViewItem(contadorLogs.ToString("0000000000"));

                    if (msg.ToLower().Contains("erro"))
                    {
                        item.BackColor = Color.LightCoral;
                        item.ForeColor = Color.White;
                    }

                    item.SubItems.Add(msg_formatada);
                    item.ImageIndex = 2;
                    listView.Items.Insert(0, item);
                });
            }
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ddl_campanhas_logs.SelectedIndexChanged -= ddl_campanhas_logs_SelectedIndexChanged;
            CarregarCampanhas();
            ddl_campanhas_logs.SelectedIndexChanged += ddl_campanhas_logs_SelectedIndexChanged;
        }

        private void CarregarCampanhas()
        {
            var campanhas = new Silver.BLL.Campanha().Obter(true);
            grid.DataSource = campanhas;
            ddl_campanha_discar.DataSource = campanhas;
            cbo_campanha.DataSource = campanhas;

            ddl_campanhas_logs.ComboBox.DataSource = campanhas;
            ddl_campanhas_logs.ComboBox.DisplayMember = "Nome";
            ddl_campanhas_logs.ComboBox.ValueMember = "Id";
        }

        private void CarregarCarga(long codigoCampanha)
        {
            gridCarga.DataSource = new Silver.BLL.Carga().SelectPelaCampanha(codigoCampanha);
        }

        private void CarregarTelefones(long codigoCarga)
        {
            gridTelefone.DataSource = new Silver.BLL.CargaTelefone().ObterPelaCarga(codigoCarga);
        }

        private void grid_SelectionChanged(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count > 0)
            {
                CarregarCarga(grid.SelectedRows[0].Cells[0].Value.ToInt64());
            }
        }

        private void gridCarga_SelectionChanged(object sender, EventArgs e)
        {
            if (gridCarga.SelectedRows.Count > 0)
            {
                CarregarTelefones(gridCarga.SelectedRows[0].Cells[0].Value.ToInt64());
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("O sistema iniciará todas as campanhas que estão ativadas. Deseja continuar?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                var campanhas = new Silver.BLL.Campanha().Obter(true);
                foreach (var item in campanhas)
                {
                    if (!processos_campanhas.ContainsKey(item.Id))
                    {
                        processos_campanhas.Add(item.Id, System.Diagnostics.Process.Start(ConfigurationManager.AppSettings["application.path.discador"], item.Id.ToString()));

                        lbl_mensagem_sistema.Text = string.Format("A Campanha {0} foi iniciada com sucesso", item.Nome);
                        coreComponent_OnOutputLogs(lbl_mensagem_sistema.Text);

                        AddItemListCampanhas(item, EventoControleSistema.Iniciar_Campanha);
                    }
                }
            }
        }

        private void btn_discar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_numero_telefone.Text.Trim()))
                return;

            //coreComponent.Discar(txt_numero_telefone.Text.Trim(), Math.Abs(Guid.NewGuid().ToString().GetHashCode()), 1, ddl_campanha_discar.Text, ddl_campanha_discar.SelectedValue.ToInt64()); //TODO- Observar o valor fixo no código para casos de relatórios
            Discando = true;
        }

        private void gridCarga_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            lbl_total_cargas_ativas.Text = gridCarga.Rows.Count.ToString("0000000000");
        }

        private void gridTelefone_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            lbl_total_telefones_ativos.Text = gridTelefone.Rows.Count.ToString("00000");
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

        }

        private void atualizarArquivoSipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new BLL.RoboDiscagemIO().GerarArquivoSIP(coreComponent_OnOutputLogs, ProtocoloSaidaArquivo.SistemArquivo);
            new BLL.RoboDiscagemIO().GerarArquivoQueue(coreComponent_OnOutputLogs, ProtocoloSaidaArquivo.SistemArquivo);

            EnviarComandoAsterisk(ComandoAsterisk.SipReload);
            EnviarComandoAsterisk(ComandoAsterisk.QueueReload);

            lbl_mensagem_sistema.Text = "O arquivo sip foi criado e enviado com sucesso para o servidor Asterisk";
        }

        private void btn_Continuar_Click(object sender, EventArgs e)
        {
            if (!processos_campanhas.ContainsKey((cbo_campanha.SelectedItem as DTO.Campanha).Id))
                processos_campanhas.Add(cbo_campanha.SelectedValue.ToInt64(), System.Diagnostics.Process.Start(ConfigurationManager.AppSettings["application.path.discador"], (cbo_campanha.SelectedItem as DTO.Campanha).Id.ToString()));

            lbl_mensagem_sistema.Text = string.Format("Campanha {0} iniciada com sucesso", (cbo_campanha.SelectedItem as DTO.Campanha).Nome);
        }

        private void btn_pausar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja parar todas as campanhas selecionadas", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                return;

            foreach (ListViewItem item in list_campanhas_execucao.CheckedItems)
            {
                var processo_a = processos_campanhas.Where(c => c.Key.Equals(item.Text.ToInt64())).FirstOrDefault();

                if (processo_a.Value != null)
                {
                    try
                    {
                        processo_a.Value.Kill();
                    }
                    catch (Exception ex)
                    {
                        coreComponent_OnOutputLogs("Falha na finalização do processo: " + ex.Message);
                    }
                }
                else
                {
                    lbl_mensagem_sistema.Text = "Não foi possével encontrar a campanha na lista de processos de campanhas ativas.";
                    coreComponent_OnOutputLogs(lbl_mensagem_sistema.Text);
                    return;
                }

                processos_campanhas.Remove(item.Text.ToInt64());

                lbl_mensagem_sistema.Text = string.Format("Campanha {0} parada com sucesso", item.SubItems[1].Text);

                list_campanhas_execucao.Items.Remove(item);
                list_campanhas_execucao.Refresh();

                coreComponent_OnOutputLogs(lbl_mensagem_sistema.Text);
            }
        }

        private void atualizarArquivoQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new BLL.RoboDiscagemIO().GerarArquivoQueue(coreComponent_OnOutputLogs, ProtocoloSaidaArquivo.SistemArquivo);
            EnviarComandoAsterisk(ComandoAsterisk.QueueReload);
            lbl_mensagem_sistema.Text = "Arquivo criado e enviado com sucesso";
        }

        private void btn_atualizar_auto_CheckedChanged(object sender, EventArgs e)
        {
            btn_01_minuto.Enabled = btn_atualizar_auto.Checked;
            btn_10_minutos.Enabled = btn_atualizar_auto.Checked;
            btn_15_minutos.Enabled = btn_atualizar_auto.Checked;
        }

        private void btn_01_minuto_Click(object sender, EventArgs e)
        {
            btn_01_minuto.Checked = false;
            btn_05_minutos.Checked = false;
            btn_10_minutos.Checked = false;
            btn_15_minutos.Checked = false;
            (sender as ToolStripMenuItem).Checked = true;
        }

        private void btn_iniciar_Click(object sender, EventArgs e)
        {
            if (cbo_campanha.SelectedItem == null)
                return;

            Silver.BLL.ControleSistema.IncluirEvento(EventoControleSistema.Iniciar_Campanha, new DTO.ControleSistema
            {
                Valor = cbo_campanha.SelectedValue.ToString(),
                Campanha = cbo_campanha.SelectedValue.ToInt64(),
                Situacao = (int)SitucaoEventoControleSistema.Aguardando,
                Solicitante = 36
            });

        }

        private void AddItemListCampanhas(DTO.Campanha campanha, EventoControleSistema evento)
        {
            ListViewItem item_campanha = new ListViewItem(campanha.Id.ToString());
            item_campanha.SubItems.Add(campanha.Nome);
            item_campanha.SubItems.Add(campanha.Agressividade.ToString());
            item_campanha.SubItems.Add(DateTime.Now.ToString("HH:mm:ss"));

            item_campanha.SubItems.Add(" Não verificado "); //operador
            item_campanha.SubItems.Add(" Não verificado "); //cliente
            item_campanha.SubItems.Add(" Não verificado "); //telefone

            ListViewItem.ListViewSubItem item_evento = new ListViewItem.ListViewSubItem(item_campanha, Enum.GetName(typeof(EventoControleSistema), evento));
            item_campanha.UseItemStyleForSubItems = true;

            switch (evento)
            {
                case EventoControleSistema.Iniciar_Campanha:
                    item_campanha.BackColor = Color.LightGreen;
                    break;
                case EventoControleSistema.Continuar_Campanha:
                    item_campanha.BackColor = Color.LightYellow;
                    break;
                case EventoControleSistema.Recarregar_Campanha:
                    break;
                case EventoControleSistema.Recarregar_Carga:
                    break;
                case EventoControleSistema.Recarregar_Telefone:
                    break;
                case EventoControleSistema.Recarregar_Sip:
                    break;
                case EventoControleSistema.Recarregar_Queue:
                    break;
                case EventoControleSistema.Processar_Carga:
                    break;
                default:
                    break;
            }
            list_campanhas_execucao.Items.Add(item_campanha);
        }

        private void grid_dashboard_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void timer_requisicoes_Tick(object sender, EventArgs e)
        {
            ExecutarSolicitacoes();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            dgv_requisicoes.DataSource = Silver.BLL.ControleSistema.ListarControles();
        }

        private void dgv_requisicoes_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
        }

        private void ExecutarSolicitacoes()
        {
            long campanha_execucao = 0;
            string solicitacao = string.Empty;
            DTO.ControleSistema controle_sistema_execucao = null;

            try
            {
                foreach (DataGridViewRow row in dgv_requisicoes.Rows)
                {
                    solicitacao = row.Cells[1].Value.ToString();
                    controle_sistema_execucao = Silver.BLL.ControleSistema.ObterControleSistema(row.Cells[0].Value.ToString().ToInt64());
                    var campanha = new Silver.BLL.Campanha().Obter(row.Cells[6].Value.ToInt64());

                    switch (solicitacao)
                    {
                        case "Continuar_Campanha":
                        case "Iniciar_Campanha":
                            campanha_execucao = row.Cells[2].Value.ToInt64();
                            
                            if (processos_campanhas.Count <= 0)
                                new Silver.BLL.SaidaAsterisk().Limpar();

                            if (!processos_campanhas.ContainsKey(campanha_execucao))
                            {
                                var path_discador = ConfigurationManager.AppSettings["application.path.discador"];

                                if (File.Exists("DiscadorOpcao.sil"))
                                    path_discador = File.ReadAllLines("DiscadorOpcao.sil")[0].Split('|')[1];

                                var args_console = string.Format("{0} {1}", campanha_execucao.ToString(), controle_sistema_execucao.Id);
                                processos_campanhas.Add(campanha_execucao, System.Diagnostics.Process.Start(path_discador, args_console));

                                lbl_mensagem_sistema.Text = string.Format("A Campanha {0} foi iniciada com sucesso", campanha.Nome);
                                AddItemListCampanhas(campanha, (EventoControleSistema)Enum.Parse(typeof(EventoControleSistema), solicitacao));
                            }

                            if (processos_escuta.Count <= 0)
                                btn_iniciar_escuta_asterisk_Click(null, null);

                            break;
                        case "Parar_Campanha":

                            campanha_execucao = row.Cells[2].Value.ToInt64();
                            var processo_a = processos_campanhas.Where(c => c.Key.Equals(row.Cells[2].Value.ToInt64())).FirstOrDefault();
                            if (processo_a.Value != null)
                            {
                                try { processo_a.Value.Kill(); }
                                catch (Exception ex) { coreComponent_OnOutputLogs("Falha na finalização da campanha: " + ex.Message + ex.StackTrace); }
                            }

                            processos_campanhas.Remove(row.Cells[2].Value.ToInt64());
                            lbl_mensagem_sistema.Text = string.Format("A Campanha {0} foi parada com sucesso", campanha.Nome);

                            foreach (ListViewItem item in list_campanhas_execucao.Items)
                                if (item.Text.Equals(campanha_execucao.ToString()))
                                    list_campanhas_execucao.Items.Remove(item);

                            list_campanhas_execucao.Refresh();

                            Silver.BLL.ControleSistema.AtualizarStatusExecucao(SitucaoEventoControleSistema.Finalizado, row.Cells[0].Value.ToInt32());
                            Silver.BLL.ControleSistema.AtualizarPorcentagem(controle_sistema_execucao.Id, 100);

                            break;

                        case "Recarregar_Campanha":
                        case "Recarregar_Carga":
                        case "Recarregar_Telefone":
                            campanha_execucao = row.Cells[2].Value.ToInt64();
                            var processo_b = processos_campanhas.Where(c => c.Key.Equals(row.Cells[2].Value.ToInt64())).FirstOrDefault();
                            if (processo_b.Value != null)
                            {
                                try { processo_b.Value.Kill(); }
                                catch (Exception ex) { coreComponent_OnOutputLogs("Falha na finalização da campanha: " + ex.Message + ex.StackTrace); }
                            }

                            processos_campanhas.Remove(row.Cells[2].Value.ToInt64());
                            foreach (ListViewItem item in list_campanhas_execucao.Items)
                                if (item.Text.Equals(campanha_execucao.ToString()))
                                    list_campanhas_execucao.Items.Remove(item);

                            list_campanhas_execucao.Refresh();

                            if (!processos_campanhas.ContainsKey(row.Cells[6].Value.ToInt64()))
                            {
                                var path_discador = ConfigurationManager.AppSettings["application.path.discador"];
                                if (File.Exists("DiscadorOpcao.sil"))
                                    path_discador = File.ReadAllLines("DiscadorOpcao.sil")[0].Split('|')[1];

                                var args_console = string.Format("{0} {1}", campanha_execucao.ToString(), controle_sistema_execucao.Id);
                                processos_campanhas.Add(row.Cells[2].Value.ToInt64(), System.Diagnostics.Process.Start(path_discador, args_console));
                                AddItemListCampanhas(campanha, (EventoControleSistema)Enum.Parse(typeof(EventoControleSistema), solicitacao));
                                lbl_mensagem_sistema.Text = string.Format("A Campanha {0} foi reiniciada com sucesso", campanha.Nome);
                            }

                            Silver.BLL.ControleSistema.AtualizarPorcentagem(controle_sistema_execucao.Id, 1);

                            break;
                        case "Recarregar_Sip":
                            new BLL.RoboDiscagemIO().GerarArquivoSIP(coreComponent_OnOutputLogs, ProtocoloSaidaArquivo.SistemArquivo);
                            new BLL.RoboDiscagemIO().GerarArquivoQueue(coreComponent_OnOutputLogs, ProtocoloSaidaArquivo.SistemArquivo);

                            Silver.BLL.ControleSistema.AtualizarStatusExecucao(SitucaoEventoControleSistema.Finalizado, row.Cells[0].Value.ToInt32());
                            Silver.BLL.ControleSistema.AtualizarPorcentagem(controle_sistema_execucao.Id, 100);

                            EnviarComandoAsterisk(ComandoAsterisk.SipReload);
                            EnviarComandoAsterisk(ComandoAsterisk.QueueReload);

                            lbl_mensagem_sistema.Text = "O arquivo Sip foi criado e enviado com sucesso para o servidor Asterisk";
                            Silver.BLL.MensagemSistema.Cadastrar(new MensagemSistema()
                            {
                                DataHora = DateTime.Now,
                                IdCampanha = controle_sistema_execucao.Campanha,
                                Mensagem = String.Format("Sip.conf atualizado com sucesso. Campanha:<br><b>{0}</b>", new Silver.BLL.Campanha().Obter(controle_sistema_execucao.Campanha).Nome),
                                Visualizada = false
                            });
                            break;
                        case "Recarregar_Queue":

                            new BLL.RoboDiscagemIO().GerarArquivoQueue(coreComponent_OnOutputLogs, ProtocoloSaidaArquivo.SistemArquivo);
                            Silver.BLL.ControleSistema.AtualizarStatusExecucao(SitucaoEventoControleSistema.Finalizado, row.Cells[0].Value.ToInt32());
                            Silver.BLL.ControleSistema.AtualizarPorcentagem(controle_sistema_execucao.Id, 100);

                            EnviarComandoAsterisk(ComandoAsterisk.QueueReload);
                            lbl_mensagem_sistema.Text = "O arquivo Queue foi criado e enviado com sucesso para o servidor Asterisk";

                            Silver.BLL.MensagemSistema.Cadastrar(new MensagemSistema()
                            {
                                DataHora = DateTime.Now,
                                IdCampanha = controle_sistema_execucao.Campanha,
                                Mensagem = String.Format("Queue.conf atualizado com sucesso. Campanha:<br><b>{0}</b>", new Silver.BLL.Campanha().Obter(controle_sistema_execucao.Campanha).Nome),
                                Visualizada = false
                            });

                            break;
                        case "Processar_Carga":
                            Silver.BLL.MensagemSistema.Cadastrar(new MensagemSistema()
                            {
                                DataHora = DateTime.Now,
                                IdCampanha = controle_sistema_execucao.Campanha,
                                Mensagem = String.Format("O Mailing enviado para o servidor está em processamento. Campanha:<br><b>{0}</b>", new Silver.BLL.Campanha().Obter(controle_sistema_execucao.Campanha).Nome),
                                Visualizada = false
                            });

                            ExecutarProcessametoCarga(row.DataBoundItem as DTO.ControleSistema);
                            lbl_mensagem_sistema.Text = string.Format("O processamento das cargas foi iniciado com sucesso!");
                            break;
                        case "Situacao_Fila":
                            requisicao_status_fila = new Silver.BLL.RelCampanha(ref comandos_asterisk, ref escuta_asterisk);

                            requisicao_status_fila.QueueShowEStatus(campanha);
                            Silver.BLL.ControleSistema.AtualizarStatusExecucao(SitucaoEventoControleSistema.Finalizado, row.Cells[0].Value.ToInt32());
                            Silver.BLL.ControleSistema.AtualizarPorcentagem(controle_sistema_execucao.Id, 100);

                            lbl_mensagem_sistema.Text = string.Format("Solicitação de atualização do status da fila realizado com sucesso.");
                            break;
                        case "Situacao_E1":
                            requisicao_status_e1.DGVShowChannels();

                            Silver.BLL.ControleSistema.AtualizarStatusExecucao(SitucaoEventoControleSistema.Finalizado, row.Cells[0].Value.ToInt32());
                            Silver.BLL.ControleSistema.AtualizarPorcentagem(controle_sistema_execucao.Id, 100);

                            lbl_mensagem_sistema.Text = string.Format("Solicitação de atualização do status dos canais E1 realizado com sucesso.");
                            break;

                        case "Iniciar_Pausa":
                            var campanha_iniciar_pausa = new Silver.BLL.Campanha().Obter(controle_sistema_execucao.Campanha);
                            var iniciar_pausa = new Silver.BLL.Pausa().Obter(controle_sistema_execucao.Valor.Split('|')[1].ToInt64());

                            comandos_asterisk.IniciarPausaNaFila
                                (
                                    controle_sistema_execucao.Valor.Split('|')[0].ToInt64(),
                                    campanha_iniciar_pausa.Nome.Trim().Replace(' ', '_'),
                                    iniciar_pausa.Descricao
                                 );

                            coreComponent_OnOutputLogs(string.Format("O ramal: {0}, Campanha: {1}, Pausa: {2} - \'INÍCIO PAUSA\'"
                                , controle_sistema_execucao.Valor.Split('|')[0].ToInt64()
                                , campanha_iniciar_pausa.Nome
                                , iniciar_pausa.Descricao));

                            Silver.BLL.ControleSistema.AtualizarStatusExecucao(SitucaoEventoControleSistema.Finalizado, row.Cells[0].Value.ToInt32());
                            Silver.BLL.ControleSistema.AtualizarPorcentagem(controle_sistema_execucao.Id, 100);

                            break;
                        case "Finalizar_Pausa":
                            var campanha_finalizar_pausa = new Silver.BLL.Campanha().Obter(controle_sistema_execucao.Campanha);
                            comandos_asterisk.FinalizarPausaNaFila
                            (
                                controle_sistema_execucao.Valor.ToInt64(),
                                campanha_finalizar_pausa.Nome.Trim().Replace(' ', '_')
                            );

                            coreComponent_OnOutputLogs(string.Format("O ramal: {0}, Campanha: {1} - \'FIM PAUSA\'"
                               , controle_sistema_execucao.Valor.ToInt64()
                               , campanha_finalizar_pausa.Nome.Trim().Replace(' ', '_')));

                            Silver.BLL.ControleSistema.AtualizarStatusExecucao(SitucaoEventoControleSistema.Finalizado, row.Cells[0].Value.ToInt32());
                            Silver.BLL.ControleSistema.AtualizarPorcentagem(controle_sistema_execucao.Id, 100);

                            break;
                    }

                    coreComponent_OnOutputLogs(lbl_mensagem_sistema.Text);
                    Silver.BLL.ControleSistema.AtualizarHoraExecucao(row.Cells[0].Value.ToInt32());
                }
                list_campanhas_execucao.Refresh();
            }
            catch (Exception ex)
            {
                Silver.BLL.ControleSistema.AtualizarHoraExecucao(solicitacao.ToInt32());
                Silver.BLL.ControleSistema.AtualizarStatusExecucao(SitucaoEventoControleSistema.Erro, solicitacao.ToInt32());
                coreComponent_OnOutputLogs(string.Format("Falha no ciclo de execução. Mensagem: {0}", ex.Message + ex.StackTrace));
            }
            finally
            {
                dgv_requisicoes.DataSource = Silver.BLL.ControleSistema.ListarControles();
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabpage_requisicoes_historico)
            {
                foreach (DataGridViewRow item in dgv_requisicoes_historico.SelectedRows)
                {
                    Silver.BLL.ControleSistema.IncluirEvento((item.DataBoundItem as DTO.ControleSistema));
                    coreComponent_OnOutputLogs(string.Format("O evento {0}:{1} foi restaurado para um nova execução", (item.DataBoundItem as DTO.ControleSistema).Id, (item.DataBoundItem as DTO.ControleSistema).Evento));
                }
            }
            else
                ExecutarSolicitacoes();
        }

        private void segundosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lbl_mensagem_sistema.Text = "O valor do intervalo do timer de execução de requisições foi atualizado para " + (sender as ToolStripMenuItem).Text;
            coreComponent_OnOutputLogs(lbl_mensagem_sistema.Text);

            timer_requisicoes.Interval = (sender as ToolStripMenuItem).Text.Split(' ')[0].ToInt32() * 1000;
            timer_requisicoes.Stop();
            timer_requisicoes.Start();
            txt_tempo_requisicoes.Text = string.Empty;
            foreach (ToolStripMenuItem item in toolStripDropDownButton_requisicao.DropDownItems.OfType<ToolStripMenuItem>())
                item.Checked = false;

            (sender as ToolStripMenuItem).Checked = true;
        }

        private void segundosToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            lbl_mensagem_sistema.Text = "O valor do intervalo do timer de atualização foi atualizado para " + (sender as ToolStripMenuItem).Text;
            coreComponent_OnOutputLogs(lbl_mensagem_sistema.Text);

            timer_atualizacao_grade.Interval = (sender as ToolStripMenuItem).Text.Split(' ')[0].ToInt32() * 1000;
            timer_atualizacao_grade.Stop();
            timer_atualizacao_grade.Start();
            txt_atualizacao_grade.Text = string.Empty;

            foreach (ToolStripMenuItem item in toolStripDropDownButton_Grade.DropDownItems.OfType<ToolStripMenuItem>())
                item.Checked = false;

            (sender as ToolStripMenuItem).Checked = true;
        }

        private void toolStripDropDownButton3_Click(object sender, EventArgs e)
        {
            dgv_requisicoes.DataSource = Silver.BLL.ControleSistema.ListarControles();
        }

        private void btn_pausar_execucao_Click(object sender, EventArgs e)
        {
            var status = "Ativado";
            if (timer_requisicoes.Enabled)
                status = "Pausado";

            timer_requisicoes.Enabled = !timer_requisicoes.Enabled;
            var msg = string.Format("[{0}] - O thread de processamento de requisições foi {1}", DateTime.Now, status);
            coreComponent_OnOutputLogs(msg);
            lbl_mensagem_sistema.Text = msg;
        }

        private decimal CalcularPorcentagemCarga(decimal total_linhas, decimal total_linhas_atual)
        {
            return (total_linhas_atual * 100) / total_linhas;
        }

        private void ProcessandoCargaAsync(DTO.ControleSistema controle_sistema)
        {
            int id_historico = 0;
            int total_clientes = 0;
            int total_telefones = 0;
            decimal total_linhas_atual = 0;
            decimal total_linhas_arquivo = 0;

            try
            {
                DTO.HistoricoCarga historico = null;
                var servico_historico = new Silver.BLL.HistoricoCarga();

                Task.Factory.StartNew(() => Silver.BLL.ControleSistema.AtualizarStatusExecucao(SitucaoEventoControleSistema.Executando, controle_sistema.Id));
                using (var sr = new StreamReader(controle_sistema.Valor))
                {
                    var tipos_telefones = new SortedList<string, string>();

                    var servico_carga = new Silver.BLL.Carga();
                    var servico_carga_telefone = new Silver.BLL.CargaTelefone();
                    var servico_carga_telefone_tipo = new Silver.BLL.CargaTelefoneTipo();

                    //As informações de campanha e historico são gravadas no nome do arquivo quando o arquivo é salvo no servidor para processamento
                    var id_campanha = Path.GetFileName(controle_sistema.Valor).Split('_')[0].ToInt64(); //Campanha
                    id_historico = Path.GetFileName(controle_sistema.Valor).Split('_')[1].Split('.')[0].ToInt32(); //Histórico

                    //Histórico da carga
                    historico = servico_historico.Obter((long)id_historico);

                    //Atualizar status do histórico
                    historico.Status = (int)SilverStatus.Processando_Mailing;

                    Task.Factory.StartNew(() => { servico_historico.Cadastrar(historico); });

                    //Atualizar status das cargas antigas
                    servico_carga.AtualizarCargaAntiga(id_campanha);

                    //Atualizar status dos telefones antigos
                    servico_carga_telefone.AtualizarCargaAntiga(id_campanha);

                    total_linhas_arquivo = File.ReadAllLines(controle_sistema.Valor).Length;

                    var linha = sr.ReadLine();
                    while (!string.IsNullOrEmpty(linha))
                    {
                        total_linhas_atual++;
                        var registro = new DTO.Carga();
                        linha = linha.Replace("\"", string.Empty);

                        if (linha.Substring(linha.Length - 1, 1) == ";")
                            linha = linha.Substring(0, linha.Length - 1);

                        var campos = linha.Split(';');
                        if (campos.Length < 5)
                        {
                            //Atualizar status do histórico
                            historico.Status = (int)SitucaoEventoControleSistema.Documento_Incompativel;
                            historico.DataHoraFim = DateTime.Now;
                            servico_historico.Cadastrar(historico);
                            return;
                        }

                        registro.Chave1 = campos[0];
                        registro.Chave2 = campos[1];
                        registro.DtCarga = DateTime.Now;
                        registro.IdCampanha = id_campanha;
                        registro.IdHistorico = id_historico;
                        registro.Ativo = true;
                        registro.Status = (int)SilverStatus.Aguardando;

                        registro.Id = servico_carga.Cadastrar(registro);
                        total_clientes++;

                        for (var i = 2; i < campos.Length; i += 4)
                        {
                            try
                            {
                                var ddd = campos[i + 1].Length <= 2 ? campos[i + 1] : campos[i + 1].Substring(0, 3);

                                ulong valor_parse = 0;
                                if (!ulong.TryParse(campos[i], out valor_parse) || // Numérico
                                    !ulong.TryParse(campos[i + 1], out valor_parse) || // Numérico
                                    !ulong.TryParse(campos[i + 2], out valor_parse) || // Numérico
                                     ulong.TryParse(campos[i + 3], out valor_parse))   // Não Numérico
                                {
                                    //Atualizar status do histórico
                                    historico.Status = (int)SitucaoEventoControleSistema.Documento_Incompativel;
                                    historico.DataHoraFim = DateTime.Now;
                                    servico_historico.Cadastrar(historico);
                                    coreComponent_OnOutputLogs(string.Format("CARGA - ERRO: O sistema encontrou uma inconsistência na linha {0}. O registro será ignorado.", total_linhas_atual.ToString("00000")));
                                    continue;
                                }

                                var t = new DTO.CargaTelefone()
                                {
                                    Ativo = true,
                                    TelId = campos[i],
                                    Ddd = ddd,
                                    Telefone = campos[i + 2],
                                    IdCarga = registro.Id,
                                    Status = (int)SilverStatus.Aguardando
                                };

                                var _tipo_telefone = campos[i + 3].Trim().ToUpper();
                                if (!tipos_telefones.ContainsKey(_tipo_telefone))
                                {
                                    var tipo_telefone = servico_carga_telefone_tipo.Buscar(_tipo_telefone);
                                    if (tipo_telefone == null)
                                    {
                                        servico_carga_telefone_tipo.Cadastrar(new DTO.CargaTelefoneTipo() { Ativo = true, Descricao = _tipo_telefone });
                                        tipo_telefone = servico_carga_telefone_tipo.Buscar(_tipo_telefone);
                                        tipos_telefones.Add(_tipo_telefone, tipo_telefone.Id.ToString());
                                    }
                                    else
                                        tipos_telefones.Add(_tipo_telefone, tipo_telefone.Id.ToString());
                                }

                                t.IdTipo = tipos_telefones[_tipo_telefone].ToInt64();
                                servico_carga_telefone.Cadastrar(t);
                                total_telefones++;
                            }
                            catch { }
                        }
                        linha = sr.ReadLine();
                        if (total_linhas_atual % 101 == 0)
                            Task.Factory.StartNew(() => Silver.BLL.ControleSistema.AtualizarPorcentagem(controle_sistema.Id, CalcularPorcentagemCarga(total_linhas_arquivo, total_linhas_atual)));
                    }
                }

                historico.Status = (int)SilverStatus.Mailing_Processado;
                historico.DataHoraFim = DateTime.Now;
                historico.TotalCliente = total_clientes;
                historico.TotalTelefone = total_telefones;

                servico_historico.Cadastrar(historico);

                Task.Factory.StartNew(() => Silver.BLL.ControleSistema.AtualizarStatusExecucao(SitucaoEventoControleSistema.Finalizado, controle_sistema.Id));
                Task.Factory.StartNew(() => Silver.BLL.ControleSistema.AtualizarPorcentagem(controle_sistema.Id, CalcularPorcentagemCarga(total_linhas_arquivo, total_linhas_atual)));

                coreComponent_OnOutputLogs(string.Format("CARGA - COMPLETA: Total de linhas processadas: {0}", total_linhas_atual.ToString("00000")));
            }
            catch (Exception ex)
            {
                coreComponent_OnOutputLogs("Falha no funcionamento da aplicação. A mensagem do erro foi: " + ex.Message);
                Silver.BLL.ControleSistema.AtualizarStatusExecucao(SitucaoEventoControleSistema.Erro, controle_sistema.Id);
            }
        }

        private void ExecutarProcessametoCarga(object controle_sistema)
        {
            ProcessarCargaAsync cargaInicial = new ProcessarCargaAsync(ProcessandoCargaAsync);
            IAsyncResult result = cargaInicial.BeginInvoke((DTO.ControleSistema)controle_sistema, ExecutarProcessametoCargaCallback, null);
        }

        private void ExecutarProcessametoCargaCallback(IAsyncResult r)
        {
            //lbl_mensagem_sistema.Text = "O processamento das cargas foi finalizado com sucesso!";
            coreComponent_OnOutputLogs(lbl_mensagem_sistema.Text);
        }

        private void btn_excluir_requisicoes_Click(object sender, EventArgs e)
        {
            if (dgv_requisicoes.SelectedRows.Count <= 0)
            {
                MessageBox.Show("Nenhuma requisição foi selecionada", "Atenção", MessageBoxButtons.OK);
                return;
            }

            if (MessageBox.Show(string.Format("Deseja excluir {0} requisição(ões) selecionada(s)", dgv_requisicoes.SelectedRows.Count.ToString("0000")), "Exclusão de Requisição", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dgv_requisicoes.SelectedRows)
                {
                    Silver.BLL.ControleSistema.AtualizarStatusExecucao(SitucaoEventoControleSistema.Parado, row.Cells[0].Value.ToInt64());
                    Silver.BLL.ControleSistema.AtualizarHoraExecucao(row.Cells[0].Value.ToInt32());
                }
            }

            dgv_requisicoes.DataSource = Silver.BLL.ControleSistema.ListarControles();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            switch (e.TabPageIndex)
            {
                case 0:
                    dgv_requisicoes.DataSource = Silver.BLL.ControleSistema.ListarControles().OrderByDescending(c => c.Id).ToList();
                    break;

                case 1:
                    dgv_requisicoes_historico.DataSource = Silver.BLL.ControleSistema.ListarTodosControles().OrderByDescending(c => c.Id).ToList();
                    break;
                default:
                    break;
            }
        }

        private void dgv_requisicoes_historico_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgv_requisicoes_historico.Rows)
            {
                try
                {
                    var situacao = (DTO.StatusProcessoDiscador)(int)row.Cells[3].Value.ToInt32();
                    row.Cells[6].Value = Enum.GetName(typeof(DTO.SitucaoEventoControleSistema), row.Cells[3].Value.ToInt32());
                }
                catch { }
            }
        }

        private void dgv_requisicoes_historico_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void btn_iniciar_escuta_asterisk_Click(object sender, EventArgs e)
        {
            if (processos_escuta.Count > 0)
                processos_escuta.Clear();

            string dir = Path.GetDirectoryName(Application.ExecutablePath);
            string app_exe = ConfigurationManager.AppSettings["application.path.consoleescuta"];
            string path = Path.Combine(dir, app_exe);

            if (File.Exists("DiscadorOpcao.sil"))
            {
                processos_escuta.Add(System.Diagnostics.Process.Start(File.ReadAllLines("DiscadorOpcao.sil")[1].Split('|')[1]));
            }
            else
            {
                processos_escuta.Add(System.Diagnostics.Process.Start(path));
            }

            coreComponent_OnOutputLogs("Serviço de escuta Asterisk iniciado com sucesso");
        }

        private void btn_logs_limpar_Click(object sender, EventArgs e)
        {
            listView.Items.Clear();
        }

        private void btn_logs_salvar_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(@"C:\Windows\Temp")) return;

            var nome_arquivo = string.Format(@"C:\Windows\Temp\{0}.txt", Guid.NewGuid().ToString());
            using (FileStream fs = new FileStream(nome_arquivo, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                    foreach (ListViewItem item in listView.Items)
                        sw.WriteLine("SEQUENCIAL: " + item.Text + ", MENSAGEM: " + item.SubItems[1].Text);
            }

            lbl_mensagem_sistema.Text = "O arquivo de logs foi exportado com sucesso";
            coreComponent_OnOutputLogs(lbl_mensagem_sistema.Text);

            System.Diagnostics.Process.Start(nome_arquivo);
        }

        private void timer_atualizacao_grade_Tick(object sender, EventArgs e)
        {
            dgv_requisicoes.DataSource = Silver.BLL.ControleSistema.ListarControles();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in list_campanhas_execucao.Items)
                item.Checked = !item.Checked;
        }

        private void btn_parar_escuta_asterisk_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < processos_escuta.Count; i++)
            {
                try
                {
                    processos_escuta[i].Kill();
                }
                catch { }
            }
            processos_escuta.Clear();
            coreComponent_OnOutputLogs("Serviço de escuta Asterisk finalizado com sucesso");
        }

        private void btn_opcoes_Click(object sender, EventArgs e)
        {
            FOpcao f = new FOpcao();
            f.ShowDialog();
        }

        private void pathDiscadorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists("DiscadorOpcao.sil"))
                MessageBox.Show(File.ReadAllLines("DiscadorOpcao.sil")[0].Split('|')[1], "Path Discador", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pathEscultaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists("DiscadorOpcao.sil"))
                MessageBox.Show(File.ReadAllLines("DiscadorOpcao.sil")[1].Split('|')[1], "Path Escuta", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_add_log_Click(object sender, EventArgs e)
        {
            if (ddl_arquivos_logs.SelectedItem == null) return;

            if (tab_logs.TabPages.Count > 1)
                tab_logs.TabPages.Remove(tab_logs.TabPages[1]);

            TabPage tab = new TabPage(ddl_arquivos_logs.SelectedItem.ToString());
            TextBox tb = new TextBox();
            tb.Name = "txt_logs_discador";
            tb.Dock = DockStyle.Fill;
            tb.Multiline = true;
            tb.ScrollBars = ScrollBars.Both;
            tb.BackColor = Color.Black;
            tb.ForeColor = Color.White;
            tab.Controls.Add(tb);

            tb.Text = ddl_arquivos_logs.SelectedItem.ToString() + Environment.NewLine;
            tb.Text += File.ReadAllText(ddl_arquivos_logs.SelectedItem.ToString());

            tab_logs.TabPages.Add(tab);
            tab_logs.SelectedTab = tab;
        }

        private void btn_carregar_campanhas_execucao_Click(object sender, EventArgs e)
        {
            ddl_campanhas_logs.Items.Clear();
            foreach (var c in processos_campanhas)
                ddl_campanhas_logs.Items.Add(c.Value.StartInfo.FileName);
        }

        private void ddl_campanhas_logs_SelectedIndexChanged(object sender, EventArgs e)
        {
            var arquivos_logs = Directory.GetFiles(ConfigurationManager.AppSettings["application.path.logs"]);
            var logs_campanha_selecionada = arquivos_logs.Where(c => c.Contains((ddl_campanhas_logs.SelectedItem as DTO.Campanha).Nome.Replace(' ', '_')));
            ddl_arquivos_logs.Items.Clear();
            foreach (var l in logs_campanha_selecionada)
                ddl_arquivos_logs.Items.Add(l);
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_abrir_discar_Click(object sender, EventArgs e)
        {
            FTelefonar f = new FTelefonar();
            f.ShowDialog();
        }

        private void dgv_requisicoes_historico_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex % 2 == 0)
                e.CellStyle.BackColor = Color.AliceBlue;
            else
                e.CellStyle.BackColor = Color.White;
        }

        private void dgv_requisicoes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex % 2 == 0)
                e.CellStyle.BackColor = Color.AliceBlue;
            else
                e.CellStyle.BackColor = Color.White;
        }

        private void iniciarServiçoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (processos_escuta.Count > 0)
                processos_escuta.Clear();

            string dir = Path.GetDirectoryName(Application.ExecutablePath);
            string app_exe = ConfigurationManager.AppSettings["application.path.servico.discagem"];
            string path = Path.Combine(dir, app_exe);

            if (File.Exists("DiscadorOpcao.sil"))
                processos_discagem.Add(System.Diagnostics.Process.Start(File.ReadAllLines("DiscadorOpcao.sil")[2].Split('|')[1]));
            else
                processos_discagem.Add(System.Diagnostics.Process.Start(path));

            coreComponent_OnOutputLogs("Serviço de Proxy de Discagem iniciado com sucesso");
        }

        private void pararServiçoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < processos_discagem.Count; i++)
            {
                try
                {
                    processos_escuta[i].Kill();
                }
                catch { }
            }
            processos_discagem.Clear();
            coreComponent_OnOutputLogs("Serviço de Proxy de Discagem iniciado com sucesso");
        }
    }
}