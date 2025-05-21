namespace APDP.Lab2;

public abstract class LibraryItem
{
    protected abstract string Title { get; set; }
    protected abstract string Author { get; set; }
    protected abstract DateTime PublicationDate { get; set; }
    protected abstract bool IsAvailable { get; set; }

    public abstract string getTitle();
    public abstract string getAuthor();
    public abstract DateTime getPublicationDate();
    public abstract bool isAvailable();
}