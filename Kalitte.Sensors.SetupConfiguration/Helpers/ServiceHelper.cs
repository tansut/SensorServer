using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using System.IO;
using Kalitte.Sensors.SetupConfiguration.Core.Exception;

namespace Kalitte.Sensors.SetupConfiguration.Helpers
{
    public static class ServiceHelper
    {
        public static ServiceController GetService(string serviceName)
        {
            ServiceController service = null;
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController svc in services)
            {
                if (svc.ServiceName == serviceName) service = svc;
            }
            return service;
        }

        public static void StartService(string serviceName)
        {
            ServiceController service = ServiceHelper.GetService(serviceName);
            foreach (var item in service.ServicesDependedOn)
            {
                try
                {
                    if (item.Status != ServiceControllerStatus.Running)
                    {
                        if (item.Status != ServiceControllerStatus.Stopped)
                            item.Stop();
                        item.Start();
                    }
                }
                catch
                {
                    continue;
                }
            }
            service.Start();
        }

        public static string ReadFile(string filePath)
        {
            if (File.Exists(filePath))
            {
               var text =  File.ReadAllText(filePath, Encoding.UTF8);
               return text;
            }
            else throw new TechnicalException("Metadata script file is not found");

        }
    }
}
