using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ext.Net;
using Kalitte.Sensors.Processing;

namespace Kalitte.Sensors.Web.Controls
{


    public class TTExceptionViewer : TTContainer
    {
        public LastException Exception
        {
            set
            {
                if (value == null)
                {
                    textField.Text = "No Exception";
                    MessageDetails = string.Empty;
                }
                else
                {
                    textField.Text = value.Message;
                    MessageDetails = value.MessageWithDetails;
                }
            }
        }




        string MessageDetails
        {
            get
            {
                if (ViewState["MessageDetails"] == null) ViewState["MessageDetails"] = string.Empty;
                return (string)ViewState["MessageDetails"];
            }
            set
            {
                ViewState["MessageDetails"] = value;
            }
        }


        private TTTextField textField = null;
        private TTWindow messageWindow = null;
        private TTButton button = null;
        private TTTextArea textArea = null;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Layout = "Column";
            textField = new TTTextField();
            textField.ColumnWidth = 0.90;
            textField.ReadOnly = true;
            textField.FieldLabel = "Last Exception";
            textField.HideLabel = true;
            this.Items.Add(textField);
            messageWindow = GetMessagePanel();
            button = new TTButton();
            button.ColumnWidth = 0.10;
            button.AutoValidateForm = false;
            button.AutoHeight = true;
            button.IconAlign = IconAlign.Left;
            button.Icon = Icon.PageError;
            button.StyleSpec = "padding-left:5px;";
            button.DirectClick += new ComponentDirectEvent.DirectEventHandler(button_DirectClick);
            this.Items.Add(button);
            this.DoLayout();

        }

        void button_DirectClick(object sender, DirectEventArgs e)
        {
            textArea.Text = MessageDetails;
            messageWindow.Render(this.Page.Form);
            messageWindow.Show();

        }

        private TTWindow GetMessagePanel()
        {
            TTWindow win = new TTWindow();
            win.ID = "ctlErrorWindow";
            win.Hidden = true;
            win.Collapsible = false;
            win.Modal = false;
            win.Title = "Exception Information";
            win.Width = 650;
            win.Height = 400;
            win.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            win.CloseAction = CloseAction.Close;
            textArea = new TTTextArea();

            TTFitLayout fl = new TTFitLayout();
            fl.Items.Add(textArea);

            win.Items.Add(fl);

            TTButton tb = new TTButton();
            tb.Text = "Close";
            tb.Listeners.Click.Handler = "Ext.getCmp('ctlErrorWindow').close();";

            win.Buttons.Add(tb);
            return win;


        }
    }
}

