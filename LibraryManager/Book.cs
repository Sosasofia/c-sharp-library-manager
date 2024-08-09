namespace LibraryManager;
public class Book
{
    public string? Title { get; set; } 
    public string? Author { get; set; }
    public int ISBN { get; set; }
    public int Published { get; set; }
    public bool Available { get; set; } = false;
};
