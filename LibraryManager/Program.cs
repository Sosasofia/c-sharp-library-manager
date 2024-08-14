using LibraryManager;

static class Program
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
        Console.WriteLine();


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
                    LoanHistory();
                    break;
                case "9":
                    continueExecution = false;
                    break;
                default:
                    Console.WriteLine(Constants.Invalid, "option");
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

            string successMessage = string.Format(Constants.SuccessMessage, "book");

            DisplayMessage(successMessage);
        }
        else
        {
            string errorMessage = string.Format(Constants.ErrorMessage, res.Message);

            DisplayMessage(errorMessage);
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

        if (Library.Exists(title))
        {
            return new Response()
            {
                Status = 400,
                Message = string.Format(Constants.AlredyExists, "Book"),
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

        if (Library.BookISBNExists(isbnNumber))
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
                Message = string.Format(Constants.Invalid, "year"),
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
            string errorMessage = string.Format(Constants.NotFound, "Books");

            DisplayMessage(errorMessage);
            return;
        }

        if (string.IsNullOrEmpty(filter))
        {
            foreach (Book book in Library.books)
            {
                Print(book);
            }

            Console.ReadKey();
            Console.Clear();
        }
        else
        {
            var result = Library.FindBook(filter);

            if (result != null)
            {
                Print(result);

                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                DisplayMessage(Constants.BookNotFound);
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

            string successMessage = string.Format(Constants.SuccessMessage, "user");

            DisplayMessage(successMessage);
        }
        else
        {
            string errorMessage = string.Format(Constants.ErrorMessage, res.Message);

            DisplayMessage(errorMessage);
        }
    }

    // 5 - Find user by id
    public static void UserById()
    {
        Console.Write(Constants.RequestUserId);

        var input = Console.ReadLine();

        if (!int.TryParse(input, out int id))
        {
            DisplayMessage(Constants.NumberInput);

            return;
        }

        var ValidationResponse = UserValidation(id);

        if (ValidationResponse.Status == 200)
        {
            var user = Library.SearchByID(id);

            string message = string.Format($"\tUser found!\r\n\tUsername: {user!.UserName}");

            DisplayMessage(message);
        }
        else
        {
            string errorMessage = string.Format(Constants.ErrorMessage, ValidationResponse.Message);

            DisplayMessage(errorMessage);
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
                Message = Constants.NotAvailable,
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
            DisplayMessage(string.Format(Constants.NumberInput));

            return;
        }

        var res = InitializeLoan(userId);

        if (res.Status == 200)
        {
            Library.AddLoan(res.Loan!);

            string successMessage = string.Format(Constants.SuccessMessage, "loan");

            DisplayMessage(successMessage);
        }
        else
        {
            string errorMessage = string.Format(Constants.ErrorMessage, res.Message);

            DisplayMessage(errorMessage);
        }
    }

    // 7 - Return book
    static void Return()
    {
        Console.Write(Constants.RequestUserId);

        var input = Console.ReadLine();


        if (!int.TryParse(input, out int id))
        {
            DisplayMessage(string.Format(Constants.Invalid, "ID"));

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
            DisplayMessage(string.Format(Constants.Invalid, "ID or book title"));

            return;
        }

        var LoanValidation = ReturnValidation(title, id);

        if (LoanValidation.Status == 200)
        {
            var loan = Library.loans.FindLast(l => l.UserId == id && l.BookTitle.Equals(title) && l.ReturnDate == null);

            if (loan != null)
            {
                loan.ReturnDate = DateTime.Now;

                DisplayMessage(Constants.CorrectReturn);
            };
        }
        else
        {
            string errorMessage = string.Format(Constants.ErrorMessage, LoanValidation.Message);

            DisplayMessage(errorMessage);
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
                Message = Constants.LoanNotFound,
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
        Console.WriteLine(Constants.MenuOptions);
        Console.WriteLine();
    }

    public static void Print(Book book)
    {
        Console.WriteLine();
        Console.WriteLine(Constants.Separator);
        Console.WriteLine(Constants.BookInfo, book.Title, book.Author, book.ISBN, book.Published);
        Console.WriteLine(Constants.Separator);
    }

    static void DisplayMessage(string message)
    {
        Console.WriteLine();
        Console.WriteLine(Constants.Separator);
        Console.WriteLine(message);
        Console.WriteLine(Constants.Separator);
        Console.ReadKey();
        Console.Clear();
    }

    public static void LoanHistory()
    {
        Console.WriteLine(Constants.Separator);
        Console.WriteLine("\tLoan history");
        Console.WriteLine("\t----------------------------------------------------------------------------------------");
        Console.WriteLine("\tBook title\tUser\tLend Date\t\t\tReturn Date");
        Console.WriteLine("\t----------------------------------------------------------------------------------------");


        foreach (Loan loan in Library.loans)
        {
            Console.WriteLine($"\t{loan.BookTitle}\t\t{loan.UserId}\t{loan.LendDate.ToString()}\t{loan.ReturnDate.ToString()}");
        }


        Console.ReadKey();
        Console.Clear();
    }
}