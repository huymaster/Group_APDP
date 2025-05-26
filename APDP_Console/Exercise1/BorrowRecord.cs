namespace APDP.Exercise1;

public class BorrowRecord(
    User user,
    Book book,
    DateTime borrowDate,
    DateTime returnDate
)
{
    public User User { get; } = user;
    public Book Book { get; } = book;
    public DateTime BorrowDate { get; } = borrowDate;
    public DateTime ReturnDate { get; } = returnDate;
}