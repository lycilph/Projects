using System.Text.RegularExpressions;

namespace CategorizationEngine.Filters
{
    public class RegexFilter : IFilter
    {
        public Regex Exp { get; set; }

        public string Description
        {
            get { return Exp.ToString(); }
        }

        public RegexFilter(Regex regex)
        {
            Exp = regex;
        }

        public bool IsMatch(Post post)
        {
            return Exp.IsMatch(post.Text);
        }
    }
}
