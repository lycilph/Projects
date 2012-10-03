using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Wpf_linq_test
{
    public class Category
    {
        public string Name { get; set; }
        public Category(string n) { Name = n; }
        public override string ToString() { return Name; }
    }

    public class Post
    {
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public double Value { get; set; }
        public Category Match { get; set; }
        public Post(DateTime d, string t, double v, Category c) { Date = d; Text = t; Value = v; Match = c; }

        public override string ToString() { return string.Format("{0} {1} {2} - {3}", Date, Text, Value, Match); }
    }

    public class Account
    {
        public string Name { get; set; }
        public ObservableCollection<Post> Posts { get; set; }
        public Account(string n) { Name = n; Posts = new ObservableCollection<Post>(); }
    }

    public class Profile
    {
        public ObservableCollection<Account> Accounts { get; set; }
        public Profile() { Accounts = new ObservableCollection<Account>(); }

        public IEnumerable<Post> GetAllPosts() { return Accounts.SelectMany(p => p.Posts); }
    }

    public class PlotData
    {
        public string Text { get; set; }
        public double Value { get; set; }
        public PlotData(string t, double v) { Text = t; Value = v; }

        public override string ToString() { return string.Format("{0} - {1}", Text, Value); }
    }

    public class Node
    {
        public string Name { get; set; }
        public ObservableCollection<Post> Posts { get; set; }
        public ObservableCollection<PlotData> Data { get; set; }
        public ObservableCollection<Node> Subnodes { get; set; }
        public Node(string n, IEnumerable<Post> p)
        {
            Name = n;
            Posts = new ObservableCollection<Post>(p);
            Data = new ObservableCollection<PlotData>();
            Subnodes = new ObservableCollection<Node>();
        }

        public override string ToString() { return Name; }

        public static void GroupByCategory(Node node)
        {
            var group_by_category = from p in node.Posts
                                    group p by p.Match into g
                                    select new PlotData(g.Key.Name, g.Sum(p => p.Value));
            node.Data = new ObservableCollection<PlotData>(group_by_category);
        }
        public static void GroupByCategoryRecursively(Node node)
        {
            GroupByCategory(node);
            foreach (Node subnode in node.Subnodes)
                GroupByCategoryRecursively(subnode);
        }
    }
}
