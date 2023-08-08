namespace Kalitte.Sensors.Rfid.Llrp.Communication
{
    using System;
    using System.Collections.Generic;

    internal class BufferPool
    {
        private List<bool> m_bufferInUse = new List<bool>();
        private byte[][] m_buffers;
        private byte[] m_byte8Kb = new byte[0x2000];
        private byte[] m_byteHeader = new byte[10];
        private object m_lock = new object();

        internal BufferPool()
        {
            this.m_buffers = new byte[][] { this.m_byteHeader, this.m_byte8Kb };
            for (int i = 0; i < this.m_buffers.Length; i++)
            {
                this.m_bufferInUse.Add(false);
            }
        }

        internal byte[] GetBuffer(int size)
        {
            lock (this.m_lock)
            {
                for (int i = 0; i < this.m_buffers.Length; i++)
                {
                    if ((this.m_buffers[i].Length >= size) && !this.m_bufferInUse[i])
                    {
                        this.m_bufferInUse[i] = true;
                        return this.m_buffers[i];
                    }
                }
                return new byte[size];
            }
        }

        internal void ReturnBuffer(byte[] buffer)
        {
            if (buffer != null)
            {
                lock (this.m_lock)
                {
                    for (int i = 0; i < this.m_buffers.Length; i++)
                    {
                        if (((this.m_buffers[i].Length == buffer.Length) && this.m_bufferInUse[i]) && this.m_buffers[i].Equals(buffer))
                        {
                            this.m_bufferInUse[i] = false;
                            break;
                        }
                    }
                }
            }
        }
    }
}
