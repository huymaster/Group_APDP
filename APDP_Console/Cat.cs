namespace APDP;

public class Cat : IPet
{
    public string Name => "Cat";
    public int Age { get; set; }

    public string MakeSound()
    {
        return "meow";
    }
}