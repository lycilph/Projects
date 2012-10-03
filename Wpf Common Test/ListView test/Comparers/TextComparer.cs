using System;
using System.Collections.Generic;
using System.ComponentModel;
using Model;
using SortUtils;

namespace ListView_test.Comparers
{
    public class TextComparer : BaseComparer<Post>
    {
        public TextComparer() : base("Text", ListSortDirection.Ascending) {}
        public TextComparer(ListSortDirection dir) : base("Text", dir) {}

        public override int Compare(Post x, Post y)
        {
            StringComparer sc = StringComparer.InvariantCulture;

            var result = sc.Compare(x.Text, y.Text);
            if (Direction == ListSortDirection.Descending)
                result = result*-1;

            return result;
        }
    }
}
