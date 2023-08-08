namespace Kalitte.Sensors.Rfid.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.Text;
    using Kalitte.Sensors.Commands;

    [Serializable]
    public sealed class GetAllPrintTemplatesResponse : Response
    {
        private readonly Collection<byte[]> templates;

        public GetAllPrintTemplatesResponse(Collection<byte[]> templates)
        {
            this.templates = templates;
            this.ValidateParameters();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<getAllPrintTemplatesResponse>");
            builder.Append("</getAllPrintTemplatesResponse>");
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            if (this.templates != null)
            {
                int num = 0;
                using (IEnumerator<byte[]> enumerator = this.templates.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Current == null)
                        {
                            throw new ArgumentNullException("templates[" + num + "]");
                        }
                        num++;
                    }
                }
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        public ReadOnlyCollection<byte[]> Templates
        {
            get
            {
                if (this.templates != null)
                {
                    return new ReadOnlyCollection<byte[]>(this.templates);
                }
                return null;
            }
        }
    }
}
