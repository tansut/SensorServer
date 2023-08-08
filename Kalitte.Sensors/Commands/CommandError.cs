using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Exceptions;
using Kalitte.Sensors.Utilities;


namespace Kalitte.Sensors.Commands
{
    [Serializable]
    public class CommandError
    {
        // Fields
        private readonly ErrorCode errorCode;
        private readonly string errorKey;
        private readonly string message;
        private readonly object[] parameters;
        private SensorProviderException providerException;

        // Methods
        public CommandError(ErrorCode errorCode, string message, string errorKey, params object[] parameters)
        {
            this.errorCode = errorCode;
            this.message = message;
            this.errorKey = errorKey;
            this.parameters = parameters;
            this.ValidateParameters();
        }

        public CommandError(ErrorCode errorCode, SensorProviderException providerException, string message, string errorKey, params object[] parameters)
            : this(errorCode, message, errorKey, parameters)
        {
            this.providerException = providerException;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<commandError>");
            builder.Append("<errorCode>");
            builder.Append(this.errorCode);
            builder.Append("</errorCode>");
            builder.Append("<message>");
            builder.Append(this.message);
            builder.Append("</message>");
            builder.Append("<errorKey>");
            builder.Append(this.errorKey);
            builder.Append("</errorKey>");
            builder.Append("<parameters>");
            if (this.parameters != null)
            {
                foreach (object obj2 in this.parameters)
                {
                    builder.Append("<parameter>");
                    builder.Append(obj2);
                    builder.Append("</parameter>");
                }
            }
            builder.Append("</parameters>");
            builder.Append("<providerException>");
            builder.Append(this.providerException);
            builder.Append("</providerException>");
            builder.Append("</commandError>");
            return builder.ToString();
        }

        private static bool validateParameter(object parameter)
        {
            if (parameter != null)
            {
                Type type = parameter.GetType();
                if (type.IsPrimitive || type.Equals(typeof(string)))
                {
                    return true;
                }
                if (!type.IsArray)
                {
                    return false;
                }
                object[] objArray = (object[])parameter;
                foreach (object obj2 in objArray)
                {
                    if (!validateParameter(obj2))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void ValidateParameters()
        {
            if (ErrorCode.Uninitialized.Value >= this.errorCode.Value)
            {
                throw new ArgumentException("InvalidEnumValue");
            }
            if (this.parameters != null)
            {
                foreach (object obj2 in this.parameters)
                {
                    validateParameter(obj2);
                }
            }
            if ((this.errorKey == null) || (this.errorKey.Length == 0))
            {
                throw new ArgumentNullException("errorKey");
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        // Properties
        public ErrorCode ErrorCode
        {
            get
            {
                return this.errorCode;
            }
        }

        public string ErrorKey
        {
            get
            {
                return this.errorKey;
            }
        }

        public string Message
        {
            get
            {
                return this.message;
            }
        }

        public Collection<object> Parameters
        {
            get
            {
                return CollectionsHelper.CreateParamsCollectionFromArray(this.parameters);
            }
        }

        public SensorProviderException ProviderException
        {
            get
            {
                return this.providerException;
            }
        }
    }




}
