using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Collections.ObjectModel;

namespace Kalitte.Sensors.Exceptions
{
    [Serializable, KnownType(typeof(Collection<SensorException>))]
    public class MultipleInnerException : SensorException
    {
        // Fields
        private Collection<SensorException> m_detailedErrors;
        private const string MULTIPLE_DETAILED_ERRORS = "MULTIPLE_DETAILED_ERRORS";

        // Methods
        public MultipleInnerException(string message)
            : base(message)
        {
            this.m_detailedErrors = new Collection<SensorException>();
        }

        protected MultipleInnerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.m_detailedErrors = new Collection<SensorException>();
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            this.m_detailedErrors = (Collection<SensorException>)info.GetValue("MULTIPLE_DETAILED_ERRORS", typeof(Collection<SensorException>));
        }


        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            base.GetObjectData(info, context);
            info.AddValue("MULTIPLE_DETAILED_ERRORS", this.m_detailedErrors);
        }


        // Properties
        public Collection<SensorException> DetailedErrors
        {
            get
            {
                return this.m_detailedErrors;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append(Environment.NewLine);
            builder.Append("InnerErrors");
            builder.Append(Environment.NewLine);
            foreach (object obj2 in this.m_detailedErrors)
            {
                builder.Append("Exception").Append(Environment.NewLine).Append((obj2 == null) ? "" : obj2.ToString()).Append(Environment.NewLine);
            }
            return builder.ToString();
        }

    }





}
