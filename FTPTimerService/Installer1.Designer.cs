namespace FTPTimerService
{
    partial class Installer1
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
            this.FTPTimerServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            this.FTPTimerServiceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            // 
            // FTPTimerServiceInstaller
            // 
            this.FTPTimerServiceInstaller.ServiceName = "FTPTimer";
            this.FTPTimerServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.FTPTimerServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstaller1_AfterInstall);
            // 
            // FTPTimerServiceProcessInstaller1
            // 
            this.FTPTimerServiceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.FTPTimerServiceProcessInstaller1.Password = null;
            this.FTPTimerServiceProcessInstaller1.Username = null;
            this.FTPTimerServiceProcessInstaller1.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceProcessInstaller1_AfterInstall);
            // 
            // Installer1
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.FTPTimerServiceInstaller,
            this.FTPTimerServiceProcessInstaller1});

        }

        #endregion

        private System.ServiceProcess.ServiceInstaller FTPTimerServiceInstaller;
        private System.ServiceProcess.ServiceProcessInstaller FTPTimerServiceProcessInstaller1;
    }
}