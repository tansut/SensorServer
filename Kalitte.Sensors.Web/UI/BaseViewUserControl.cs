using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

using System.Collections;
using Kalitte.Sensors.Web.Controls;
using Kalitte.Sensors.Web.Core;

namespace Kalitte.Sensors.Web.UI
{

    public enum ViewControlType
    {
        Lister,
        Editor
    }


    public abstract class BaseViewUserControl: UserControl
    {

        public abstract ViewControlType ControlType { get; }


        protected string CurrentID
        {
            get
            {
                if (ViewState["ci"] == null)
                    return string.Empty;
                else return ViewState["ci"].ToString();
            }
            set
            {
                ViewState["ci"] = value;
            }
        }

        protected string CurrentDetailID
        {
            get
            {
                if (ViewState["cdi"] == null)
                    return string.Empty;
                else return ViewState["cdi"].ToString();
            }
            set
            {
                ViewState["cdi"] = value;
            }
        }

        protected string State
        {
            get
            {
                if (ViewState["cis"] == null)
                    return string.Empty;
                else return ViewState["cis"].ToString();
            }
            set
            {
                ViewState["cis"] = value;
            }
        }


        protected ViewPageBase PageInstance
        {
            get
            {
                return (ViewPageBase)this.Page;
            }
        }

        public virtual void Show(object sender, CommandInfo command)
        {

        }
    }
}
