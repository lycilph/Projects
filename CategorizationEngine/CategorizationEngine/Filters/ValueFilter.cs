using System;

namespace CategorizationEngine.Filters
{
    public enum ValueOperator { Less, Around, Greater, Range };

    public class ValueFilter : IFilter
    {
        public double Value1 { get; set; }
        public double Value2 { get; set; }
        public double Epsilon { get; set; }
        public ValueOperator Operator { get; set; }

        public string Description
        {
            get { return Operator + " " + Value1 + " " + Value2 + " " + Epsilon; }
        }

        public bool IsMatch(Post post)
        {
            switch (Operator)
            {
                case ValueOperator.Less: return post.Value < Value1;
                case ValueOperator.Around: return Math.Abs(post.Value - Value1) < Epsilon;
                case ValueOperator.Greater: return post.Value > Value1;
                case ValueOperator.Range: return post.Value > Value1 && post.Value < Value2;
            }
            return false;
        }
    }
}
