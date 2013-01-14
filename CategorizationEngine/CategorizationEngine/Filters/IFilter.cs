namespace CategorizationEngine.Filters
{
    public interface IFilter
    {
        string Description { get; }

        bool IsMatch(Post post);
    }
}
