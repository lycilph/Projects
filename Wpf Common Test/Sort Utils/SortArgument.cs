using System.ComponentModel;

namespace SortUtils
{
    public class SortArgument
    {
        public string Property { get; set; }
        public ListSortDirection? Direction { get; set; }

        public SortArgument(string p, ListSortDirection? d)
        {
            Property = p;
            Direction = d;
        }
    }
}
