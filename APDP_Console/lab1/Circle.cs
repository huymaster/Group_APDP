public class Circle : Shape
{
    public double Radius { get; set; }
    public Circle(double radius)
    {
        Radius = radius;
    }
    // Implement the abstract method from the base class
    public override double Area()
    {
        return Math.PI * Radius * Radius;}
}
    
