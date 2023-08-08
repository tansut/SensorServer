using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Runtime.Serialization;

namespace Kalitte.Sensors.Exceptions
{
    [Serializable, StrongNameIdentityPermission(SecurityAction.InheritanceDemand)]
    public class ProcessorException : RemoteObjectException
    {
        string relatedModule;

        public string RelatedModule
        {
            get
            {
                return this.relatedModule;
            }
            set
            {
                this.relatedModule = value;
            }
        }

        public ProcessorException()
        {
           
        }



        public ProcessorException(string message)
            : base(message)
        {
        }

        public ProcessorException(string message, string relatedModule)
            : this(message)
        {
            this.RelatedModule = relatedModule;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("relatedModule", relatedModule);
        }

        protected ProcessorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.relatedModule = info.GetString("relatedModule");
        }

        public ProcessorException(string message, Exception innerException)
            : base(message, innerException, null, null)
        {
        }

        public ProcessorException(string message, string errorCode, params object[] parameters)
            : base(message, errorCode, parameters)
        {
        }

        public ProcessorException(string message, Exception innerException, string errorCode, string relatedModule, params object[] parameters)
            : base(message, innerException, errorCode, parameters)
        {
            this.RelatedModule = relatedModule;
        }

    }




}
