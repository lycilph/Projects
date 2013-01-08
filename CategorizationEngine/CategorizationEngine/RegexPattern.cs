using System.Text.RegularExpressions;

namespace CategorizationEngine
{
    public class RegexPattern : IPattern
    {
        public Regex Ex { get; set; }

        public string Description
        {
            get { return Ex.ToString(); }
        }

        public RegexPattern(Regex regex)
        {
            Ex = regex;
        }

        public bool IsMatch(Post post)
        {
            return Ex.IsMatch(post.Text);
        }
    }
}
