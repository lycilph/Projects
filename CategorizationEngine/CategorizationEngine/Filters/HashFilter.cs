using System.Collections.Generic;

namespace CategorizationEngine.Filters
{
    public class HashFilter : IFilter
    {
        public HashSet<Post> Set { get; set; }

        public string Description
        {
            get { return "Items count: " + Set.Count; }
        }

        public HashFilter()
        {
            Set = new HashSet<Post>();
        }

        public bool IsMatch(Post post)
        {
            return Set.Contains(post);
        }
    }
}
