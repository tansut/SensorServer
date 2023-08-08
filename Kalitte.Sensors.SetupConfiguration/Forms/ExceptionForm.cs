using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kalitte.Sensors.SetupConfiguration.Core;
using Kalitte.Sensors.SetupConfiguration.Core.Exception;

namespace Kalitte.Sensors.SetupConfiguration.Forms
{
    public partial class ExceptionForm : Form
    {
        public ExceptionForm()
        {
            InitializeComponent();
        }

        public void Execute(Exception e)
        {
            InitUI(e);
            ShowDialog();
        }

        private void InitUI(Exception e)
        {
            Clear();
            this.Text = e.GetType().Name;
            ctlMessage.Text = e.Message;
            if (e is UserException)
                ctlStackTrace.Text = ((UserException)e).Detail;
            else
                ctlStackTrace.Text = e.StackTrace;
        }

        private void Clear()
        {
            ctlMessage.Clear();
            ctlStackTrace.Clear();
        }
    }
}
