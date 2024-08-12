namespace LibraryManager;

public class Library
{
    public List<Book> books = new List<Book>();

    public List<User> users = new List<User>();

    public List<Loan> loans = new List<Loan>();


    public void AddBook(Book book)
    {
        books.Add(book);
    }

    public void AddUser(User user)
    {
        int id = users.Count();

        users!.Add(new User()
        {
            UserName = user.UserName,
            UserID = id + 1,
        });
    }

    public void RegisterLend(Book book, User user)
    {
        User? foundUser = users.FirstOrDefault(u => u.UserID == user.UserID);

        if (foundUser != null)
        {
            foundUser.Borrow(book);
        }
    }

    public void RegisterReturn(Book book, User user)
    {
        User? userInfo = users.FirstOrDefault(u => u.UserID == user.UserID);
        user.Return(book);
    }

    // mejorar
    public void LoanHistory()
    {
        Console.WriteLine("Book title\t\t\tLend Date\t\t\tReturn Date");
        loans!.ForEach(lend => Console.WriteLine($"{lend.BookTitle}\t\t{lend.LendDate.ToString()}\t\t{lend.ReturnDate.ToString()}"));
    }
};
