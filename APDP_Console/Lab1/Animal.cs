namespace APDP.Lab1;

public class Animal
{
    public Animal(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public string Name { get; set; }
    public int Age { get; set; }

    public virtual void MakeSound()
    {
        Console.WriteLine("Some generic animal sound");
    }
}

// Derived class (child class) that inherits from Animal