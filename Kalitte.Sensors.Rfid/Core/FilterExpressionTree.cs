namespace Kalitte.Sensors.Rfid.Core
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;

    [Serializable]
    public sealed class FilterExpressionTree : IEquatable<FilterExpressionTree>
    {
        private readonly FilterExpressionTree leftTree;
        private readonly FilterLogicalOperator logicalOperator;
        private readonly ReadFilter readFilter;
        private readonly FilterExpressionTree rightTree;

        public FilterExpressionTree(ReadFilter readFilter)
        {
            this.readFilter = readFilter;
            this.ValidateReadFilter();
        }

        public FilterExpressionTree(FilterExpressionTree leftTree, FilterExpressionTree rightTree, FilterLogicalOperator logicalOperator)
        {
            this.leftTree = leftTree;
            this.rightTree = rightTree;
            this.logicalOperator = logicalOperator;
            this.ValidateSubTree();
        }

        public bool Equals(FilterExpressionTree other)
        {
            if (other == null)
            {
                return false;
            }
            return ((((((this.leftTree == null) && (other.leftTree == null)) || ((this.leftTree != null) && this.leftTree.Equals(other.leftTree))) && (((this.rightTree == null) && (other.rightTree == null)) || ((this.rightTree != null) && this.rightTree.Equals(other.rightTree)))) && (((this.readFilter == null) && (other.readFilter == null)) || ((this.readFilter != null) && this.readFilter.Equals(other.readFilter)))) && (this.logicalOperator == other.logicalOperator));
        }

        public override string ToString()
        {
            if (this.readFilter != null)
            {
                return this.readFilter.ToString();
            }
            StringBuilder builder = new StringBuilder();
            builder.Append('(');
            builder.Append('(');
            builder.Append(this.leftTree);
            builder.Append(')');
            builder.Append((FilterLogicalOperator.And == this.logicalOperator) ? " && " : " || ");
            builder.Append('(');
            builder.Append(this.rightTree);
            builder.Append(')');
            builder.Append(')');
            return builder.ToString();
        }

        private void ValidateParameters()
        {
            bool flag = this.readFilter != null;
            bool flag2 = ((this.logicalOperator != FilterLogicalOperator.Uninitialized) || (this.leftTree != null)) || (null != this.rightTree);
            if (flag == flag2)
            {
                throw new ArgumentException("InvalidReadFilterTree");
            }
            if (flag)
            {
                this.ValidateReadFilter();
            }
            if (flag2)
            {
                this.ValidateSubTree();
            }
        }

        [OnDeserialized]
        private void ValidateParameters(StreamingContext context)
        {
            this.ValidateParameters();
        }

        private void ValidateReadFilter()
        {
            if (this.readFilter == null)
            {
                throw new ArgumentNullException("readFilter");
            }
        }

        private void ValidateSubTree()
        {
            if (this.leftTree == null)
            {
                throw new ArgumentNullException("leftTree");
            }
            if (this.rightTree == null)
            {
                throw new ArgumentNullException("rightTree");
            }
            if (this.logicalOperator == FilterLogicalOperator.Uninitialized)
            {
                throw new ArgumentException("InvalidOperator");
            }
        }

        public FilterExpressionTree LeftTree
        {
            get
            {
                return this.leftTree;
            }
        }

        public FilterLogicalOperator LogicalOperator
        {
            get
            {
                return this.logicalOperator;
            }
        }

        public ReadFilter ReadFilter
        {
            get
            {
                return this.readFilter;
            }
        }

        public FilterExpressionTree RightTree
        {
            get
            {
                return this.rightTree;
            }
        }

        public enum FilterLogicalOperator
        {
            Uninitialized,
            And,
            Or
        }
    }
}
