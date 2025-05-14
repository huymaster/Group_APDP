namespace APDP;

public interface IPet
{
    public string Name { get; }
    public int Age { get; set; }
    public string MakeSound();
}