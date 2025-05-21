using APDP.Lab2;

namespace APDP;

public static class Program
{
    public static void Main(string[] args)
    {
        var catalog = new LibraryCatalog();

        var book1 = new Book("The Great Gatsby", "F. Scott Fitzgerald", new DateTime(1925, 4, 10), true, "Fiction");
        var book2 = new Book("To Kill a Mockingbird", "Harper Lee", new DateTime(1960, 7, 11), false, "Fiction");
        var book3 = new Book("1984", "George Orwell", new DateTime(1949, 6, 8), true, "Dystopian Fiction");

        var dvd1 = new DVD("The Matrix", "The Wachowskis", new DateTime(1999, 3, 31), true, 180);
        var dvd2 = new DVD("Avatar", "James Cameron", new DateTime(2009, 12, 17), false, 162);

        catalog.AddItem(book1);
        catalog.AddItem(book2);
        catalog.AddItem(book3);
        catalog.AddItem(dvd1);
        catalog.AddItem(dvd2);

        catalog.PrintAllItems();
    }
}