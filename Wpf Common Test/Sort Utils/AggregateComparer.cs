using System.Collections.Generic;
using System.Windows.Data;

namespace SortUtils
{
    public class AggregateComparer<T> : BaseComparer<T>
    {
        private readonly ListCollectionView view = null;
        private readonly IComparerFactory<T> factory = null;
        public readonly List<BaseComparer<T>> comparers = new List<BaseComparer<T>>();

        public AggregateComparer(ListCollectionView v, IComparerFactory<T> f)
        {
            view = v;
            view.CustomSort = this;
            factory = f;
        }

        public void Update(SortArgument sort_argument)
        {
            // Find comparer
            var comparer = comparers.Find(c => c.Property == sort_argument.Property);

            if (sort_argument.Direction.HasValue) // Sort direction is NOT null, so this is either a new sort property or an old property that must change direction
            {
                if (comparer != null) // This is an old property that must change direction
                {
                    comparer.Direction = sort_argument.Direction.Value;
                    view.Refresh();
                }
                else // This is a new sort property that must be added
                {
                    Add(factory.Create(sort_argument));
                }
            }
            else // Sort direction is null, so old comparers/sorters must be removed
            {
                if (comparer != null)
                    Remove(comparer);
            }
        }

        public void Add(BaseComparer<T> c)
        {
            comparers.Add(c);
            view.Refresh();
        }

        public void Remove(BaseComparer<T> c)
        {
            comparers.Remove(c);
            view.Refresh();
        }

        public void Refresh()
        {
            view.Refresh();
        }

        public override int Compare(T x, T y)
        {
            int result = 0;

            foreach (var c in comparers)
            {
                result = c.Compare(x, y);
                if (result != 0)
                    return result;
            }

            return result;
        }
    }
}
