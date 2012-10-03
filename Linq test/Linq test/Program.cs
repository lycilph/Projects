using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Linq_test
{
    class Category
    {
        public string Name { get; set; }
        public Category(string n) { Name = n; }
        public override string ToString() { return Name; }
    }

    class Post
    {
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public double Value { get; set; }
        public Category Match { get; set; }
        public Post(DateTime d, string t, double v, Category c) { Date = d; Text = t; Value = v; Match = c; }
        public override string ToString() { return string.Format("{0} {1} {2} - {3}", Date, Text, Value, Match); }
    }

    class Account
    {
        public ObservableCollection<Post> Posts { get; set; }
        public Account() { Posts = new ObservableCollection<Post>(); }
    }

    class Profile
    {
        public ObservableCollection<Account> Accounts { get; set; }
        public Profile() { Accounts = new ObservableCollection<Account>(); }
    }

    class GroupByDate
    {
        public DateTime Date { get; set; }
        public double Total { get; set; }
        public GroupByDate(DateTime d, double t) { Date = d; Total = t; }
        public override string ToString() { return Date + " - " + Total; }
    }

    static class Extensions
    {
        public static IEnumerable<Post> AllPosts(this Profile profile)
        {
            return profile.Accounts.SelectMany(a => a.Posts);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("All categories");
            List<Category> categories = new List<Category>();
            categories.Add(new Category("Category 1"));
            categories.Add(new Category("Category 2"));
            categories.Add(new Category("Category 3"));
            categories.ForEach(c => Console.WriteLine(c));

            Console.WriteLine("All posts");
            List<Post> posts = new List<Post>();
            posts.Add(new Post(DateTime.Now.AddDays(0), "Post 1", 1.1, categories[0]));
            posts.Add(new Post(DateTime.Now.AddDays(5), "Post 2", 2.2, categories[1]));
            posts.Add(new Post(DateTime.Now.AddDays(5), "Post 3", 3.3, categories[2]));
            posts.Add(new Post(DateTime.Now.AddDays(2), "Post 4", 4.4, categories[1]));
            posts.Add(new Post(DateTime.Now.AddDays(3), "Post 5", 5.5, categories[1]));
            posts.Add(new Post(DateTime.Now.AddDays(1), "Post 6", 6.6, categories[2]));
            posts.Add(new Post(DateTime.Now.AddDays(1), "Post 7", 7.7, categories[2]));
            posts.Add(new Post(DateTime.Now.AddDays(5), "Post 8", 8.8, categories[0]));
            posts.Add(new Post(DateTime.Now.AddDays(6), "Post 9", 9.9, categories[1]));
            posts.ForEach(p => Console.WriteLine(p));

            Profile profile = new Profile();
            profile.Accounts.Add(new Account());
            profile.Accounts[0].Posts.Add(posts[0]);
            profile.Accounts[0].Posts.Add(posts[1]);
            profile.Accounts[0].Posts.Add(posts[2]);
            profile.Accounts.Add(new Account());
            profile.Accounts[1].Posts.Add(posts[3]);
            profile.Accounts[1].Posts.Add(posts[4]);
            profile.Accounts[1].Posts.Add(posts[5]);
            profile.Accounts.Add(new Account());
            profile.Accounts[2].Posts.Add(posts[6]);
            profile.Accounts[2].Posts.Add(posts[7]);
            profile.Accounts[2].Posts.Add(posts[8]);

            Console.WriteLine("Posts in category, sorted");
            var posts_by_category = from p in posts
                                    where p.Match == categories[1]
                                    orderby p.Date
                                    select p;
            foreach (var p in posts_by_category)
                Console.WriteLine(p);

            Console.WriteLine("Sum of posts by category");
            var grouped_by_category = from p in posts
                                      group p by p.Match into g
                                      select new { Category = g.Key, Total = g.Sum(p => p.Value) };
            foreach (var g in grouped_by_category)
                Console.WriteLine(g);

            Console.WriteLine("Sum of posts by date");
            var grouped_by_date = from p in posts
                                  group p by p.Date into g
                                  orderby g.Key
                                  select new GroupByDate(g.Key, g.Sum(p => p.Value));
            foreach (var g in grouped_by_date)
                Console.WriteLine(g);
            var oc = new ObservableCollection<GroupByDate>(grouped_by_date);
            foreach (var g in oc)
                Console.WriteLine(g);

            Console.WriteLine("Flattened profile hierarchy");
            var flattened_hierarchy = profile.Accounts.SelectMany(a => a.Posts);
            foreach (var p in flattened_hierarchy)
                Console.WriteLine(p);

            Console.WriteLine("Flattened profile hierarchy - extension method");
            foreach (var p in profile.AllPosts())
                Console.WriteLine(p);

            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
}