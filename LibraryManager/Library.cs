namespace LibraryManager;

public class Library
{
    public List<Book> books = new List<Book>();

    public List<User> users = new List<User>();

    public List<Lend> lendings = new List<Lend>();


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
        User.PrintUserData();
    }

    public void RegisterReturn(Book book, User user)
    {
        User? userInfo = users.FirstOrDefault(x => x.UserID == user.UserID);
        user.Return(book);
        User.PrintUserData();

    }

    // mejorar
    public void LoanHistory()
    {
        Console.WriteLine("Book title\t\t\tLend Date\t\t\tReturn Date");
        lendings!.ForEach(lend => Console.WriteLine($"{lend.BookTitle}\t\t{lend.LendDate.ToString()}\t\t{lend.ReturnDate.ToString()}"));
    }
};
