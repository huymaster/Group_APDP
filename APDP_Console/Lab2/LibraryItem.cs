namespace APDP.Lab2;

public abstract class LibraryItem
{
    protected abstract string Title { get; set; }
    protected abstract string Author { get; set; }
    protected abstract DateTime PublicationDate { get; set; }
}