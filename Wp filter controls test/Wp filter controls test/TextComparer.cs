using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Wp_filter_controls_test
{
    public class TextComparer : Comparer<Model>
    {
        public ListSortDirection SortDirection { get; set; }

        public TextComparer() : this(ListSortDirection.Ascending) {}
        public TextComparer(ListSortDirection dir)
        {
            SortDirection = dir;
        }

        public override int Compare(Model x, Model y)
        {
            StringComparer sc = StringComparer.InvariantCulture;

            var result = sc.Compare(x.Text, y.Text);
            if (SortDirection == ListSortDirection.Descending)
                result = result*-1;

            return result;
        }
    }
}
