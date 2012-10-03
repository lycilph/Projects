using System;
using System.ComponentModel;
using SortUtils;
using Model;

namespace ListView_test.Comparers
{
    public class ComparerFactory : IComparerFactory<Post>
    {
        public BaseComparer<Post> Create(SortArgument sort_argument)
        {
            if (!sort_argument.Direction.HasValue)
                throw new ArgumentException("SortArgument direction must NOT be null");

            return Create(sort_argument.Property, sort_argument.Direction.Value);
        }

        public BaseComparer<Post> Create(string property)
        {
            return Create(property, ListSortDirection.Ascending);
        }

        public BaseComparer<Post> Create(string property, ListSortDirection direction)
        {
            switch (property)
            {
                case "Date": return new DateComparer(direction);
                case "Text": return new TextComparer(direction);
                case "Value": return new ValueComparer(direction);
                default:
                    throw new Exception("No comparer found for property: " + property);
            }
        }
    }
}
