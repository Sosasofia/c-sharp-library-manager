namespace LibraryManager;
public static class Constants
{
    public const string Title = "\tLibrary Manager\r";
    public const string Separator = "\t**************************************\n";

    public const string MenuMessage = "\tMenu: \n";
    public const string AddBook = "\t1 - Add book";
    public const string SearchBookByTitle = "\t2 - Search book by title";
    public const string ListAllBooks = "\t3 - List books";
    public const string AddUser = "\t4 - Create user";
    public const string SearchUserById = "\t5 - Search user by ID";
    public const string LendBook = "\t6 - Lend book";
    public const string ReturnBook = "\t7 - Return book";
    public const string LoanHistory = "\t8 - Loan history";
    public const string FinishExecution = "\t9 - Finish execution";

    public const string InvalidOption = "Invalid option!";
    public const string InvalidID = "Invalid ID";
    public const string InvalidYear = "Invalid year range";
    public const string Error = "Error";
    public const string ErrorMessage = "\tError: {0}";
    public const string NotFound = "Not Found";
    public const string NoBooks = "There are no registered books";
    public const string EmptyInput = "{0} input field can not be empty";
    public const string NumberInput = "Input must be a number";
    public const string BookNotFound = "Book with title *{0}* not found!";
    public const string UserNotFound = "User with ID *{0}* not found!";
    public const string LoanNotFound = "Loan not found!";
    public const string BookAlreadyExists = "Book title already exist";
    public const string UserAlreadyExists = "Username already exists";
    public const string AlredyExists = "{0} already exists";

    public const string RequestTitle = "\tEnter book title: ";
    public const string RequestAuthor = "\tEnter book author: ";
    public const string RequestISBN = "\tEnter book ISBN: ";
    public const string RequestPublicationYear = "\tEnter book publication date: ";
    public const string RequestUserId = "\tEnter user id: ";
    public const string RequestUserName = "\tEnter username: ";

    public const string UserCreated = "\tSuccesfull user creation!";
    public const string BookCreated = "\tSuccesfull book creation!";
}
