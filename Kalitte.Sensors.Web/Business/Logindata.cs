using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Web.Business
{
    [Serializable]
    public sealed class Logindata
    {
        public string Password { get; set; }
        public string ServerHost { get; set; }
        public int Port { get; set; }


    }
}
