namespace APDP;

public class Product(
    string name,
    double price,
    int quantity
) : ICalculable
{
    public string Name { get; } = name;
    public double Price { get; } = price;
    public int Quantity { get; } = quantity;

    public double CalculateTotal()
    {
        return Price * Quantity;
    }
}