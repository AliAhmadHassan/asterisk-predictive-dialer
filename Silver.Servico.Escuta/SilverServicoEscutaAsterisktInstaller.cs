using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace Silver.Servico.Escuta
{
    [RunInstaller(true)]
    public partial class SilverServicoEscutaAsterisktInstaller : System.Configuration.Install.Installer
    {
        public SilverServicoEscutaAsterisktInstaller()
        {
            InitializeComponent();
        }
    }
}
