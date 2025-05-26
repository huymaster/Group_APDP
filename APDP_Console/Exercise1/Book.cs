namespace APDP.Exercise1;

public class Book(string title, string author)
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public string Title { get; } = title;
    public string Author { get; } = author;

    public override string ToString()
    {
        return $"Book[{Title} by {Author}]";
    }
}