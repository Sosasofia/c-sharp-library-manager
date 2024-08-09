namespace LibraryManager;

public class Library
{
    public List<Book> books = new List<Book>();

    public List<User> users = new List<User>();

    public List<Lend> lendings = new List<Lend>();


    // Book implementations
    public void AddBook(Book book)
    {
        books.Add(book);
    }

    public void AddBook(string t, string author, int ISBN, int year)
    {
        Book book = new Book()
        {
            Title = t,
            Author = author,
            ISBN = ISBN,
            Published = year,
        };

        books.Add(book);
    }

    public Book SearchByTitle(string title)
    {
        Book? result = new Book();

        result = books.FirstOrDefault(book => book.Title == title);

        return result!;
    }

    public List<Book> Books()
    {
        return books;
    }


    // User implementations
    public void AddUser(string name)
    {
        int id = users.Count();
        
        users!.Add(new User()
        {
            UserName = name,
            UserID = id + 1,
        });
    }

    public User SearchUserByID(int id)
    {
        return users!.FirstOrDefault(user => user.UserID == id)!; 
    }

    public void RegisterLend(Book book, User user)
    {
        User foundUser = users.FirstOrDefault(x => x.UserID == user.UserID);
        foundUser.Borrow(book.Title);
    }

    public void RegisterReturn(Book book, User user)
    {
        User userInfo = users.FirstOrDefault(x => x.UserID == user.UserID);
        user.Return(book.Title);
    }

    // mejorar
    public void LoanHistory()
    {
        Console.WriteLine("Book title\t\t\tLend Date\t\t\tReturn Date");
        lendings!.ForEach(lend => Console.WriteLine($"{lend.BookTitle}\t\t{lend.LendDate.ToString()}\t\t{lend.ReturnDate.ToString()}"));
    }
};
