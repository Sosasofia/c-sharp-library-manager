using LibraryManager;

class Program
{
    private static Library library; 
    public static Library Library
    {
        get
        {
            if (library == null)
            {
                library = new Library();
            }
            return library;
        }
    }

    static void Main()
    {
        bool continueExecution = true;


        Console.WriteLine(Constants.Title);
        Console.WriteLine(Constants.Separator);


        while (continueExecution)
        {
            PrintMenu();
            string? op = Console.ReadLine();

            switch (op)
            {
                case "1":
                    AddBook();
                    break;
                case "2":
                    SearchBookByTitle();
                    break;
                case "3":
                    PrintBooks(string.Empty);
                    break;
                case "4":
                    CreateUser();
                    break;
                case "5":
                    UserById();
                    break;
                case "6":
                    Loan();
                    break;
                case "7":
                    Return();
                    break;
                case "8":
                    Console.WriteLine("Loan history");
                    Library.LoanHistory();
                    break;
                case "9":
                    continueExecution = false;
                    break;
                default:
                    Console.WriteLine(Constants.InvalidOption);
                    break;
            }

            Console.WriteLine("\n");
        }
    }

    // 1 - Add book
    static void AddBook()
    {
        var res = InitializeBook();

        if (res.Status == 200)
        {
            Library.AddBook(res.Book!);
        }
        else
        {
            Console.WriteLine(Constants.ErrorMessage, res.Message);
        }
    }

    static Response InitializeBook()
    {
        Console.Write(Constants.RequestTitle);

        var title = Console.ReadLine();

        if (string.IsNullOrEmpty(title))
        {
            return new Response()
            {
                Message = Constants.EmptyInput,
                Status = 400
            };
        }

        Console.Write(Constants.RequestAuthor);

        var author = Console.ReadLine();

        if (string.IsNullOrEmpty(author))
        {
            return new Response()
            {
                Message = Constants.EmptyInput,
                Status = 400
            };
        };

        Console.Write(Constants.RequestISBN);

        var isbn = Console.ReadLine();

        if (string.IsNullOrEmpty(isbn))
        {
            return new Response()
            {
                Message = Constants.EmptyInput,
                Status = 400
            };
        }

        if (!int.TryParse(isbn, out int isbnNumber))
        {
            return new Response()
            {
                Message = Constants.NumberInput,
                Status = 400
            };
        }

        if (Library.books!.Any(book => book.ISBN == isbnNumber))
        {
            return new Response()
            {
                Message = string.Format(Constants.AlredyExists, "ISBN number"),
                Status = 400
            };
        }

        Console.Write(Constants.RequestPublicationYear);

        var input = Console.ReadLine();

        if (!int.TryParse(input, out int publishedYear))
        {

            return new Response()
            {
                Message = Constants.NumberInput,
                Status = 400
            };
        }

        if (publishedYear < 0 || publishedYear > DateTime.Now.Year)
        {
            return new Response()
            {
                Message = Constants.InvalidYear,
                Status = 400
            };
        }

        return new Response()
        {
            Book = new Book() { Title = title, Author = author, ISBN = isbnNumber, Published = publishedYear },
            Status = 200,
        };
    }

    // 2- Search book by title
    private static void SearchBookByTitle()
    {
        Console.Write(Constants.RequestTitle);

        var title = Console.ReadLine();

        PrintBooks(title);
    }

    // 3 - List all books
    static void PrintBooks(string? filter)
    {
        if (Library.books.Count < 1)
        {
            Console.WriteLine(Constants.NoBooks);
            return;
        }

        if (string.IsNullOrEmpty(filter))
        {
            foreach (Book book in Library.books)
            {
                Print(book);
            }
        }
        else
        {
            var result = Library.FindBook(filter);

            if (result != null)
            {
                Print(result);
            }
            else
            {
                Console.WriteLine(Constants.BookNotFound, filter);
            }
        }
    }

    // 4- Create user
    static Response UserValidation()
    {
        Console.Write(Constants.RequestUserName);

        var name = Console.ReadLine();

        if (string.IsNullOrEmpty(name))
        {
            return new Response()
            {
                Status = 400,
                Message = string.Format(Constants.EmptyInput, "Username")
            };
        }

        if (Library.Exists(name))
        {
            return new Response()
            {
                Message = string.Format(Constants.AlredyExists, "Username"),
                Status = 400,
            };
        }

        return new Response
        {
            User = new User() { UserName = name },
            Status = 200,
        };
    }

    public static void CreateUser()
    {
        var res = UserValidation();

        if (res.Status == 200)
        {
            Library.AddUser(res.User!);
            Console.WriteLine(Constants.UserCreated);
        }
        else
        {
            Console.WriteLine(Constants.ErrorMessage, res.Message);
        }
    }

    // 5 - Find user by id
    public static void UserById()
    {
        Console.Write(Constants.RequestUserId);

        var input = Console.ReadLine();

        if(!int.TryParse(input, out int id))
        {
            Console.WriteLine($"\tError: input must be number");

            return;
        }

        var ValidationResponse = UserValidation(id);

        if (ValidationResponse.Status == 200)
        {
            var user = Library.SearchByID(id);

            Console.WriteLine("\n\tUser found!");
            Console.WriteLine("\tUsername: {0}", user!.UserID);
        }
        else
        {
            Console.WriteLine(ValidationResponse.Message);
        }
    }

    static Response UserValidation(int id)
    {
        if (!library.UserIdExists(id))
        {
            return new Response()
            {
                Message = string.Format(Constants.UserNotFound, id),
                Status = 400
            };
        }

        return new Response()
        {
            Status = 200
        };
    }

    // 6 - Borrow book
    private static Response InitializeLoan(int userId)
    {
        // Valido si existe usuario
        if (!Library.UserIdExists(userId))
        {
            return new Response()
            {
                Message = string.Format(Constants.UserNotFound, userId),
                Status = 400
            };
        }

        Console.Write(Constants.RequestTitle);

        var bookTitle = Console.ReadLine();

        if (string.IsNullOrEmpty(bookTitle))
        {
            return new Response()
            {
                Message = string.Format(Constants.EmptyInput, "Book title"),
                Status = 400
            };
        }

        if (!Library.BookExists(bookTitle))
        {
            return new Response()
            {
                Message = string.Format(Constants.BookNotFound, bookTitle),
                Status = 400
            };
        }

        // Busco si hay prestamos activo de ese libro
        if (Library.ActiveLoan(bookTitle))
        {
            return new Response()
            {
                Status = 400,
                Message = "Not available"
            };
        }

        return new Response
        {
            Loan = new Loan() { BookTitle = bookTitle, UserId = userId, LendDate = DateTime.Now },
            Status = 200
        };
    }

    private static void Loan()
    {
        Console.Write(Constants.RequestUserId);

        var id = Console.ReadLine();

        if (!int.TryParse(id, out int userId))
        {
            Console.WriteLine(string.Format(Constants.NumberInput));

            return;
        }

        var res = InitializeLoan(userId);

        if (res.Status == 200)
        {
            Library.AddLoan(res.Loan!);

            Console.WriteLine("Correct loan");
        }
        else
        {
            Console.WriteLine(Constants.ErrorMessage, res.Message);
        }
    }

    // 7 - Return book
    static void Return()
    {
        Console.Write(Constants.RequestUserId);

        var input = Console.ReadLine();


        if (!int.TryParse(input, out int id))
        {
            Console.WriteLine(Constants.NumberInput); 

            return;
        }

        var UserResponse = UserValidation(id);

        Console.WriteLine(Constants.RequestTitle);

        var title = Console.ReadLine();

        if (string.IsNullOrEmpty(title))
        {
            Console.WriteLine(string.Format(Constants.EmptyInput, "Book title"));

            return;
        }

        var BookResponse = BookValidation(title);

        if (UserResponse.Status == 400 || BookResponse.Status == 400)
        {
            Console.WriteLine("Error with the id or the book title");

            return;
        }

        var LoanValidation = ReturnValidation(title, id);

        if (LoanValidation.Status == 200)
        {
            var loan = Library.loans.FindLast(l => l.UserId == id && l.BookTitle.Equals(title) && l.ReturnDate == null);

            if (loan != null)
            {
                loan.ReturnDate = DateTime.Now;

                Console.WriteLine("Correct return");
            };
        } 
        else
        {
            Console.WriteLine("\tError");
        }
    }

    // Valida si existe prestamo registrado
    static Response ReturnValidation(string title, int id)
    {
        bool loanExists = Library.loans.Any(loan => loan.BookTitle == title && loan.UserId == id && loan.ReturnDate == null);

        if (!loanExists)
        {
            return new Response()
            {
                Message = "Loan does not exist",
                Status = 400
            };
        }

        return new Response()
        {
            Status = 200,
        };
    }

    // Valida libro
    static Response BookValidation(string title)
    {
        
        if (!Library.books.Exists(b => b.Title!.Equals(title)))
        {
            return new Response()
            {
                Status = 400,
                Message = string.Format(Constants.BookNotFound, title)
            };
        }

        return new Response()
        {
            Status = 200
        };
    }

    // Print methods
    private static void PrintMenu()
    {
        Console.WriteLine(Constants.MenuMessage);
        Console.WriteLine(Constants.AddBook);
        Console.WriteLine(Constants.SearchBookByTitle);
        Console.WriteLine(Constants.ListAllBooks);
        Console.WriteLine(Constants.AddUser);
        Console.WriteLine(Constants.SearchUserById);
        Console.WriteLine(Constants.LendBook);
        Console.WriteLine(Constants.ReturnBook);
        Console.WriteLine(Constants.LoanHistory);
        Console.WriteLine(Constants.FinishExecution);
        Console.WriteLine();
    }

    public static void Print(Book book)
    {
        Console.WriteLine("\n\tTitle: {0}", book.Title);
        Console.WriteLine("\tAuthor: {0}", book.Author);
        Console.WriteLine("\tISBN: {0}", book.ISBN);
        Console.WriteLine("\tPublished: {0}\n", book.Published);
    }
}