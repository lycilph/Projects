using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matching_test
{
    public static class MatchExtensions
    {
        public static IEnumerable<T> GetOverlapWith<T>(this IEnumerable<T> enumerable_1, IEnumerable<T> enumerable_2)
        {
            List<T> list_1 = enumerable_1.ToList();
            List<T> list_2 = enumerable_2.ToList();

            // Find match start
            T match_start = list_1.Last(x => x.Equals(list_2.First()));
            int match_start_index = list_1.IndexOf(match_start);

            // Bail out if no match start was found
            if (match_start_index == -1)
                return null;

            // Add overlapping items to list, bail out if overlap sequence stops before end of list 2
            List<T> result = new List<T>();
            for (int i = match_start_index; i < list_1.Count; ++i)
            {
                T t1 = list_1[i];
                T t2 = list_2[i - match_start_index];

                if (t1.Equals(t2))
                    result.Add(t2);
                else
                    return null;
            }
            return result;
        }

        public static bool HasOverlap<T>(this IEnumerable<T> enumerable_1, IEnumerable<T> enumerable_2, int min_overlap)
        {
            var overlap = enumerable_1.GetOverlapWith(enumerable_2);
            return (overlap != null) && (overlap.Count() >= min_overlap);
        }
    }
}
