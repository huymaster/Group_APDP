namespace APDP;

public static class Program
{
    public static void Main(string[] args)
    {
        Circle circle = new Circle(5);
        Rectangle rectangle = new Rectangle(4, 6);
        // Call the Area method on the instances
        Console.WriteLine($"Circle Area: {circle.Area()}");
        Console.WriteLine($"Rectangle Area: {rectangle.Area()}");
    }
}