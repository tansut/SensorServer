namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;
    using System.Collections.ObjectModel;
    using System.Text;
    using Kalitte.Sensors.Rfid.Llrp.Helpers;

    internal class LLRPMessageStream
    {
        private Collection<LLRPElement> m_elements = new Collection<LLRPElement>();
        private uint m_totalBitsYet;

        internal LLRPMessageStream()
        {
        }

        internal void Append(byte[] element, uint bitsOfInterest, bool isBigEndian)
        {
            this.m_elements.Add(new LLRPElement(element, bitsOfInterest, isBigEndian));
            this.m_totalBitsYet += bitsOfInterest;
        }

        internal void Append(bool element, uint bitsOfInterest, bool isBigEndian)
        {
            this.Append(BitConverter.GetBytes(element), bitsOfInterest, isBigEndian);
        }

        internal void Append(long element, uint bitsOfInterest, bool isBigEndian)
        {
            if (!BitConverter.IsLittleEndian)
            {
                isBigEndian = !isBigEndian;
            }
            this.Append(BitConverter.GetBytes(element), bitsOfInterest, isBigEndian);
        }

        internal void Append(ulong element, uint bitsOfInterest, bool isBigEndian)
        {
            if (!BitConverter.IsLittleEndian)
            {
                isBigEndian = !isBigEndian;
            }
            this.Append(BitConverter.GetBytes(element), bitsOfInterest, isBigEndian);
        }

        internal byte[] Merge()
        {
            uint num = this.m_totalBitsYet / 8;
            byte[] destination = new byte[num];
            uint bitsRemainingInByte = 8;
            uint byteIndex = 0;
            foreach (LLRPElement element in this.m_elements)
            {
                uint num5;
                int num6;
                uint bitsInElement = element.BitsInElement;
                if (element.IsBigEndian)
                {
                    num6 = (int) ((element.BitsInElement - 1) / 8);
                    while ((bitsInElement > 0) && (num6 >= 0))
                    {
                        num5 = ((bitsInElement % 8) == 0) ? 8 : (bitsInElement % 8);
                        BitHelper.CopyByteToArray(element.Element[num6], num5, destination, ref bitsRemainingInByte, ref byteIndex);
                        bitsInElement -= num5;
                        num6--;
                    }
                    continue;
                }
                for (num6 = 0; (bitsInElement > 0) && (num6 < element.Element.Length); num6++)
                {
                    num5 = (bitsInElement > 8) ? 8 : bitsInElement;
                    BitHelper.CopyByteToArray(element.Element[num6], num5, destination, ref bitsRemainingInByte, ref byteIndex);
                    bitsInElement -= num5;
                }
            }
            return destination;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<LlrpMessageStream>");
            foreach (LLRPElement element in this.m_elements)
            {
                builder.Append(element);
            }
            builder.Append("</LlrpMessageStream>");
            return builder.ToString();
        }
    }
}
