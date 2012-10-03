using System;

namespace Model
{
    public class Post
    {
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public double Value { get; set; }
        public Post(DateTime d, string t, double v)
        {
            Date = d;
            Text = t;
            Value = v;
        }
    }
}
