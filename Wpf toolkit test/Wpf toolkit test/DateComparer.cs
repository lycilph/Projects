using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Wpf_toolkit_test
{
    public class DateComparer : Comparer<Data>
    {
        public ListSortDirection SortDirection { get; set; }

        public DateComparer() : this(ListSortDirection.Ascending) {}
        public DateComparer(ListSortDirection dir)
        {
            SortDirection = dir;
        }

        public override int Compare(Data x, Data y)
        {
            var result = x.Date.CompareTo(y.Date);
            if (SortDirection == ListSortDirection.Ascending)
                result = result * -1;

            return result;
        }
    }
}
