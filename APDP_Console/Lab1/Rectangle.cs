namespace APDP.Lab1;

public class Rectangle : Shape
{
    public Rectangle(double length, double width)
    {
        Length = length;
        Width = width;
    }

    public double Length { get; set; }

    public double Width { get; set; }

    public override double Area()
    {
        return Length * Width;
    }
}