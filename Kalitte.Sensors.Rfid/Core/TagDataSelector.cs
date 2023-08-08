using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Rfid.Core
{


    [Serializable]
    public class TagDataSelector
    {
        // Fields
        private const int ALL_VALUE = 0x7b;
        private const int DATA_VALUE = 2;
        private int dataSelector = 0x10;
        private const int ID_VALUE = 1;
        private const int NOT_DATA_VALUE = 0xfffd;
        private const int NOT_ID_VALUE = 0xfffe;
        private const int NOT_NSI_VALUE = 0xffbf;
        private const int NOT_SOURCE_VALUE = 0xffef;
        private const int NOT_TIME_VALUE = 0xffdf;
        private const int NOT_TYPE_VALUE = 0xfff7;
        private const int NSI_VALUE = 0x40;
        private const int SOURCE_VALUE = 0x10;
        private const int TIME_VALUE = 0x20;
        private const int TYPE_VALUE = 8;

        // Methods
        private static bool AreEqual(TagDataSelector selector1, TagDataSelector selector2)
        {
            if (!object.ReferenceEquals(null, selector1) && !object.ReferenceEquals(null, selector2))
            {
                return selector1.dataSelector.Equals(selector2.dataSelector);
            }
            return (object.ReferenceEquals(null, selector1) && object.ReferenceEquals(null, selector2));
        }

        public override bool Equals(object obj)
        {
            return ((obj is TagDataSelector) && AreEqual(this, (TagDataSelector)obj));
        }

        public override int GetHashCode()
        {
            return this.dataSelector;
        }

        internal bool IsOnlyOneFieldSelected()
        {
            return (0 == (this.dataSelector & (this.dataSelector - 1)));
        }

        public static bool operator ==(TagDataSelector dataSelector1, TagDataSelector dataSelector2)
        {
            return AreEqual(dataSelector1, dataSelector2);
        }

        public static bool operator !=(TagDataSelector dataSelector1, TagDataSelector dataSelector2)
        {
            return !AreEqual(dataSelector1, dataSelector2);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<tagDataSelector>");
            builder.Append("<isId>");
            builder.Append(this.IsId);
            builder.Append("</isId>");
            builder.Append("<isData>");
            builder.Append(this.IsData);
            builder.Append("</isData>");
            builder.Append("<isType>");
            builder.Append(this.IsType);
            builder.Append("</isType>");
            builder.Append("<isTime>");
            builder.Append(this.IsTime);
            builder.Append("</isTime>");
            builder.Append("<isNumberingSystemIdentifier>");
            builder.Append(this.IsNumberingSystemIdentifier);
            builder.Append("</isNumberingSystemIdentifier>");
            builder.Append("</tagDataSelector>");
            return builder.ToString();
        }

        // Properties
        public static TagDataSelector All
        {
            get
            {
                TagDataSelector selector = new TagDataSelector();
                selector.dataSelector = 0x7b;
                return selector;
            }
        }

        public bool IsData
        {
            get
            {
                return (0 != (this.dataSelector & 2));
            }
            set
            {
                if (value)
                {
                    this.dataSelector |= 2;
                }
                else
                {
                    this.dataSelector &= 0xfffd;
                }
            }
        }

        public bool IsId
        {
            get
            {
                return (0 != (this.dataSelector & 1));
            }
            set
            {
                if (value)
                {
                    this.dataSelector |= 1;
                }
                else
                {
                    this.dataSelector &= 0xfffe;
                }
            }
        }

        public bool IsInitialized
        {
            get
            {
                return (0 != this.dataSelector);
            }
        }

        public bool IsNumberingSystemIdentifier
        {
            get
            {
                return (0 != (this.dataSelector & 0x40));
            }
            set
            {
                if (value)
                {
                    this.dataSelector |= 0x40;
                }
                else
                {
                    this.dataSelector &= 0xffbf;
                }
            }
        }

        protected bool IsSource
        {
            get
            {
                return (0 != (this.dataSelector & 0x10));
            }
            set
            {
                if (value)
                {
                    this.dataSelector |= 0x10;
                }
                else
                {
                    this.dataSelector &= 0xffef;
                }
            }
        }

        public bool IsTime
        {
            get
            {
                return (0 != (this.dataSelector & 0x20));
            }
            set
            {
                if (value)
                {
                    this.dataSelector |= 0x20;
                }
                else
                {
                    this.dataSelector &= 0xffdf;
                }
            }
        }

        public bool IsType
        {
            get
            {
                return (0 != (this.dataSelector & 8));
            }
            set
            {
                if (value)
                {
                    this.dataSelector |= 8;
                }
                else
                {
                    this.dataSelector &= 0xfff7;
                }
            }
        }
    }








}
