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
    public abstract class Response
    {
        // Fields
        private VendorData vendorReplies;

        // Methods
        protected Response()
        {
        }

        private static IEnumerable<Type> GetKnownTypes()
        {
            return TypesHelper.GetKnownTypeEnumerator();
        }

        // Properties
        public VendorData VendorReplies
        {
            get
            {
                return this.vendorReplies;
            }
            set
            {
                this.vendorReplies = value;
            }
        }
    }





}
