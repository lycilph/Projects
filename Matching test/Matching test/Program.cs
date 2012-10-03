using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Matching_test
{
    class Post
    {
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public Post(DateTime d, string t) { Date = d; Text = t; }
        public override string ToString() { return Date.ToShortDateString() + " " + Text; }
        public override int GetHashCode()
        {
            return Date.GetHashCode() ^ Text.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            Post p = obj as Post;
            if ((System.Object)p == null)
                return false;
            return Date.Date.Equals(p.Date.Date) && Text.Equals(p.Text);
        }
    }

    class PostWrapper
    {
        public string State { get; set; }
        public Post post { get; set; }
        public PostWrapper(string s, Post p) { State = s; post = p; }
        public override string ToString() { return post + " - " + State; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Post> master = new List<Post>();
            for (int i = 0; i < 10; ++i)
                master.Add(new Post(DateTime.Now.AddDays(i), string.Format("Post {0}", i)));
            //master.Last().Text = "last";
            Console.WriteLine("Master");
            foreach (Post p in master)
                Console.WriteLine(p);

            List<Post> imported = new List<Post>();
            for (int i = 7; i < 17; ++i)
                imported.Add(new Post(DateTime.Now.AddDays(i), string.Format("Post {0}", i)));
            //imported.Insert(0, new Post(DateTime.Now.AddDays(5), string.Format("Post {0}", 5)));
            Console.WriteLine("Imported");
            foreach (Post p in imported)
                Console.WriteLine(p);

            Console.WriteLine("Extension methods version");
            foreach (Post p in master.GetOverlapWith(imported))
                Console.WriteLine(p);
            Console.WriteLine(master.HasOverlap(imported, 3));

            var overlap = master.GetOverlapWith(imported);
            foreach (var p in overlap)
                p.Text = "Duplicate";
            foreach (var p in imported)
                Console.WriteLine(p);

            var wrapped_posts = overlap.Select(p => new PostWrapper("Valid", p));
            foreach (var p in wrapped_posts)
                Console.WriteLine(p);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
