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
    [SensorCommandEditor("Rfid/LockTagCommandEditor.ascx")]
    public sealed class LockTagCommand : TagCommand
    {
        // Fields
        private readonly byte[] m_tagId;
        private readonly bool permanent;
        private LockTagResponse response;
        private readonly LockTargets targets;

        // Methods
        public LockTagCommand(byte[] passCode, byte[] tagId, LockTargets targets)
            : base(passCode)
        {
            this.m_tagId = tagId;
            this.targets = targets;
            this.ValidateParameters();
        }

        public LockTagCommand(byte[] passCode, byte[] tagId, LockTargets targets, bool permanent)
            : this(passCode, tagId, targets)
        {
            this.permanent = permanent;
        }

        public byte[] GetTagId()
        {
            return CollectionsHelper.CloneByte(this.m_tagId);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<lockTag>");
            builder.Append(base.ToString());
            builder.Append("<tagId>");
            builder.Append(this.m_tagId);
            builder.Append("</tagId>");
            builder.Append("<targets>");
            builder.Append(this.targets);
            builder.Append("</targets>");
            builder.Append("<permanent>");
            builder.Append(this.permanent);
            builder.Append("</permanent>");
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</lockTag>");
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
        public bool IsPermanent
        {
            get
            {
                return this.permanent;
            }
        }

        public LockTagResponse Response
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
