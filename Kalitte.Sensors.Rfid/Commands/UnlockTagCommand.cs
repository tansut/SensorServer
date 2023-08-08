using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Rfid.Utilities;
using System.Runtime.Serialization;
using Kalitte.Sensors.Rfid.Core;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.UI;

namespace Kalitte.Sensors.Rfid.Commands
{
    [Serializable]
    [SensorCommandEditor("Rfid/UnLockTagCommandEditor.ascx")]
    public sealed class UnlockTagCommand : TagCommand
    {
        // Fields
        private readonly byte[] m_tagId;
        private UnlockTagResponse response;
        private readonly LockTargets targets;

        // Methods
        public UnlockTagCommand(byte[] passCode, byte[] tagId, LockTargets targets)
            : base(passCode)
        {
            this.m_tagId = tagId;
            this.targets = targets;
            this.ValidateParameters();
        }

        public byte[] GetTagId()
        {
            return CollectionsHelper.CloneByte(this.m_tagId);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<unlockTag>");
            builder.Append(base.ToString());
            builder.Append("<tagId>");
            builder.Append(this.m_tagId);
            builder.Append("</tagId>");
            builder.Append("<targets>");
            builder.Append(this.targets);
            builder.Append("</targets>");
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</unlockTag>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if ((this.m_tagId == null) || (this.m_tagId.Length == 0))
            {
                throw new ArgumentNullException("tagId");
            }
            if (LockTargets.None.Value >= this.targets.Value)
            {
                throw new ArgumentException("InvalidEnumValue");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        // Properties
        public UnlockTagResponse Response
        {
            get
            {
                return this.response;
            }
            set
            {
                this.response = value;
            }
        }

        public LockTargets Targets
        {
            get
            {
                return this.targets;
            }
        }
    }





}
