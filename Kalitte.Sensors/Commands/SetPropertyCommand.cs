using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Configuration;


namespace Kalitte.Sensors.Commands
{
    [Serializable, MayChangeState]
    public sealed class SetPropertyCommand : SensorCommand
    {
        // Fields
        private readonly EntityProperty property;
        private SetPropertyResponse response;

        // Methods
        public SetPropertyCommand(EntityProperty entityProperty)
        {
            this.property = entityProperty;
            this.ValidateParameters();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<setProperty>");
            builder.Append(base.ToString());
            builder.Append("<property>");
            builder.Append(this.property);
            builder.Append("</property>");
            builder.Append("<response>");
            builder.Append(this.response);
            builder.Append("</response>");
            builder.Append("</setProperty>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if (this.property == null)
            {
                throw new ArgumentNullException("entityProperty");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        // Properties
        public EntityProperty Property
        {
            get
            {
                return this.property;
            }
        }

        public SetPropertyResponse Response
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
    }




}
