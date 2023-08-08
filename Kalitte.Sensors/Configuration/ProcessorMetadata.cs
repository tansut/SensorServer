using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Kalitte.Sensors.Configuration
{
    [Serializable]
    public class ProcessorMetadata
    {
        private Dictionary<PropertyKey, ProcessorPropertyMetadata> processorPropertyMetadata;

        public ProcessorMetadata(Dictionary<PropertyKey, ProcessorPropertyMetadata> processorPropertyMetadata)
        {
            this.processorPropertyMetadata = processorPropertyMetadata;
        }

        public Dictionary<PropertyKey, ProcessorPropertyMetadata> ProcessorPropertyMetadata
        {
            get
            {
                return this.processorPropertyMetadata;
            }
        }
    }
}
