using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Wp_filter_controls_test
{
    public class DateComparer : Comparer<Model>
    {
        public ListSortDirection SortDirection { get; set; }

        public DateComparer() : this(ListSortDirection.Ascending) {}
        public DateComparer(ListSortDirection dir)
        {
            SortDirection = dir;
        }

        public override int Compare(Model x, Model y)
        {
            var result = x.Date.CompareTo(y.Date);
            if (SortDirection == ListSortDirection.Ascending)
                result = result * -1;

            return result;
        }
    }
}
