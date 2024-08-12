namespace LibraryManager;
public class Loan
{
    //public Guid UserId { get; set; }
    public int UserId { get; set; }
    public string BookTitle { get; set; } = string.Empty; 
    public DateTime? LendDate { get; set; }
    public DateTime? ReturnDate { get; set; } = null;
};
