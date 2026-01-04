namespace Silver.Servico.Escuta
{
    partial class SilverServicoEscutaAsterisktInstaller
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
            this.serviceProcessInstallerEscutaAsterisk = new System.ServiceProcess.ServiceProcessInstaller();
            this.servicoInstalacaoEscutaAsterisk = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstallerEscutaAsterisk
            // 
            this.serviceProcessInstallerEscutaAsterisk.Password = null;
            this.serviceProcessInstallerEscutaAsterisk.Username = null;
            // 
            // servicoInstalacaoEscutaAsterisk
            // 
            this.servicoInstalacaoEscutaAsterisk.DisplayName = "Silver.Servico.EscutaAsterisk";
            this.servicoInstalacaoEscutaAsterisk.ServiceName = "Silver.Servico.EscutaAsterisk";
            this.servicoInstalacaoEscutaAsterisk.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstallerEscutaAsterisk,
            this.servicoInstalacaoEscutaAsterisk});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstallerEscutaAsterisk;
        private System.ServiceProcess.ServiceInstaller servicoInstalacaoEscutaAsterisk;
    }
}