namespace APDP;

public class Dog : IPet
{
    public string Name => "Dog";
    public int Age { get; set; }

    public string MakeSound()
    {
        return "woof";
    }
}