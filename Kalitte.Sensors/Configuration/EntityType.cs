using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Kalitte.Sensors.Communication;
using Kalitte.Sensors.Commands;
using Kalitte.Sensors.Events.Management;

namespace Kalitte.Sensors.Configuration
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct EntityType : IEquatable<EntityType>
    {
        private const int UninitializedValue = 0;
        private const int CommandValue = 1;
        private const int ResponseValue = 2;
        private const int ObservationValue = 3;
        private const int ManagementEventValue = 4;
        private const int TransportSettingsValue = 5;
        private const int PrintTemplateFieldValue = 6;
        private const int LastValue = 6;
        public static readonly EntityType Uninitialized;
        public static readonly EntityType Command;
        public static readonly EntityType Response;
        public static readonly EntityType Observation;
        public static readonly EntityType ManagementEvent;
        public static readonly EntityType TransportSettings;
        public static readonly EntityType PrintTemplateField;
        private static Dictionary<int, string> standardDescriptions;
        internal static Dictionary<int, Type> baseType;
        internal static Dictionary<int, Type> extensionType;
        private readonly int enumValue;
        private readonly string description;
        public int Value
        {
            get
            {
                return this.enumValue;
            }
        }
        public string Description
        {
            get
            {
                return this.description;
            }
        }
        private EntityType(int value)
        {
            if (standardDescriptions == null)
            {
                Init();
            }
            if (0 > value)
            {
                throw new ArgumentException("InvalidEnumValue");
            }
            this.enumValue = value;
            this.description = standardDescriptions[value];
        }

        public EntityType(int value, string description)
        {
            if (0 >= value)
            {
                throw new ArgumentException("InvalidEnumValue");
            }
            if (value <= 6)
            {
                throw new InvalidOperationException("UseStandardCons");
            }
            this.enumValue = value;
            this.description = description;
        }

        private static void Init()
        {
            standardDescriptions = new Dictionary<int, string>();
            standardDescriptions[1] = "Command";
            standardDescriptions[2] = "Response";
            standardDescriptions[3] = "Observation";
            standardDescriptions[4] = "MgmtEvent";
            standardDescriptions[5] = "TransportSettings";
            standardDescriptions[6] = "PrintTemplateField";
            standardDescriptions[0] = "Uninitialized";
            baseType = new Dictionary<int, Type>();
            baseType[1] = typeof(SensorCommand);
            baseType[2] = typeof(Response);
            //baseType[3] = typeof(Observation);
            baseType[4] = typeof(ManagementEvent);
            baseType[5] = typeof(TransportSettings);
            //baseType[6] = typeof(PrintTemplateField);
            extensionType = new Dictionary<int, Type>();
            extensionType[1] = typeof(VendorCommand);
            extensionType[2] = typeof(VendorResponse);
            //extensionType[3] = typeof(VendorDefinedEvent);
            extensionType[4] = typeof(VendorDefinedManagementEvent);
            extensionType[5] = typeof(VendorTransportSettings);
            //extensionType[6] = typeof(VendorDefinedField);
        }

        public static explicit operator EntityType(int value)
        {
            if (0 > value)
            {
                throw new ArgumentException("InvalidEnumValue");
            }
            switch (value)
            {
                case 0:
                    return Uninitialized;

                case 1:
                    return Command;

                case 2:
                    return Response;

                case 3:
                    return Observation;

                case 4:
                    return ManagementEvent;

                case 5:
                    return TransportSettings;

                case 6:
                    return PrintTemplateField;
            }
            throw new ArgumentException("NonstandardValue");
        }

        public bool Equals(EntityType other)
        {
            return (this == other);
        }

        public override bool Equals(object obj)
        {
            return ((obj is EntityType) && (this == ((EntityType)obj)));
        }

        public override int GetHashCode()
        {
            return this.enumValue;
        }

        public static bool operator ==(EntityType entityType1, EntityType entityType2)
        {
            return (entityType1.enumValue == entityType2.enumValue);
        }

        public static bool operator !=(EntityType entityType1, EntityType entityType2)
        {
            return (entityType1.enumValue != entityType2.enumValue);
        }

        public override string ToString()
        {
            return this.Description;
        }

        static EntityType()
        {
            Uninitialized = new EntityType(0);
            Command = new EntityType(1);
            Response = new EntityType(2);
            Observation = new EntityType(3);
            ManagementEvent = new EntityType(4);
            TransportSettings = new EntityType(5);
            PrintTemplateField = new EntityType(6);
        }
    }





}
