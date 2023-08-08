using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.UI;

using Ext.Net;
using Kalitte.Sensors.Web.Security;
using Kalitte.Sensors.Web.Core;

namespace Kalitte.Sensors.Web.Controls
{
    public delegate void CommandHandler(object sender, CommandInfo command);

    public class TTCmdButon : TTButton
    {
        private ICommandHandler mvcControl = null;
        private ICommandSource commandSource = null;
        private bool? isVisible = null;
        public string ConnectedGrid { get; set; }
        public bool ForceGridSelection { get; set; }
        public event CommandHandler Command = null;
        public string MaskMessage { get; set; }
        public bool ShowMask { get; set; }
        public bool UseDirectClick { get; set; }
        private bool clickSet = false;

        public TTCmdButon()
            : base()
        {
            ForceGridSelection = true;
            ShowMask = false;
            MaskMessage = "Processing ...";
            UseDirectClick = true;
        }

        protected ICommandHandler MvcControl
        {
            get
            {
                if (mvcControl == null)
                    mvcControl = GetCommandHandler(true);
                return mvcControl;
            }
        }

        protected ICommandSource CommandSource
        {
            get
            {
                if (commandSource == null)
                    commandSource = GetCommandSource();
                return commandSource;
            }
        }

        protected ICommandHandler GetCommandHandler(bool useThis)
        {
            if (useThis && this is ICommandHandler)
            {
                return (ICommandHandler)this;
            }
            else
            {
                Control t = this;
                while ((t = t.Parent) != null)
                {
                    if (t is ICommandHandler)
                    {
                        return (ICommandHandler)t;

                    }
                }
            }
            return null;
        }

        protected ICommandSource GetCommandSource()
        {

            Control t = this;
            while ((t = t.Parent) != null)
            {
                if (t is ICommandSource)
                {
                    return (ICommandSource)t;

                }
            }

            return null;
        }


        public KnownCommand KnownCommand
        {
            get
            {
                if (ViewState["kc"] == null)
                    return Security.KnownCommand.None;
                else return (Security.KnownCommand)ViewState["kc"];
            }

            set
            {
                if (value == Security.KnownCommand.None)
                    ViewState["kc"] = null;
                else ViewState["kc"] = value;
                if (value != Security.KnownCommand.None)
                    CommandName = value.ToString();
            }
        }


        protected override void OnInit(EventArgs e)
        {
            

            if (!UseDirectClick && !clickSet) {
                clickSet = true;
                this.Click += new EventHandler(TTCmdButon_Click);
            }
            else if (!clickSet)
            {
                clickSet = true;
                this.DirectEvents.Click.Event += new ComponentDirectEvent.DirectEventHandler(TTMvcButon_DirectClick);
            }


            if (!Ext.Net.X.IsAjaxRequest)
            {

                if (ShowMask && !string.IsNullOrEmpty(MaskMessage))
                {
                    DirectEvents.Click.EventMask.ShowMask = true;
                    DirectEvents.Click.EventMask.Msg = MaskMessage;
                    if (!string.IsNullOrEmpty(ConnectedGrid))
                    {
                        DirectEvents.Click.EventMask.Target = MaskTarget.CustomTarget;
                        DirectEvents.Click.EventMask.CustomTarget = ConnectedGrid;
                    }
                }
                if (KnownCommand != Security.KnownCommand.None)
                {
                    switch (KnownCommand)
                    {
                        case Security.KnownCommand.CreateInEditor:
                            {
                                if (Icon == Ext.Net.Icon.None)
                                    Icon = Ext.Net.Icon.Add;
                                if (string.IsNullOrEmpty(Text))
                                    Text = "Create New";
                                break;
                            }
                        case Security.KnownCommand.DeleteEntity:
                            {
                                if (Icon == Ext.Net.Icon.None)
                                    Icon = Ext.Net.Icon.Delete;
                                if (string.IsNullOrEmpty(Text))
                                    Text = "Delete";
                                break;
                            }
                        case Security.KnownCommand.EditInEditor:
                            {
                                if (Icon == Ext.Net.Icon.None)
                                    Icon = Ext.Net.Icon.NoteEdit;
                                if (string.IsNullOrEmpty(Text))
                                    Text = "Edit";
                                break;
                            }
                    }
                }
            }

            base.OnInit(e);
        }

        void TTCmdButon_Click(object sender, EventArgs e)
        {
            this.TTMvcButon_DirectClick(sender, new DirectEventArgs(new ParameterCollection()));
        }

        protected override void OnLoad(EventArgs e)
        {
            if (Visible && !string.IsNullOrEmpty(ConnectedGrid) && !Ext.Net.X.IsAjaxRequest)
            {
                if (ForceGridSelection)
                {
                    string jsFunc = "getGridSelectedId(#{" + ConnectedGrid + "})";
                    DirectEvents.Click.ExtraParams.Add(new Parameter("id", jsFunc, ParameterMode.Raw));

                    if (KnownCommand != Security.KnownCommand.CreateInEditor)
                    {
                        Listeners.Click.Handler = "return " + jsFunc + " != null;";
                    }
                }

                if (KnownCommand == KnownCommand.DeleteEntity)
                {
                    DirectEvents.Click.Confirmation.ConfirmRequest = true;
                    DirectEvents.Click.Confirmation.Message = "Delete selected item ?";
                }

            }



            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        void TTMvcButon_DirectClick(object sender, Ext.Net.DirectEventArgs e)
        {
            CommandInfo cmd = new CommandInfo(this.CommandName, e);
            string argument = ((TTCmdButon)sender).CommandArgument;
            if (!string.IsNullOrEmpty(argument))
                cmd.Parameters.Add("CommandArgument", argument);
            cmd.Source = CommandSource;
            if (Command != null)
                Command(sender, cmd);
            else if (MvcControl != null)
                MvcControl.ProcessCommand(sender, cmd);

        }
    }
}
