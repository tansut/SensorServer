using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable]
    [KnownType("GetSubTypes"), DataContract]
    public abstract class PersistEntityBase: IDisposable
    {
        [DataMember]
        public virtual string Name { get; private set; }

        [DataMember]
        public string Description { get; set; }

        protected PersistEntityBase(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        protected PersistEntityBase(string name): this(name, string.Empty)
        {
        }

        private static IEnumerable<Type> GetSubTypes()
        {
            return TypesHelper.GetTypes(typeof(PersistEntityBase));
        }

        #region IDisposable Members

        public virtual void Dispose()
        {
            
        }

        #endregion
    }
}
