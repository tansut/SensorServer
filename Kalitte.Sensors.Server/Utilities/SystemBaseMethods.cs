using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Kalitte.Sensors.Server.Utilities
{
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    internal class SystemBaseMethods
    {
        public SystemBaseMethods()
        {

        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole(); 
    }
}
