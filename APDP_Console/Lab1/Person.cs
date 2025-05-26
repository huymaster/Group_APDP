namespace APDP.Lab1;

public delegate void ComparisonDelegate(Person p1, Person p2);

public class Person(string name, int age)
{
    private int _age = age;
    private string _name = name;

    public string Name
    {
        get => _name;
        set
        {
            if (!string.IsNullOrEmpty(value))
                _name = value;
        }
    }

    public int Age
    {
        get => _age;
        set
        {
            if (value >= 0)
                _age = value;
        }
    }

    public void Introduce()
    {
        Console.WriteLine($"Hello, my name is {Name}, and I am {Age} years old.");
    }

    public static int CompareName(Person p1, Person p2)
    {
        return string.Compare(p1.Name, p2.Name, StringComparison.Ordinal);
    }

    public static int CompareAge(Person p1, Person p2)
    {
        return p1.Age.CompareTo(p2.Age);
    }
}