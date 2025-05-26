using APDP.Exercise1;

namespace APDP;

public static class Program
{
    public static void Main(string[] args)
    {
        var libraryManager = LibraryManager.getInstance();

        var bookA = new Book("The Great Gatsby", "F. Scott Fitzgerald");
        var bookB = new Book("To Kill a Mockingbird", "Harper Lee");
        var bookC = new Book("Nineteen Eighty-Four", "George Orwell");
        var bookD = new Book("In Search of Lost Time", "Marcel Proust");

        var userA = new User("A");
        var userB = new User("B");
        var userC = new User("C");

        libraryManager.addBook(bookA);
        libraryManager.addBook(bookB);
        libraryManager.addBook(bookC);
        libraryManager.addBook(bookD);

        libraryManager.borrowBook(userA, bookA, DateTime.Now.AddDays(7));
        libraryManager.borrowBook(userB, bookB, DateTime.Now.AddDays(1));
        libraryManager.borrowBook(userC, bookC, DateTime.Now.AddDays(-9));

        Console.WriteLine("\nAll books:");
        PrintList(libraryManager.getAllBooks());

        Console.WriteLine("\nBorrowed books:");
        PrintList(libraryManager.getBorrowedBooks());

        Console.WriteLine("\nAvailable books:");
        PrintList(libraryManager.getAvailableBooks());

        Console.WriteLine("\nReturned books:");
        PrintList(libraryManager.getReturnedBooks());
    }

    private static void PrintList<T>(List<T> list)
    {
        foreach (var item in list) Console.WriteLine(item);
    }
}