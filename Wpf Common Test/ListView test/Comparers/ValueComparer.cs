using System.Collections.Generic;
using System.ComponentModel;
using Model;
using SortUtils;

namespace ListView_test.Comparers
{
    public class ValueComparer : BaseComparer<Post>
    {
        public ValueComparer() : base("Value", ListSortDirection.Ascending) {}
        public ValueComparer(ListSortDirection dir) : base("Value", dir) {}

        public override int Compare(Post x, Post y)
        {
            int result = 0;

            if (x.Value < y.Value)
                result = -1;
            else if (x.Value > y.Value)
                result = 1;
            
            if (Direction == ListSortDirection.Descending)
                result = result * -1;

            return result;
        }
    }
}
