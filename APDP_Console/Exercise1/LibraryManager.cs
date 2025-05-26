namespace APDP.Exercise1;

public class LibraryManager
{
    private static readonly LibraryManager INSTANCE = new();

    private readonly List<Book> items = [];
    private readonly List<BorrowRecord> records = [];

    private LibraryManager()
    {
    }

    public static LibraryManager getInstance()
    {
        return INSTANCE;
    }

    public void addBook(Book book)
    {
        items.Add(book);
    }

    public void addBook(string id, string title, string author)
    {
        addBook(new Book(title, author));
    }

    public void borrowBook(User user, Book book, DateTime returnDate)
    {
        var borrowDate = DateTime.Now;
        if (records.Any(record => record.User == user && record.Book == book && record.ReturnDate > borrowDate)) return;
        var record = new BorrowRecord(user, book, borrowDate, returnDate);
        records.Add(record);
    }

    public List<Book> getAllBooks()
    {
        return items;
    }

    public List<Book> getBorrowedBooks()
    {
        return records
            .Where(record => record.ReturnDate > DateTime.Now)
            .Select(record => record.Book)
            .ToList();
    }

    public List<Book> getAvailableBooks()
    {
        return items
            .Where(book => records.Any(record => record.Book.Id == book.Id && record.ReturnDate <= DateTime.Now) ||
                           !records.Exists(record => record.Book.Id == book.Id))
            .ToList();
    }

    public List<Book> getReturnedBooks()
    {
        return records
            .Where(record => record.ReturnDate <= DateTime.Now)
            .Select(record => record.Book)
            .ToList();
    }
}