using System.Collections.Generic;
using System.Linq;
using CategorizationEngine.Filters;

namespace CategorizationEngine
{
    public class Category
    {
        public Category Parent { get; private set; }

        public string Name { get; set; }
        public List<Category> Categories { get; private set; }
        public List<IFilter> Filters { get; private set; }
        public List<Post> Posts { get; private set; }

        public Category(string name = "")
        {
            Parent = null;

            Name = name;
            Categories = new List<Category>();
            Filters = new List<IFilter>();
            Posts = new List<Post>();
        }

        public void Add(Category category)
        {
            category.Parent = this;
            Categories.Add(category);
        }

        public void Remove(Category category)
        {
            category.Parent = null;
            Categories.Remove(category);
        }

        public void ClearPosts()
        {
            Posts.Clear();
            foreach (var category in Categories)
                category.ClearPosts();
        }

        public bool IsMatch(Post post)
        {
            return Filters.All(pattern => pattern.IsMatch(post));
        }

        public IEnumerable<Post> AggregatePosts()
        {
            IEnumerable<Post> result = new List<Post>(Posts);
            return Categories.Aggregate(result, (current, category) => current.Concat(category.AggregatePosts()));
        }

        public static List<Category> Flatten(Category root)
        {
            var flattened = new List<Category> {root};
            foreach (var category in root.Categories)
                flattened.AddRange(Flatten(category));
            return flattened; 
        }
    }
}
