﻿using System.ServiceProcess;

namespace RestWCFWinService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        static void Main()
        {
//#if DEBUG
//            MyRestWCFRestWinSer myservice = new MyRestWCFRestWinSer();
//            myservice.OnDebug();
//            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
//#else
            ServiceBase[] ServicesToRun;

            ServicesToRun = new ServiceBase[]
            {
                new MyRestWCFRestWinSer()
            };

            ServiceBase.Run(ServicesToRun);
//#endif
        }

    }
}