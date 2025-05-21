    class Dog : Animal
{
    public string Breed { get; set; }

    public Dog(string name, int age, string breed) : base(name, age)
    {
        Breed = breed;
    }

    // Override the MakeSound method with a specific implementation
    public override void MakeSound()
    {
        Console.WriteLine("Bark, bark!");
    }

    public void Fetch()
    {
        Console.WriteLine("The dog is fetching a ball.");
    }
}
