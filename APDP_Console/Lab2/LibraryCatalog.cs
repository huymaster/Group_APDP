namespace APDP.Lab2;

public class LibraryCatalog
{
    private readonly List<LibraryItem> items = [];

    public void AddItem(LibraryItem item)
    {
        items.Add(item);
    }

    public void FindItem(string searchQuery)
    {
        var foundItems = items.Where(item =>
            item.getTitle().Contains(searchQuery) || item.getAuthor().Contains(searchQuery));

        foreach (var item in foundItems)
            Console.WriteLine($"{item.getTitle()} by {item.getAuthor()}. Available: {item.isAvailable()}");
    }
}