namespace APDP.Lab2;

public class DVD(
    string title,
    string author,
    DateTime publicationDate,
    bool isAvailable,
    int lengthInSecond
) : LibraryItem
{
    protected sealed override string Title { get; set; } = title;
    protected sealed override string Author { get; set; } = author;
    protected sealed override DateTime PublicationDate { get; set; } = publicationDate;
    protected sealed override bool IsAvailable { get; set; } = isAvailable;
    protected int LengthInSecond { get; set; } = lengthInSecond;

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

    public int getLengthInSecond()
    {
        return LengthInSecond;
    }
}