using APDP.Lab1;

namespace APDP;

public static class Program
{
    public static void Main(string[] args)
    {
        var myDog = new Dog("Buddy", 3, "Golden Retriever");

        Console.WriteLine($"Name: {myDog.Name}");
        Console.WriteLine($"Age: {myDog.Age}");

        Console.WriteLine($"Breed: {myDog.Breed}");

        myDog.MakeSound();

        myDog.Fetch();
    }
}