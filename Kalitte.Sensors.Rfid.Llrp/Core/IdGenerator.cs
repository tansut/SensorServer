namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;

    internal class IdGenerator
    {
        private static uint m_accessSpecIdGenerator = 0x7fffffff;
        private static uint m_accessSpecIdGeneratorForProvider = 0;
        private static ushort m_inventorySpecIdGenerator = 0;
        private static uint m_llrpMessageIdGenerator = 0;
        private static ushort m_OPSpecIdGenerator = 0;
        private static uint m_roSpecIdGenerator = 0x7fffffff;
        private static uint m_roSpecIdGeneratorForProvider = 0;

        private static ushort Generate16BitId(ushort id)
        {
            if (id == 0xffff)
            {
                id = 0;
            }
            id = (ushort) (id + 1);
            return id;
        }

        private static uint Generate32BitId(uint id)
        {
            return Generate32BitId(id, 0, uint.MaxValue);
        }

        private static uint Generate32BitId(uint id, uint minValue, uint maxValue)
        {
            if (id == maxValue)
            {
                id = minValue;
            }
            id++;
            return id;
        }

        internal static uint GenerateAccessSpecId()
        {
            m_accessSpecIdGenerator = Generate32BitId(m_accessSpecIdGenerator, 0x7fffffff, uint.MaxValue);
            return m_accessSpecIdGenerator;
        }

        internal static uint GenerateAccessSpecIdForProvider()
        {
            m_accessSpecIdGeneratorForProvider = Generate32BitId(m_accessSpecIdGeneratorForProvider, 0, 0x7fffffff);
            return m_accessSpecIdGeneratorForProvider;
        }

        internal static ushort GenerateInventorySpecId()
        {
            m_inventorySpecIdGenerator = Generate16BitId(m_inventorySpecIdGenerator);
            return m_inventorySpecIdGenerator;
        }

        internal static uint GenerateLlrpMessageId()
        {
            m_llrpMessageIdGenerator = Generate32BitId(m_llrpMessageIdGenerator);
            return m_llrpMessageIdGenerator;
        }

        internal static ushort GenerateOPSpecId()
        {
            m_OPSpecIdGenerator = Generate16BitId(m_OPSpecIdGenerator);
            return m_OPSpecIdGenerator;
        }

        internal static uint GenerateROSpecId()
        {
            m_roSpecIdGenerator = Generate32BitId(m_roSpecIdGenerator, 0x7fffffff, uint.MaxValue);
            return m_roSpecIdGenerator;
        }

        internal static uint GenerateROSpecIdForProvider()
        {
            m_roSpecIdGeneratorForProvider = Generate32BitId(m_roSpecIdGeneratorForProvider, 0, 0x7fffffff);
            return m_roSpecIdGeneratorForProvider;
        }
    }
}
