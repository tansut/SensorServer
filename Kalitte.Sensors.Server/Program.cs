using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Kalitte.Sensors.Server.Utilities;
using System.Threading;
using System.IO;

namespace Kalitte.Sensors.Server
{


    static class Program
    {
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {

            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            bool newInstance;

            Mutex m = new Mutex(true, "Kalitte.Sensors.Server", out newInstance);
            var sType = ServerHelper.GetStartType(args);
            if (newInstance)
            {

                if (sType == StartType.Service)
                {
                    ServiceBase[] ServicesToRun;
                    ServicesToRun = new ServiceBase[] { new KalitteSensorServer() };
                    ServiceBase.Run(ServicesToRun);
                }
                else
                {
                    SystemBaseMethods.AllocConsole();
                    KalitteSensorServer kss = new KalitteSensorServer();
                    kss.WaitForStartup = true;
                    try
                    {
                        kss.Run(args);
                    }
                    catch
                    {
                        
#if DEBUG
                        Console.WriteLine("Enter to exit");
                        Console.ReadLine();
#endif
                        Environment.Exit(-1);
                    }
                }
            }
            else
            {
                if (sType != StartType.Service)
                {
                    SystemBaseMethods.AllocConsole();
                    Console.WriteLine("Kalitte.Sensors.Server is already running");
                }
            }
        }



    }
}
