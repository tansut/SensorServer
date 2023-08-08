using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Web.Core;
using Kalitte.Sensors.Web.Business;
using Ext.Net.Utilities;
using Ext.Net;

namespace Kalitte.Sensors.Web.UI
{


    public abstract class ViewControlBase<T> : BaseViewUserControl, ICommandSource where T : BusinessBase
    {
        public virtual T BusinessObject { get; set; }

        public virtual void HideAllWindows()
        {
            ControlUtils.FindControls<Window>(this).ForEach(new Action<Window>((w) => { w.Hide(); })); 
        }


        #region ICommandSource Members

        public BusinessBase ControllerObject
        {
            get { return BusinessObject as BusinessBase; }
        }

        #endregion
    }
}
