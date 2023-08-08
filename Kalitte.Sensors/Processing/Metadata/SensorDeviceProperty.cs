using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Communication;
using Kalitte.Sensors.Security;
using Kalitte.Sensors.Configuration;
using System.Runtime.Serialization;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Processing.Metadata
{
    [Serializable, DataContract]
    public class SensorDeviceProperty: EntityPropertyBase
    {
        [DataMember] 
        public ConnectionInformation Connection { get; set; }

        private Dictionary<string, PropertyList> sourceProfiles;

        [DataMember(IsRequired=false)] 
        public Dictionary<string, PropertyList> SourceProfiles
        {
            get
            {
                if (sourceProfiles == null)
                    sourceProfiles = new Dictionary<string, PropertyList>();
                return sourceProfiles;
            }
            internal set
            {
                sourceProfiles = value;
            }
            
        }

        [DataMember] 
        public AuthenticationInformation Authentication { get; set; }

        public SensorDeviceProperty(ConnectionInformation connection, 
            AuthenticationInformation authentication,
            PropertyList propertyProfile, PropertyList extendedProfile, Dictionary<string, PropertyList> sourceProfiles, ItemStartupType startup): base(propertyProfile, extendedProfile, startup)
        {
            this.Connection = connection;
            this.Authentication = authentication;
            this.sourceProfiles = sourceProfiles;
        }

        public PropertyList GetSettableProfile(PropertyList sourceProfile, Dictionary<PropertyKey, EntityMetadata> metaData)
        {
            var settableProps = new PropertyList(sourceProfile.Name);
            foreach (var mdata in metaData)
                if ((!mdata.Value.IsPersistent && mdata.Value.IsWritable) && sourceProfile.ContainsKey(mdata.Key))
                    settableProps.Add(mdata.Key, sourceProfile[mdata.Key]);
            return settableProps;
        }

        private static IEnumerable<Type> GetKnownTypes()
        {
            return TypesHelper.GetKnownTypeEnumerator();
        }
    }
}
