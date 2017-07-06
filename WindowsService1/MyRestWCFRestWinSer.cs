using System.ServiceProcess;
using System.ServiceModel;

namespace RestWCFWinService
{
    public partial class MyRestWCFRestWinSer : ServiceBase
    {
        ServiceHost oServiceHost = null;
        public MyRestWCFRestWinSer()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            oServiceHost = new ServiceHost(typeof(RestWCFServiceLibrary.RestWCFServiceLibrary));
            oServiceHost.Open();
        }

        protected override void OnStop()
        {
            if (oServiceHost != null)
            {
                oServiceHost.Close();
                oServiceHost = null;
            }
        }
    }
}