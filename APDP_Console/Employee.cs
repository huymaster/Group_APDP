namespace APDP;

public abstract record Employee(
    string Id,
    string Name,
    double Salary
)
{
    public string Id { get; init; } = Id;
    public string Name { get; init; } = Name;
    public double Salary { get; init; } = Salary;
    public abstract double Bonus { get; }

    public double TotalSalary => Salary + Salary * Bonus;

    public override string ToString()
    {
        return $"Employee[id={Id}, name={Name}, salary={Salary} + {Bonus * Salary} = {TotalSalary}]";
    }
}

public record FullTimeEmployee(string Id, string Name, double Salary) : Employee(Id, Name, Salary)
{
    public override double Bonus => 0.1;
}

public record PartTimeEmployee(string Id, string Name, double Salary) : Employee(Id, Name, Salary)
{
    public override double Bonus => 0.05;
}