using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Reflection;
using System.Security.Permissions;
using System.Collections.ObjectModel;

namespace Kalitte.Sensors.Exceptions
{
    [Serializable]
    public class SensorException : Exception
    {
        // Fields
        private string errorCode;
        private object[] parameters;
        private string Sensor_ERROR_CODE;
        private const string Sensor_ERROR_PARAMETERS = "SENSOR_ERROR_PARAMS";

        // Methods
        public SensorException()
        {
            this.Sensor_ERROR_CODE = "SENSOR_ERROR_CODE";
        }

        public SensorException(string message)
            : base(message)
        {
            this.Sensor_ERROR_CODE = "SENSOR_ERROR_CODE";
        }

        protected SensorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.Sensor_ERROR_CODE = "SENSOR_ERROR_CODE";
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            this.errorCode = info.GetString(this.Sensor_ERROR_CODE);
            object obj2 = info.GetValue("SENSOR_ERROR_PARAMS", typeof(object));
            this.parameters = GetObjectArray(obj2);
        }

        public SensorException(string message, Exception innerException)
            : base(message, innerException)
        {
            this.Sensor_ERROR_CODE = "SENSOR_ERROR_CODE";
        }

        public SensorException(string message, string errorCode, params object[] exceptionParameters)
            : this(message, null, errorCode, exceptionParameters)
        {
        }

        public SensorException(string message, Exception innerException, string errorCode, params object[] exceptionParameters)
            : base(message, innerException)
        {
            this.Sensor_ERROR_CODE = "SENSOR_ERROR_CODE";
            this.errorCode = errorCode;
            this.parameters = exceptionParameters;
            this.ValidateParameters();
        }

        internal void ClearInnerException()
        {
            Type type = typeof(Exception);
            type.GetField("_innerException", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this, null);
        }

        internal void ClearStackTraces()
        {
            Type type = typeof(Exception);
            type.GetField("_stackTrace", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this, null);
            type.GetField("_remoteStackTraceString", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this, null);
        }

        private static Collection<object> GetCollectionFromObjectArray(object[] objs)
        {
            if (objs == null)
            {
                return null;
            }
            Collection<object> collection = new Collection<object>();
            foreach (object obj2 in objs)
            {
                collection.Add(obj2);
            }
            return collection;
        }

        private static object[] GetObjectArray(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            object[] array = obj as object[];
            if (array == null)
            {
                Collection<object> collection = obj as Collection<object>;
                if (collection == null)
                {
                    throw new InvalidOperationException();
                }
                array = new object[collection.Count];
                collection.CopyTo(array, 0);
            }
            return array;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            base.GetObjectData(info, context);
            info.AddValue(this.Sensor_ERROR_CODE, this.errorCode);
            info.AddValue("SENSOR_ERROR_PARAMS", GetCollectionFromObjectArray(this.parameters));
        }

        internal static bool IsValidParameter(object parameter)
        {
            if (!parameter.GetType().IsPrimitive)
            {
                return (parameter.GetType() == typeof(string));
            }
            return true;
        }

        internal void SetMessage(string newMessage)
        {
            Type type = typeof(Exception);
            type.GetField("_message", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this, newMessage);
        }

        private void ValidateParameters()
        {
            if (this.parameters != null)
            {
                foreach (object obj2 in this.parameters)
                {
                    if ((obj2 != null) && !IsValidParameter(obj2))
                    {
                        throw new ArgumentException("InvalidExceptionParameterType", "parameters");
                    }
                }
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        // Properties
        public string ErrorCode
        {
            get
            {
                return this.errorCode;
            }
            set
            {
                this.errorCode = value;
            }
        }

        [XmlIgnore]
        public object[] Parameters
        {
            get
            {
                return this.parameters;
            }
        }

        public object[] ParametersForXml
        {
            get
            {
                return this.parameters;
            }
            set
            {
                this.parameters = value;
            }
        }
    }




}
