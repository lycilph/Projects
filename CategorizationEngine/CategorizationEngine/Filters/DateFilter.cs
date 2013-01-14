using System;

namespace CategorizationEngine.Filters
{
    public enum DateOperator { Before, Equal, After, Range };

    public class DateFilter : IFilter
    {
        public DateTime Date1 { get; set; }
        public DateTime Date2 { get; set; }
        public DateOperator Operator { get; set; }

        public string Description
        {
            get { return Operator + " " + Date1.ToShortDateString() + " " + Date2.ToShortDateString(); }
        }

        public bool IsMatch(Post post)
        {
            switch (Operator)
            {
                case DateOperator.Before: return Date1.CompareTo(post.Date) > 0;
                case DateOperator.Equal: return Date1.CompareTo(post.Date) == 0;
                case DateOperator.After: return Date1.CompareTo(post.Date) < 0;
                case DateOperator.Range: return Date1.CompareTo(post.Date) < 0 && Date2.CompareTo(post.Date) > 0;
            }
            return false;
        }
    }
}
