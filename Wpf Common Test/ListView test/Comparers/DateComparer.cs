using System;
using System.Collections.Generic;
using System.ComponentModel;
using Model;
using SortUtils;

namespace ListView_test.Comparers
{
    public class DateComparer : BaseComparer<Post>
    {
        public DateComparer() : base("Date", ListSortDirection.Ascending) {}
        public DateComparer(ListSortDirection dir) : base("Date", dir) {}

        public override int Compare(Post x, Post y)
        {
            var result = x.Date.CompareTo(y.Date);
            if (Direction == ListSortDirection.Descending)
                result = result * -1;

            return result;
        }
    }
}
