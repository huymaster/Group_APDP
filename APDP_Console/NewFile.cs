public class Publication
{
    public Publication(string title, decimal price, int copies)
    {
        Title = title;
        Price = price;
        Copies = copies;
    }

    public string Title { get; set; }
    public decimal Price { get; set; }
    public int Copies { get; set; }

    public void SellCopy()
    {
        if (Copies > 0)
        {
            Copies--;
            Console.WriteLine($"Sold a copy of '{Title}'. Remaining copies: {Copies}");
        }
        else
        {
            Console.WriteLine($"'{Title}' is out of stock.");
        }
    }
}

public class Book : Publication
{
    public Book(string title, decimal price, int copies, string author)
        : base(title, price, copies)
    {
        Author = author;
    }

    public string Author { get; set; }

    public void OrderCopies(int quantity)
    {
        Copies += quantity;
        Console.WriteLine($"Ordered {quantity} copies of '{Title}'. Total copies: {Copies}");
    }
}

public class Magazine : Publication
{
    public Magazine(string title, decimal price, int copies, int orderQty, int currentIssue)
        : base(title, price, copies)
    {
        OrderQty = orderQty;
        CurrentIssue = currentIssue;
    }

    public int OrderQty { get; set; }
    public int CurrentIssue { get; set; }

    public void AdjustQty(int adjustment)
    {
        Copies += adjustment;
        Console.WriteLine($"Adjusted quantity of '{Title}' by {adjustment}. Total copies: {Copies}");
    }

    public void RecNewIssue()
    {
        CurrentIssue++;
        Console.WriteLine($"Recorded a new issue of '{Title}'. Current issue: {CurrentIssue}");
    }
}