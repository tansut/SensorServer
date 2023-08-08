using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.UI;
using System.Runtime.Serialization;

namespace Kalitte.Sensors.Rfid.EventModules.Client.TagView
{
    [Serializable]
    [PropertyEditor("EventModuleMetadata/TagStatusModule/TagStatusCustomDataEditor.ascx")]
    public class TagStatusCustomData
    {
        [DataMember]
        public string Prop1 { get; set; }

    }
}
