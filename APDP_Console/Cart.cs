namespace APDP;

public class Cart
{
    private readonly List<Product> _products = [];

    public void AddProduct()
    {
        Console.WriteLine("===============Product adding wizard===============");
        var name = "";
        double price = 0;
        var quantity = 0;

        Functions.ReadInput("Enter product name", s => s, ref name);
        Functions.ReadInput("Enter product price", double.Parse, ref price, p => p > 0);
        Functions.ReadInput("Enter product quantity", int.Parse, ref quantity, q => q > 0);

        _products.Add(new Product(name, price, quantity));

        Console.WriteLine("===================================================");
    }

    public void AddProduct(Product? product)
    {
        if (product is null) return;
        if (product.Quantity <= 0) return;
        if (_products.Contains(product)) return;
        _products.Add(product);
    }

    public void PrintCart()
    {
    }
}