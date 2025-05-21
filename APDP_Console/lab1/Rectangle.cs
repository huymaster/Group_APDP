public class Rectangle : Shape
{
    public double Length { get; set; }
    public double Width { get; set; }
    public Rectangle(double length, double width)
    {
        Length = length;
        Width = width;
    }
    // Implement the abstract method from the base class
    public override double Area()
    {
        return Length * Width;
    }
}

