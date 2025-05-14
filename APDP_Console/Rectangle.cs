namespace APDP;

public class Rectangle
{
    public Rectangle(int s1, int s2)
    {
        if (s1 <= 0 || s2 <= 0)
            throw new ArgumentException("Illegal width or height");

        Width = int.Min(s1, s2);
        Height = int.Max(s1, s2);
    }

    public int Width { get; set; }
    public int Height { get; set; }

    public int Area => Width * Height;
    public int Perimeter => 2 * (Width + Height);

    public override string ToString()
    {
        return "Rectangle[" + Width + ", " + Height + "]";
    }
}