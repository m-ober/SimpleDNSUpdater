namespace SimpleDNSUpdater
{
    public partial class ProjectInstaller
    {
        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller serviceInstaller;
        private System.ComponentModel.IContainer components;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller()
            {
                Account = System.ServiceProcess.ServiceAccount.LocalSystem,
                Password = null,
                Username = null
            };

            serviceInstaller = new System.ServiceProcess.ServiceInstaller()
            {
                DisplayName = Config.Instance.LongName,
                ServiceName = Config.Instance.ShortName,
                ServicesDependedOn = Config.Instance.Dependencies.Split(','),
                StartType = System.ServiceProcess.ServiceStartMode.Automatic
            };

            Installers.AddRange(new System.Configuration.Install.Installer[] {
				serviceProcessInstaller,
				serviceInstaller});
        }
    }
}