namespace APDP.Lab3;

public class Student
{
    private static int _studentID;

    public Student(string firstName, string lastName, DateTime? dateOfBirth = null)
    {
        _studentID += 1;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }

    public int StudentID { get; private set; } = _studentID;
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime? DateOfBirth { get; private set; }
    public DateTime EnrollmentDate { get; private set; } = DateTime.Now;
}