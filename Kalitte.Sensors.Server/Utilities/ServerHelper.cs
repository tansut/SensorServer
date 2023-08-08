using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Server.Utilities
{

    public enum StartType
    {
        Service, CommandPrompt,CPromptWithDebug,Help
    }


   public static class ServerHelper
    {
       public static StartType GetStartType(string[] args)
       {
           if ((args != null) && (args.Length != 0))
           {
               if (args.Contains("/c") || args.Contains("-c"))
               {
                   if (args.Contains("/d") || args.Contains("-d"))
                   {
                       return StartType.CPromptWithDebug;
                   }
                   return StartType.CommandPrompt;
               }
               if (args.Contains("/?") || args.Contains("-h"))
               {
                   return StartType.Help;
               }
           }
           return StartType.Service;
       }
    }
}
