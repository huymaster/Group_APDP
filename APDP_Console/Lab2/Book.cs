namespace APDP.Lab2;

public class Book(
    string title,
    string author,
    DateTime publicationDate,
    bool isAvailable,
    string genre
) : LibraryItem
{
    protected sealed override string Title { get; set; } = title;
    protected sealed override string Author { get; set; } = author;
    protected sealed override DateTime PublicationDate { get; set; } = publicationDate;
    protected sealed override bool IsAvailable { get; set; } = isAvailable;
    protected string Genre { get; } = genre;

    public string getTitle()
    {
        return Title;
    }

    public string getAuthor()
    {
        return Author;
    }

    public DateTime getPublicationDate()
    {
        return PublicationDate;
    }

    public bool isAvailable()
    {
        return IsAvailable;
    }

    public string getGenre()
    {
        return Genre;
    }
}