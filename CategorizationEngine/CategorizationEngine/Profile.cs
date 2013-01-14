using System.Collections.Generic;

namespace CategorizationEngine
{
    public class Profile
    {
        public string Name { get; set; }
        public List<Account> Accounts { get; private set; }
        public Category RootCategory { get; private set; }

        public Profile()
        {
            Name = string.Empty;
            Accounts = new List<Account>();
            RootCategory = new Category("Root");
        }

        public List<Post> AggregatedPosts()
        {
            var aggregated_posts = new List<Post>();
            foreach (var account in Accounts)
                aggregated_posts.AddRange(account.Posts);
            return aggregated_posts;
        }
    }
}
