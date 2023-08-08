using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ext.Net;
using Ext.Net.Utilities;

namespace Kalitte.Sensors.Web.Controls
{
    public class TTFormPanel : FormPanel
    {

        private bool readOnly = false;

        public bool ReadOnly
        {
            get
            {
                return readOnly;
            }
            set
            {
                readOnly = value;
            }
        }

        public TTFormPanel()
            : base()
        {
            DefaultAnchor = "100%";
            BodyBorder = false;
            BodyStyle = "background-color:transparent;";
            
        }

        public void SetReadonly(bool isReadonly)
        {
            ReadOnly = isReadonly;
            ControlUtils.FindControls<Field>(this).Each(p => p.ReadOnly = readOnly);
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);            
        }
        protected override void OnLoad(EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest && ReadOnly)
                ControlUtils.FindControls<Field>(this).Each(p => p.ReadOnly = readOnly);
            base.OnLoad(e);
        }

        public void ClearFields()
        {
            ControlUtils.FindControls<Field>(this).Each(p => p.Clear());
            ClearInvalid();
        }

        public override void ClearInvalid()
        {
            base.ClearInvalid();
            ControlUtils.FindControls<Field>(this).Each(p => p.ClearInvalid());
        }

    }
}
