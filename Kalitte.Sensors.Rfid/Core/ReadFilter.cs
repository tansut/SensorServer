namespace Kalitte.Sensors.Rfid.Core
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Text.RegularExpressions;
    using Kalitte.Sensors.Core;
    using Kalitte.Sensors.Utilities;

    [Serializable, KnownType(typeof(RegexOptions))]
    public sealed class ReadFilter : IEquatable<ReadFilter>
    {
        private readonly ComparePatternByteArray byteArrayValueComparisonPattern;
        private readonly bool invertMatch;
        private readonly Regex stringPattern;
        private readonly TagType tagType;
        private readonly FilterDataSelector targetField;
        private VendorData vendorSpecificData;

        public ReadFilter(ComparePatternByteArray pattern, FilterDataSelector targetField, bool invertMatch)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException("pattern");
            }
            this.byteArrayValueComparisonPattern = pattern;
            validate(targetField);
            if ((!targetField.IsId && !targetField.IsData) && !targetField.IsNumberingSystemIdentifier)
            {
                throw new ArgumentException("InvalidPattern");
            }
            this.targetField = targetField;
            this.invertMatch = invertMatch;
        }

        public ReadFilter(TagType tagType, FilterDataSelector targetField, bool invertMatch)
        {
            if (TagType.Uninitialized == tagType)
            {
                throw new ArgumentNullException("tagType");
            }
            this.tagType = tagType;
            validate(targetField);
            if (!targetField.IsType)
            {
                throw new ArgumentException("InvalidPattern");
            }
            this.targetField = targetField;
            this.invertMatch = invertMatch;
        }

        public ReadFilter(Regex pattern, FilterDataSelector targetField, bool invertMatch)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException("pattern");
            }
            this.stringPattern = pattern;
            validate(targetField);
            if (!targetField.IsSource)
            {
                throw new ArgumentException("InvalidPattern");
            }
            this.targetField = targetField;
            this.invertMatch = invertMatch;
        }

        public bool Equals(ReadFilter other)
        {
            if (other == null)
            {
                return false;
            }
            return (((((this.stringPattern != null) && this.stringPattern.Equals(other.stringPattern)) || ((this.byteArrayValueComparisonPattern != null) && this.byteArrayValueComparisonPattern.Equals(other.byteArrayValueComparisonPattern))) && ((this.invertMatch == other.invertMatch) && this.targetField.Equals(other.targetField))) && CollectionsHelper.CompareDictionaries(this.vendorSpecificData, other.vendorSpecificData));
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<readFilter>");
            if (this.stringPattern != null)
            {
                builder.Append("<pattern>");
                builder.Append(this.stringPattern);
                builder.Append("</pattern>");
            }
            else
            {
                builder.Append(this.byteArrayValueComparisonPattern);
            }
            builder.Append("<invertMatch>");
            builder.Append(this.invertMatch);
            builder.Append("</invertMatch>");
            builder.Append("<targetField>");
            builder.Append(this.targetField);
            builder.Append("</targetField>");
            builder.Append("</readFilter>");
            return builder.ToString();
        }

        private static void validate(FilterDataSelector targetField)
        {
            if (!targetField.IsInitialized)
            {
                throw new ArgumentException("InvalidSelectorValue");
            }
            if (!targetField.IsOnlyOneFieldSelected())
            {
                throw new ArgumentException("MultipleFields");
            }
        }

        public ComparePatternByteArray ByteArrayValueComparisonPattern
        {
            get
            {
                return this.byteArrayValueComparisonPattern;
            }
        }

        public bool InvertMatch
        {
            get
            {
                return this.invertMatch;
            }
        }

        public Regex StringPattern
        {
            get
            {
                return this.stringPattern;
            }
        }

        public TagType TagType
        {
            get
            {
                return this.tagType;
            }
        }

        public FilterDataSelector TargetField
        {
            get
            {
                return this.targetField;
            }
        }

        public VendorData VendorSpecificData
        {
            get
            {
                return this.vendorSpecificData;
            }
            set
            {
                this.vendorSpecificData = value;
            }
        }
    }
}
