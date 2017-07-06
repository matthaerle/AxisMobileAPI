namespace RestWCFWinService
{
    #region Namespaces
    using System.ComponentModel;
    using System.ServiceProcess;
    #endregion

    [RunInstaller(true)]
    public partial class MyRestWCFRestWinSerInstaller : System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public MyRestWCFRestWinSerInstaller()
        {
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            service = new ServiceInstaller();
            service.ServiceName = "AxisMobileAPI";
            service.Description = "WCF REST API For Axis Mobile on Android";
            service.DelayedAutoStart = true;
            Installers.Add(process);
            Installers.Add(service);
        }

    }
}








