namespace LibraryManager;

public class User
{
    public int UserID { get; set; }
    public string? UserName { get; set; }

    private static List<Book>? borrowed = new List<Book>();


    public void Borrow(Book book)
    {
        var borrowedBook = borrowed!.FirstOrDefault(b => b.Title == book.Title);

        if (borrowedBook != null && !borrowedBook.Available)
        {
            borrowedBook.Available = true;
        }
        else
        {
            borrowed!.Add(new Book() { Available = true, Title = book.Title });
        }
    }

    public void Return(Book book)
    {
        var borrowedBook = borrowed!.FirstOrDefault(b => b.Title!.Equals(book.Title));

        if (borrowedBook != null)
        {
            borrowedBook.Available = false;
        }
    }
}
