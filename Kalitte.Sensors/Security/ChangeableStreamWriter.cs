using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Kalitte.Sensors.Security
{
    public sealed class ChangeableStreamWriter
    {
        // Fields
        private StreamWriter m_streamWriter;

        // Methods
        public ChangeableStreamWriter(StreamWriter streamWriter)
        {
            this.StreamWriter = streamWriter;
        }

        // Properties
        public StreamWriter StreamWriter
        {
            get
            {
                return this.m_streamWriter;
            }
            set
            {
                this.m_streamWriter = value;
            }
        }
    }

 

}
