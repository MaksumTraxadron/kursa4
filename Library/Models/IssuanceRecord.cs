namespace SchoolLibrary.Models;

public class IssuanceRecord
{
    public string FullName { get; set; } = string.Empty;
    public int TicketNumber { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }

    public bool IsOverdue => DateTime.Now > DueDate;
}