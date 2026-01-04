namespace Silver.ServerEscuta
{
    partial class ProjectInstallerEscutaRamalDesligou
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SilverServerEscuta = new System.ServiceProcess.ServiceProcessInstaller();
            this.SilverServerEscutaRamalDesligou = new System.ServiceProcess.ServiceInstaller();
            // 
            // SilverServerEscuta
            // 
            this.SilverServerEscuta.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.SilverServerEscuta.Password = null;
            this.SilverServerEscuta.Username = null;
            // 
            // SilverServerEscutaRamalDesligou
            // 
            this.SilverServerEscutaRamalDesligou.Description = "Serviço de escuta ramal desligou asterisk do discador silver";
            this.SilverServerEscutaRamalDesligou.DisplayName = "Silver Server Escuta Ramal Desligou";
            this.SilverServerEscutaRamalDesligou.ServiceName = "SilverServerEscutaRamalDesligou";
            this.SilverServerEscutaRamalDesligou.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstallerEscutaRamalDesligou
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.SilverServerEscuta,
            this.SilverServerEscutaRamalDesligou});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller SilverServerEscuta;
        private System.ServiceProcess.ServiceInstaller SilverServerEscutaRamalDesligou;
    }
}