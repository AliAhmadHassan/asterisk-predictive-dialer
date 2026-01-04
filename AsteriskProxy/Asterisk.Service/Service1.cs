using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Asterisk.Service
{
    public partial class Service1 : ServiceBase
    {
        private Silver.AsteriskClient.ClienteAsterisk cliente_asterisk = new Silver.AsteriskClient.ClienteAsterisk();

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            cliente_asterisk.IniciarConexao();
        }

        protected override void OnStop()
        {
        }
    }
}
