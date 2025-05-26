namespace APDP.Exercise1;

public class User(string name)
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public string Name { get; } = name;
}