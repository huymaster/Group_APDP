namespace APDP.Lab1;

public class Person
{
    private int age;

    private string name;

    public string Name
    {
        get => name;
        set
        {
            if (!string.IsNullOrEmpty(value))
                name = value;
        }
    }

    public int Age
    {
        get => age;
        set
        {
            if (value >= 0)
                age = value;
        }
    }

    public void Introduce()
    {
        Console.WriteLine($"Hello, my name is {Name}, and I am {Age} years old.");
    }
}