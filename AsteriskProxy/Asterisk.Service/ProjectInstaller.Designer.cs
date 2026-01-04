namespace Asterisk.Service
{
    partial class ProjectInstaller
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
            this.ServerAsterisk = new System.ServiceProcess.ServiceProcessInstaller();
            this.ServerAsteriskInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // ServerAsterisk
            // 
            this.ServerAsterisk.Password = null;
            this.ServerAsterisk.Username = null;
            // 
            // ServerAsteriskInstaller
            // 
            this.ServerAsteriskInstaller.Description = "Asterisk Service";
            this.ServerAsteriskInstaller.DisplayName = "AsteriskService";
            this.ServerAsteriskInstaller.ServiceName = "AsteriskService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ServerAsterisk,
            this.ServerAsteriskInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ServerAsterisk;
        private System.ServiceProcess.ServiceInstaller ServerAsteriskInstaller;
    }
}