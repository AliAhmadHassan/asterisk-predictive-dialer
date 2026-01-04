using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Silver.Common;
using System.Threading;
using Silver.BLL;
using System.Drawing;
using Silver.DTO;
using System.IO;
using System.Configuration;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using Silver.RoboDiscagem.Exceptions;
using Silver.AsteriskClient;
using System.Deployment;
using System.Deployment.Application;
using System.Reflection;
using Silver.RoboDiscagem.BLL;

namespace Silver.RoboDiscagem
{
    public partial class Dashboard
    {
        System.Timers.Timer timer_atualiza_statuscampanha = new System.Timers.Timer(2000);

        System.Timers.Timer timer_logs = new System.Timers.Timer(1000);

        private void timer_atualiza_statuscampanha_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer_atualiza_statuscampanha.Stop();
            if (list_campanhas_execucao.InvokeRequired)
            {
                list_campanhas_execucao.BeginInvoke((MethodInvoker)delegate()
                {
                    foreach (ListViewItem item in list_campanhas_execucao.Items)
                    {
                        var status = new Silver.BLL.Campanha().StatusCampanha(item.Text.ToInt32());
                        item.SubItems[4].Text = status.Operador.ToString("00");
                        item.SubItems[5].Text = status.Carga.ToString("00000");
                        item.SubItems[6].Text = status.Telefone.ToString("00000");
                    }
                });
            }
            else
            {
                foreach (ListViewItem item in list_campanhas_execucao.Items)
                {
                    var status = new Silver.BLL.Campanha().StatusCampanha(item.Text.ToInt32());
                    item.SubItems[4].Text = status.Operador.ToString("00000");
                    item.SubItems[5].Text = status.Carga.ToString("00000");
                    item.SubItems[6].Text = status.Telefone.ToString("00000");
                }
            }
            timer_atualiza_statuscampanha.Start();
        }

        private void timer_requisicao_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var requisicoes = Silver.BLL.ControleSistema.ListarControles();
        }
    }
}
