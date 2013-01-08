using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CategorizationEngine
{
    public class Category
    {
        public string Name { get; set; }
        public ObservableCollection<Category> Categories { get; private set; }
        public List<IPattern> Patterns { get; private set; }
        public List<Post> Posts { get; private set; }

        public Category(string name = "")
        {
            Name = name;
            Categories = new ObservableCollection<Category>();
            Patterns = new List<IPattern>();
            Posts = new List<Post>();
        }

        public override string ToString()
        {
            return string.Format("{0}, categories {1}, patterns {2}", Name, Categories.Count, Patterns.Count);
        }

        public bool IsMatch(Post post)
        {
            return Patterns.Any(pattern => pattern.IsMatch(post));
        }
        
        public static List<Category> Flatten(Category root)
        {
            var flattened = new List<Category> {root};
            foreach (var category in root.Categories)
                flattened.AddRange(Flatten(category));
            return flattened; 
        }

        public static Category Random()
        {
            return new Category(Wordlist.Instance.Random());
        }
    }
}
