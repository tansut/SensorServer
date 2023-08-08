namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Kalitte.Sensors.Rfid.Utilities;
    using Kalitte.Sensors.Rfid.Core;
    using Kalitte.Sensors.Utilities;

    [Serializable]
    public sealed class PrintLabel
    {
        private readonly LockTargets lockTargets;
        private readonly byte[] newAccessCode;
        private readonly byte[] newKillCode;
        private readonly byte[] tagData;
        private readonly byte[] tagId;
        private Dictionary<string, string> textFieldsAndBarcodes;

        public PrintLabel(byte[] tagId, byte[] tagData, byte[] newAccessCode, byte[] newKillCode, Dictionary<string, string> textFieldsAndBarcodes, LockTargets lockTargets)
        {
            this.tagId = tagId;
            this.tagData = tagData;
            this.newAccessCode = newAccessCode;
            this.newKillCode = newKillCode;
            this.textFieldsAndBarcodes = textFieldsAndBarcodes;
            this.lockTargets = lockTargets;
        }

        public byte[] GetNewAccessCode()
        {
            return CollectionsHelper.CloneByte(this.newAccessCode);
        }

        public byte[] GetNewKillCode()
        {
            return CollectionsHelper.CloneByte(this.newKillCode);
        }

        public byte[] GetTagData()
        {
            return CollectionsHelper.CloneByte(this.tagData);
        }

        public byte[] GetTagId()
        {
            return CollectionsHelper.CloneByte(this.tagId);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<printLabel>");
            if (this.tagId != null)
            {
                builder.Append("<tagId>");
                //builder.Append(Convert.ToBase64String(this.tagId));
                builder.Append(HexHelper.HexEncode(this.tagId));
                builder.Append("</tagId>");
            }
            if (this.tagData != null)
            {
                builder.Append("<tagData>");
                //builder.Append(Convert.ToBase64String(this.tagData));
                builder.Append(HexHelper.HexEncode(this.tagData));
                builder.Append("</tagData>");
            }
            if (this.newAccessCode != null)
            {
                builder.Append("<newAccessCode>");
                //builder.Append(Convert.ToBase64String(this.newAccessCode));
                builder.Append(HexHelper.HexEncode(this.newAccessCode));
                builder.Append("</newAccessCode>");
            }
            if (this.newKillCode != null)
            {
                builder.Append("<newKillCode>");
                //builder.Append(Convert.ToBase64String(this.newKillCode));
                builder.Append(HexHelper.HexEncode(this.newKillCode));
                builder.Append("</newKillCode>");
            }
            builder.Append("<lockTargets>");
            builder.Append(this.lockTargets);
            builder.Append("</lockTargets>");
            if (this.textFieldsAndBarcodes != null)
            {
                builder.Append("<textFieldsAndBarcodes>");
                foreach (string str in this.textFieldsAndBarcodes.Keys)
                {
                    builder.Append("<nameValuePair>");
                    builder.Append("<fieldName>");
                    builder.Append(str.ToString());
                    builder.Append("</fieldName>");
                    builder.Append("<value>");
                    builder.Append(this.textFieldsAndBarcodes[str]);
                    builder.Append("</value>");
                    builder.Append("</nameValuePair>");
                }
                builder.Append("</textFieldsAndBarcodes>");
            }
            builder.Append("</printLabel>");
            return builder.ToString();
        }

        public LockTargets LockTargets
        {
            get
            {
                return this.lockTargets;
            }
        }

        public Dictionary<string, string> TextFieldsAndBarcodes
        {
            get
            {
                if (this.textFieldsAndBarcodes == null)
                {
                    this.textFieldsAndBarcodes = new Dictionary<string, string>();
                }
                return this.textFieldsAndBarcodes;
            }
        }
    }
}
