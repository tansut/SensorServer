using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Configuration;

namespace Kalitte.Sensors.Processing
{
    [Serializable]
    public sealed class DispatcherModuleInformation
    {
        public DispatcherEntity InstanceEntity { get; private set; }

        internal DispatcherModuleInformation(DispatcherEntity entity)
        {
            this.InstanceEntity = entity;
        }

        public PropertyList PropertyProfile
        {
            get
            {
                return InstanceEntity.Properties.Profile;
            }
        }

        public PropertyList ExtendedProfile
        {
            get
            {
                return InstanceEntity.Properties.Profile;
            }
        }
    }
}
