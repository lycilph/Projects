namespace CategorizationEngine
{
    public interface IPattern
    {
        string Description { get; }

        bool IsMatch(Post post);
    }
}
