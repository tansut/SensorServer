using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace Kalitte.Sensors.SetupConfiguration.Helpers
{
    public enum MSMQState
    {
        Installed, InstalledAndRunning, NotInstalled
    }
    public static class MsmqHelper
    {

        public static readonly string MsmqDisplayName = "Microsoft Message Queuing";
        public static readonly string MsmqName = "MSMQ";


        public static MSMQState GetMsmqState()
        {
            ServiceController msmq = ServiceHelper.GetService(MsmqName);
            if (msmq == null)
            {
                return MSMQState.NotInstalled;
            }
            else
            {
                if (msmq.Status == ServiceControllerStatus.Running)
                    return MSMQState.InstalledAndRunning;
                else return MSMQState.Installed;
            }
        }



        public static void StartMsmq()
        {
            ServiceHelper.StartService(MsmqName);
        }
    }
}
