namespace APDP.Lab3;

public class Student(string firstName, string lastName, DateTime? dateOfBirth = null)
{
    public int StudentID { get; set; }
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public DateTime? DateOfBirth { get; set; } = dateOfBirth;
    public DateTime EnrollmentDate { get; set; } = DateTime.Now;
}