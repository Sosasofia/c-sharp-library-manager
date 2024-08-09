namespace LibraryManager;

public class User
{
    public int UserID { get; set; }
    public string? UserName { get; set; }
    public List<Book> Borrowed { get; set; } = new List<Book>();


    // arreglar
    public void Borrow(string? title)
    {
        Book book = Borrowed.Find(x => x.Title == title);

        if (book != null)
        {
            book.Available = true;
        }

        Borrowed.Add(new Book() { Title = title, Available = true });
    }

    // arreglar
    public void Return(string? title)
    {
        Book borrow = Borrowed.FindLast(book => book.Title == title && book.Available == true);

        if (borrow == null)
        {
            Console.WriteLine("Error");
        }
        else
        {
            borrow.Available = false;
        }
    }
}
