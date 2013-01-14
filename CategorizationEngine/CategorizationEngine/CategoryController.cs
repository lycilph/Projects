using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace CategorizationEngine
{
    public class CategoryController
    {
        private readonly Profile profile;

        public CategoryController(Profile profile)
        {
            this.profile = profile;
        }

        public long Categorize()
        {
            ResetCategories();

            var sw = Stopwatch.StartNew();
            foreach (var post in profile.AggregatedPosts())
            {
                foreach (var category in Category.Flatten(profile.RootCategory).Where(c => c.Filters.Count > 0))
                {
                    if (category.IsMatch(post))
                    {
                        category.Posts.Add(post);
                        post.Categories.Add(category);
                    }
                }
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        public long ResetCategories()
        {
            var sw = Stopwatch.StartNew();
            foreach (var post in profile.AggregatedPosts())
                post.Categories.Clear();

            profile.RootCategory.ClearPosts();
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }
        
        public void Move(string c1, string c2)
        {
            var category1 = Find(c1);
            var category2 = Find(c2);
            if (category1 == null || category2 == null)
                return;

            category1.Parent.Remove(category1);
            category2.Add(category1);
        }

        public Category Find(string pattern, Category root)
        {
            //if (root.Name == c)
            //    return root;
            if (Regex.IsMatch(root.Name, pattern))
                return root;

            foreach (var category in root.Categories)
            {
                var result = Find(pattern, category);
                if (result != null)
                    return category;
            }

            return null;
        }

        public Category Find(string pattern)
        {
            return Find(pattern, profile.RootCategory);
        }
    }
}
