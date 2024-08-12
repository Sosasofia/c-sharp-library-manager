namespace LibraryManager;
public class Response
{
    public int Status { get; set; }
    public string? Message { get; set; }
    public Book? Book { get; set; }
    public User? User { get; set; }
    public Loan? Loan { get; set; }
}
