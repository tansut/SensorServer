namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    [StructLayout(LayoutKind.Sequential)]
    internal struct LLRPElement
    {
        private byte[] m_element;
        private uint m_bitsOfInterest;
        private bool m_fbigEndian;
        internal byte[] Element
        {
            get
            {
                return this.m_element;
            }
        }
        internal uint BitsInElement
        {
            get
            {
                return this.m_bitsOfInterest;
            }
        }
        internal bool IsBigEndian
        {
            get
            {
                return this.m_fbigEndian;
            }
        }
        internal LLRPElement(byte[] element, uint bitsInElement, bool bigEndian)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (element.Length == 0)
            {
                throw new ArgumentOutOfRangeException("element");
            }
            if ((bitsInElement < 1) || (bitsInElement > (element.Length * 8)))
            {
                throw new ArgumentOutOfRangeException("bitsInElement");
            }
            this.m_element = element;
            this.m_bitsOfInterest = bitsInElement;
            this.m_fbigEndian = bigEndian;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<LLRPElement>");
            builder.Append("<Element>");
            if (this.Element != null)
            {
                foreach (byte num in this.Element)
                {
                    builder.Append("<Byte>");
                    builder.Append(num);
                    builder.Append("</Byte>");
                }
            }
            builder.Append("</Element>");
            builder.Append("<NumberOfBits>");
            builder.Append(this.BitsInElement);
            builder.Append("</NumberOfBits>");
            builder.Append("<BigEndian>");
            builder.Append(this.IsBigEndian);
            builder.Append("</BigEndian>");
            builder.Append("</LLRPElement>");
            return builder.ToString();
        }
    }
}
