using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Utilities;


namespace Kalitte.Sensors.Commands
{
    [Serializable, KnownType("GetKnownTypes")]
    public abstract class SensorCommand
    {
        private readonly string id = Guid.NewGuid().ToString("D");
        private VendorData vendorDefinedParameters;

        protected SensorCommand()
        {
        }

        private static IEnumerable<Type> GetKnownTypes()
        {
            return TypesHelper.GetKnownTypeEnumerator();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<id>");
            builder.Append(this.id);
            builder.Append("</id>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if (string.IsNullOrEmpty(this.id))
            {
                throw new ArgumentNullException("id");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        // Properties
        public string Id
        {
            get
            {
                return this.id;
            }
        }

        public VendorData VendorDefinedParameters
        {
            get
            {
                return this.vendorDefinedParameters;
            }
            set
            {
                this.vendorDefinedParameters = value;
            }
        }
    }





}
