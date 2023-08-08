using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Ext.Net;

namespace Kalitte.Sensors.Web.UI
{
    public class BasePage: Page
    {
        public ResourceManager ResMan
        {
            get
            {
                return ResourceManager.GetInstance(this);
            }
        }
    }
}
