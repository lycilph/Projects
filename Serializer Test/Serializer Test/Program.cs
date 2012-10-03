using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Linq_Test
{
    class Category
    {
        public string Name { get; set; }
        public Regex Expression { get; set; }
        public Category(string n) : this(n, "") { }
        public Category(string n, string e) { Name = n; Expression = new Regex(e); }
        [JsonConstructor]
        public Category(string n, Regex r) { Name = n; Expression = r; }
    }

    class Post
    {
        public enum CategoryMatchType { Auto, Manual }

        public string Text { get; set; }
        public Category Match { get; set; }
        public CategoryMatchType MatchType { get; set; }
        public Post(string s) { Text = s; }
    }

    class Profile
    {
        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<Post> Posts { get; set; }
        public Profile()
        {
            Categories = new ObservableCollection<Category>();
            Posts = new ObservableCollection<Post>();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Profile profile = new Profile();
            profile.Categories.Add(new Category("Category 1", ".*"));
            profile.Categories.Add(new Category("Category 2", ".*"));
            profile.Categories.Add(new Category("Category 3", ".*"));
            profile.Posts.Add(new Post("Post 1"));
            profile.Posts.Add(new Post("Post 2"));
            profile.Posts.Add(new Post("Post 3"));

            profile.Posts[0].Match = profile.Categories[0];
            profile.Posts[0].MatchType = Post.CategoryMatchType.Auto;
            profile.Posts[1].Match = profile.Categories[1];
            profile.Posts[1].MatchType = Post.CategoryMatchType.Manual;
            profile.Posts[2].Match = profile.Categories[2];
            profile.Posts[2].MatchType = Post.CategoryMatchType.Auto;

            // Serialize
            string output = JsonConvert.SerializeObject(profile, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            Console.WriteLine(output);
            // Deserialize
            Profile output_profile = JsonConvert.DeserializeObject<Profile>(output);


            IEnumerable<Post> auto_posts = profile.Posts.Where(x => x.MatchType == Post.CategoryMatchType.Auto);
            foreach (Post post in auto_posts)
                Console.WriteLine(post.Text + " -> matched with " + post.Match.Name);

            Post single_post = profile.Posts.First(x => x.Text.Contains("2"));
            Console.WriteLine(single_post.Text);

            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
