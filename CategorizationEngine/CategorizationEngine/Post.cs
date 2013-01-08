using System;
using System.Collections.Generic;
using System.Text;

namespace CategorizationEngine
{
    public class Post
    {
        private static readonly Random rand = new Random();

        public DateTime Date { get; set; }
        public string Text { get; set; }
        public double Value { get; set; }

        public List<Category> Categories { get; private set; }

        public Post()
        {
            Categories = new List<Category>();
        }

        public override string ToString()
        {
            var sb = new StringBuilder(Date.ToShortDateString() + " " + Text + " " + Value);
            foreach (var category in Categories)
            {
                sb.Append(" " + category.Name);
            }
            return sb.ToString();
        }
        
        public static Post Random()
        {
            var p = new Post();

            // Generate random date
            var start = new DateTime(2000, 1, 1);
            var range = (DateTime.Today - start).Days;
            p.Date = start.AddDays(rand.Next(range));

            // Generate random text
            p.Text = Wordlist.Instance.Random(3);

            // Generate random value
            p.Value = rand.NextDouble()*1000;

            return p;
        }

        public static List<Post> Random(int number)
        {
            List<Post> result = new List<Post>();
            for (var i = 0; i < number; i++)
                result.Add(Random());
            return result;
        }
    }
}
