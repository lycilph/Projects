using System;

namespace CategorizationEngine
{
    public enum DateOperator {Before, Equal, After};

    public class DatePattern : IPattern
    {
        public DateTime Date { get; set; }
        public DateOperator Operator { get; set; }

        public string Description
        {
            get { return Operator + " " + Date.ToShortDateString(); }
        }

        public bool IsMatch(Post post)
        {
            switch (Operator)
            {
                case DateOperator.Before: return Date.CompareTo(post.Date) > 0;
                case DateOperator.Equal: return Date.CompareTo(post.Date) == 0;
                case DateOperator.After: return Date.CompareTo(post.Date) < 0;
            }
            return false;
        }
    }
}
