using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Kalitte.Sensors.SetupConfiguration.Forms;
using System.Threading;
using Kalitte.Sensors.SetupConfiguration.Core.Exception;

namespace Kalitte.Sensors.SetupConfiguration
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            Application.Run(new MainForm());            
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            ExceptionManager.Manage(e.Exception);
        }

   

   
    }
}
