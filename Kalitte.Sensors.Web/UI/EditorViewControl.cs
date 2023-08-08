using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Web.Business;

namespace Kalitte.Sensors.Web.UI
{
    public abstract class EditorViewControl<T> : ViewControlBase<T> where T : BusinessBase
    {
        public override ViewControlType ControlType
        {
            get { return ViewControlType.Editor; }
        }


    }
}
