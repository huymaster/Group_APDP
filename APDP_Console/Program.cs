namespace APDP;

public static class Program
{
    public static void Main(string[] args)
    {
        // Create an instance of the Dog class
        Dog myDog = new Dog("Buddy", 3, "Golden Retriever");

        // Access properties from the base class
        Console.WriteLine($"Name: {myDog.Name}");
        Console.WriteLine($"Age: {myDog.Age}");

        // Access properties specific to the Dog class
        Console.WriteLine($"Breed: {myDog.Breed}");

        // Call methods inherited from the base class
        myDog.MakeSound();

        // Call a method specific to the Dog class
        myDog.Fetch();

    }
}