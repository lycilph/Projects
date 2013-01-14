using System.Collections.Generic;

namespace CategorizationEngine
{
    public class Account : ObservableObject
    {
        // Properties that should be serialized
        public string Name { get; set; }
        public List<Post> Posts { get; private set; }

        public Account()
        {
            Name = string.Empty;
            Posts = new List<Post>();
        }

        public void Add(List<Post> posts)
        {
            foreach (var post in posts)
                post.Account = this;
            Posts.AddRange(posts);
        }
    }
}
