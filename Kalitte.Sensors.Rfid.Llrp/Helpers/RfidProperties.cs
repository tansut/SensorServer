namespace Kalitte.Sensors.Rfid.Llrp.Helpers
{
    using System;
    using System.Collections.Generic;
    

    [Serializable]
    public class SensorProperties
    {
        private Dictionary<int, PropertyProfile> m_antennaProfiles;
        private PropertyProfile m_deviceProfile;
        private Dictionary<int, PropertyProfile> m_gpiProfiles;
        private Dictionary<int, PropertyProfile> m_gpoProfiles;

        public SensorProperties(PropertyProfile deviceProfile, Dictionary<int, PropertyProfile> antennaProfiles, Dictionary<int, PropertyProfile> gpiProfiles, Dictionary<int, PropertyProfile> gpoProfiles)
        {
            if (((deviceProfile == null) && (antennaProfiles == null)) && ((gpiProfiles == null) && (gpoProfiles == null)))
            {
                throw new ArgumentNullException();
            }
            this.m_deviceProfile = deviceProfile;
            this.m_antennaProfiles = antennaProfiles;
            this.m_gpiProfiles = gpiProfiles;
            this.m_gpoProfiles = gpoProfiles;
        }

        public Dictionary<int, PropertyProfile> AntennaProfiles
        {
            get
            {
                return this.m_antennaProfiles;
            }
        }

        public PropertyProfile DeviceProfile
        {
            get
            {
                return this.m_deviceProfile;
            }
        }

        public Dictionary<int, PropertyProfile> GpiProfiles
        {
            get
            {
                return this.m_gpiProfiles;
            }
        }

        public Dictionary<int, PropertyProfile> GpoProfiles
        {
            get
            {
                return this.m_gpoProfiles;
            }
        }
    }
}
