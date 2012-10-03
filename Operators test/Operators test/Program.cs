using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Operators_test
{
    class Post
    {
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public double Value { get; set; }
        public Post(Post p) : this(p.Date, p.Text, p.Value) {}
        public Post(DateTime d, string t, double v) { Date = d; Text = t; Value = v; }
        public override string ToString() { return string.Format("{0} {1} {2} - ({3})", Date, Text, Value, GetHashCode()); }
        public override int GetHashCode()
        {
            return Date.GetHashCode() ^ Text.GetHashCode() ^ Value.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            Post p = obj as Post;
            if ((System.Object)p == null)
                return false;
            return Date.Equals(p.Date) && Text.Equals(p.Text) && Value.Equals(p.Value);
        }
        public static Post Random()
        {
            Random rnd = new Random();
            DateTime d = DateTime.Now.AddDays(rnd.Next(0, 10));
            string t = Path.GetRandomFileName().Replace(".", "");
            double v = rnd.NextDouble() * 1000;
            return new Post(d, t, v);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Post p1 = Post.Random();
            Post p2 = new Post(p1);
            Console.WriteLine(p1);
            Console.WriteLine(p2);

            Console.WriteLine("P1 == P2 " + (p1 == p2));
            Console.WriteLine("P1 equals P2 " + p1.Equals(p2));

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
