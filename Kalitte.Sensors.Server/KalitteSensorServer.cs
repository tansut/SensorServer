using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using Kalitte.Sensors.Server.Utilities;
using Kalitte.Sensors.Processing.Core;
using Kalitte.Sensors.Security;

namespace Kalitte.Sensors.Server
{
    partial class KalitteSensorServer : ServiceBase
    {
        ServerManager manager;
        StartType startType = StartType.Service;
        public bool WaitForStartup { get; set; }

        public KalitteSensorServer()
        {
            InitializeComponent();
            WaitForStartup = false;
        }

        protected override void OnStart(string[] args)
        {
            Run(args);
        }


        public void Run(string[] args)
        {
            startType = ServerHelper.GetStartType(args);
            manager = new ServerManager();
            try
            {
                if (startType != StartType.Service)
                {
                    if (startType == StartType.Help)
                    {
                        SetupHelpMenu();
                        Console.ReadLine();
                    }
                    else
                    {
                        if (startType == StartType.CPromptWithDebug)
                        {
                            Console.WriteLine("Waiting for debugger. Press enter to continue");
                            Console.ReadLine();
                        }
                        Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
                        Console.WriteLine("Starting Kalitte Sensor Server ...");
                        manager.Startup(WaitForStartup);
                        Console.WriteLine("Kalitte Sensor Server is running, hit Ctrl-C to exit");
                        while (true)
                        {
                            Console.Read();
                        }
                    }
                }
                else
                {
                    manager.Startup();
                }
            }
            catch (Exception exc)
            {
                if (startType == StartType.CommandPrompt || startType == StartType.CPromptWithDebug)
                    ExceptionManager.WriteConsoleException(exc);

                throw;
            }
        }

        private void SetupHelpMenu()
        {
            Console.WriteLine("Usage: KalitteSensorServer.exe [-c] [-d] [-h]");
            Console.WriteLine("[-c]\tStart from the command line instead of as a service");
            Console.WriteLine("[-d]\tWait on startup for a debugger, requires -c to be specified");
            Console.WriteLine("[-h]\tUsage");

        }

        void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            OnStop();
        }


        protected override void OnStop()
        {
            if (startType == StartType.CommandPrompt || startType == StartType.CPromptWithDebug)
                Console.WriteLine("Server is shutting down ...");
            try
            {
                manager.Shutdown();
                manager = null;
                if (startType == StartType.CommandPrompt || startType == StartType.CPromptWithDebug)
                    Console.WriteLine("Server shutdown successfully.");
                if (startType != StartType.Service)
                    Environment.Exit(0);
            }
            catch (Exception exc)
            {
                if (startType == StartType.CommandPrompt)
                    ExceptionManager.WriteConsoleException(exc);
                else
                {
                    throw;
                }
            }
        }

    }
}
