using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Kalitte.Sensors.Commands;
using Kalitte.Sensors.Client;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Web.UI
{
    public abstract class SensorCommandEditor: UserControl
    {


        protected SensorCommand CreateSensorCommand(string type, params object[] parameters)
        {
            Type t = TypesHelper.GetType(type);
            return Activator.CreateInstance(t, parameters) as SensorCommand;
        }

        protected byte[] GetBytes(string data, bool returnNullIfEmpty = true)
        {
            if (returnNullIfEmpty && string.IsNullOrEmpty(data))
                return null;
            else return ASCIIEncoding.ASCII.GetBytes(data);
        }
    }
}
