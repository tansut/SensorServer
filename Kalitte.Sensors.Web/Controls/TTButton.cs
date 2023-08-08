using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ext.Net;
using System.ComponentModel;
using System.Web.UI;
using Ext.Net.Utilities;

namespace Kalitte.Sensors.Web.Controls
{
    public class TTButton : Button
    {
        

        [DefaultValue(true)]
        public bool AutoValidateForm
        {
            get
            {
                if (ViewState["vf"] == null)
                    return true;
                return (bool)ViewState["vf"];
            }
            set
            {
                ViewState["vf"] = value;
            }
        }

        public string AssociatedForm { get; set; }
        public Control FormValidationContainer { get; set; }

        public TTButton()
            : base()
        {
        }



        protected override void PreRenderAction()
        {
            CheckValidationContainer();
            if (AutoValidateForm && !Ext.Net.X.IsAjaxRequest && !string.IsNullOrEmpty(AssociatedForm))
            {
                string[] forms = AssociatedForm.Split(',');
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < forms.Length; i++)
                {
                    sb.Append("#{" + forms[i] + "}.getForm().isValid()");
                    if (i == forms.Length - 1)
                        sb.Append(";");
                    else sb.Append(" && ");

                }
                Listeners.Click.Handler = "return " + sb.ToString() + Listeners.Click.Handler;
                //DirectEvents.Click.EventMask.ShowMask = true;
                //DirectEvents.Click.EventMask.Target = MaskTarget.CustomTarget;
                //DirectEvents.Click.EventMask.CustomTarget = "#{" + forms[0] + "}";
                //DirectEvents.Click.EventMask.Msg = "Processing ...";
            }
            base.PreRenderAction();
        }

        private void CheckValidationContainer()
        {
            if (FormValidationContainer != null && string.IsNullOrEmpty(AssociatedForm) && !Ext.Net.X.IsAjaxRequest)
            {
                List<TTFormPanel> forms = ControlUtils.FindControls<TTFormPanel>(FormValidationContainer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < forms.Count; i++)
                    sb.Append(forms[i].ID + ",");
                if (sb.Length > 0)
                    sb.Remove(sb.Length - 1, 1);
                AssociatedForm = sb.ToString();
            }
        }
    }
}
