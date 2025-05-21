public class Circle : Shape
{
    public Circle(double radius)
    {
        Radius = radius;
    }

    public double Radius { get; set; }

    // Implement the abstract method from the base class
    public override double Area()
    {
        return Math.PI * Radius * Radius;
    }
}