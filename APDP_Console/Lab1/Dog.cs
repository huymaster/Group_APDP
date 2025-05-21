namespace APDP.Lab1;

public class Dog : Animal
{
    public Dog(string name, int age, string breed) : base(name, age)
    {
        Breed = breed;
    }

    public string Breed { get; set; }

    public override void MakeSound()
    {
        Console.WriteLine("Bark, bark!");
    }

    public void Fetch()
    {
        Console.WriteLine("The dog is fetching a ball.");
    }
}