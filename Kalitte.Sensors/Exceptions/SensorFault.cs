using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Exceptions
{
    [Serializable, KnownType("GetKnownTypes")]
    public class SensorFault
    {
        // Fields
        private string remoteErrorCode;
        private string remoteErrorMessage;
        private SensorException remoteException;
        private object[] remoteParams;
        private string remoteStackTrace;

        // Methods
        private static IEnumerable<Type> GetKnownTypes()
        {
            return TypesHelper.GetKnownTypeEnumerator();
        }

        public object[] GetRemoteParameters()
        {
            return this.remoteParams;
        }

        public void SetRemoteParameters(object[] parameters)
        {
            this.remoteParams = parameters;
        }

        public override string ToString()
        {
            return SerializationHelper.SerializeToXmlDataContract(this, base.GetType(), true, true);
        }

        // Properties
        public string RemoteErrorCode
        {
            get
            {
                return this.remoteErrorCode;
            }
            internal set
            {
                this.remoteErrorCode = value;
            }
        }

        public string RemoteErrorMessage
        {
            get
            {
                return this.remoteErrorMessage;
            }
            internal set
            {
                this.remoteErrorMessage = value;
            }
        }

        public SensorException RemoteException
        {
            get
            {
                return this.remoteException;
            }
            internal set
            {
                this.remoteException = value;
                if (this.remoteException != null)
                {
                    this.remoteException.ClearStackTraces();
                    this.remoteException.ClearInnerException();
                }
            }
        }

        public string RemoteStackTrace
        {
            get
            {
                return this.remoteStackTrace;
            }
            internal set
            {
                this.remoteStackTrace = value;
            }
        }
    }



}
