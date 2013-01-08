using System.Collections.Generic;

namespace CategorizationEngine
{
    public class HashPattern : IPattern
    {
        public HashSet<Post> Set { get; set; }

        public string Description
        {
            get { return "Items count: " + Set.Count; }
        }

        public HashPattern()
        {
            Set = new HashSet<Post>();
        }

        public bool IsMatch(Post post)
        {
            return Set.Contains(post);
        }
    }
}
