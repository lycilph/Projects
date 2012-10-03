using System.Collections.Generic;
using System.ComponentModel;

namespace Wpf_toolkit_test
{
    public class ValueComparer : Comparer<Data>
    {
        public ListSortDirection SortDirection { get; set; }

        public ValueComparer() : this(ListSortDirection.Ascending) { }
        public ValueComparer(ListSortDirection dir)
        {
            SortDirection = dir;
        }

        public override int Compare(Data x, Data y)
        {
            int result = 0;

            if (x.Value < y.Value)
                result = -1;
            else if (x.Value > y.Value)
                result = 1;
            
            if (SortDirection == ListSortDirection.Ascending)
                result = result * -1;

            return result;
        }
    }
}
