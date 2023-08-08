using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Commands;
using Kalitte.Sensors.UI;
using Kalitte.Sensors.Rfid.Commands;
using System.Text;
using Kalitte.Sensors.Rfid.Events;
using Kalitte.Sensors.Rfid.Core;


namespace Kalitte.Sensors.Rfid.Client.CommandEditors
{
    public partial class TagMemorySelect : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public bool LengthEnabled
        {
            get
            {
                return ctlLength.Visible;
            }
            set
            {
                ctlLength.Visible = value;
                ctlLengthLabel.Visible = value;
                ctlLengthValidator.Enabled = value;
            }
        }

        internal void SetPosition(C1G2MemoryBankPosition position)
        {
            switch (position.MemoryBank)
            {
                case C1G2MemoryBank.Epc:
                    {
                        ctlEpc.Checked = true;
                        break;
                    }
                case C1G2MemoryBank.Reserved:
                    {
                        ctlReserved.Checked = true;
                        break;
                    }
                case C1G2MemoryBank.User:
                    {
                        ctlUser.Checked = true;
                        break;
                    }
                case C1G2MemoryBank.Tid:
                    {
                        ctlTid.Checked = true;
                        break;
                    }
            }
            ctlStart.Text = position.Start.ToString();
            ctlLength.Text = position.Length.ToString();
        }

        internal C1G2MemoryBankPosition GetPosition()
        {
            int start = int.Parse(ctlStart.Text);
            int length = int.Parse(ctlLength.Text);
            C1G2MemoryBank bank = C1G2MemoryBank.Epc;
            if (ctlReserved.Checked)
                bank = C1G2MemoryBank.Reserved;
            else if (ctlEpc.Checked)
                bank = C1G2MemoryBank.Epc;
            else if (ctlTid.Checked)
                bank = C1G2MemoryBank.Tid;
            else if (ctlUser.Checked)
                bank = C1G2MemoryBank.User;
            return new C1G2MemoryBankPosition(bank, start, length);
        }

        public byte[] GetTagId()
        {
            return HexHelper.HexDecode(ctlTagId.Text);
        }

        public byte[] GetPasscode()
        {
            return RfidHelper.GetBytes(ctlPasscode.Text);
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            imgTable.Style.Add(HtmlTextWriterStyle.BackgroundImage, ResolveClientUrl("~/Resource/Image/Rfid/TagMemoryC1G2.gif"));

        }



    }
}