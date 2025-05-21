public class Person
{
    // Private fields are used to store the data
    private string name;
    private int age;

    // Public properties provide controlled access to the private fields
    public string Name
    {
        get { return name; }
        set
        {
            if (!string.IsNullOrEmpty(value))
                name = value;
        }
    }

    public int Age
    {
        get { return age; }
        set
        {
            if (value >= 0)
                age = value;
        }
    }

    // Public method to introduce the person
    public void Introduce()
    {
        Console.WriteLine($"Hello, my name is {Name}, and I am {Age} years old.");
    }
}
