using System;

namespace CategorizationEngine
{
    public enum ValueOperator { Less, Equal, Greater };

    public class ValuePattern : IPattern
    {
        public double Value { get; set; }
        public ValueOperator Operator { get; set; }

        public string Description
        {
            get { return Operator + " " + Value; }
        }

        public bool IsMatch(Post post)
        {
            switch (Operator)
            {
                case ValueOperator.Less: return post.Value < Value;
                case ValueOperator.Equal: return Math.Abs(post.Value - Value) < 0.01;
                case ValueOperator.Greater: return post.Value > Value;
            }
            return false;
        }
    }
}
