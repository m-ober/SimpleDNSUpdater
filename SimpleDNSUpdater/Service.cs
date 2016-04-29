using System.ServiceProcess;

namespace SimpleDNSUpdater
{
    public partial class Service : ServiceBase
    {
        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args) 
        {
            SimpleDNSUpdater.Start();
        }

        protected override void OnStop()
        {
            SimpleDNSUpdater.Stop();
        }
    }
}
