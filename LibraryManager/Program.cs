﻿using LibraryManager;

class Program
{
    private static Library? library;
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
                    Lend();
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
                case "a":

                    break;
                default:
                    Console.WriteLine(Constants.InvalidOption);
                    break;
            }

            Console.WriteLine("\n");
        }
    }

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

    // 1 - Add book
    private static void AddBook()
    {
        Response res = InitializeBook();

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

        if (!Int32.TryParse(isbn, out int isbnNumber))
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

        if (!Int32.TryParse(input, out int publishedYear))
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

    public static void Print(Book book)
    {
        Console.WriteLine("\n\tTitle: {0}", book.Title);
        Console.WriteLine("\tAuthor: {0}", book.Author);
        Console.WriteLine("\tISBN: {0}", book.ISBN);
        Console.WriteLine("\tPublished: {0}\n", book.Published);
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
            Book? result = Library.books.FirstOrDefault(book => book.Title!.Equals(filter));

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

        if (Library.users!.Exists(user => user.UserName == name))
        {
            return new Response()
            {
                Status = 400,
                Message = string.Format(Constants.AlredyExists, "Username")
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
        Response res = UserValidation();

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
    static Response ValidateID()
    {
        Console.Write(Constants.RequestUserId);
        var input = Console.ReadLine();

        if (!int.TryParse(input, out int id))
        {
            return new Response()
            {
                Message = string.Format(Constants.NumberInput, "ID"),
                Status = 400
            };
        }

        if (!Library.users.Exists(user => user.UserID == id))
        {
            return new Response()
            {
                Message = string.Format(Constants.UserNotFound, id),
                Status = 400,
            };
        }


        return new Response()
        {
            User = new User() { UserID = id },
            Status = 200,
        };
    }

    public static void UserById()
    {
        Response res = ValidateID();

        if (res.Status == 200)
        {
            User? result = Library.users.FirstOrDefault(user => user.UserID.Equals(res.User!.UserID));
            Console.WriteLine("\n\tUser found!");
            Console.WriteLine("\tUsername: {0}", result!.UserName);
        }
        else
        {
            Console.WriteLine(Constants.ErrorMessage, res.Message);
        }
    }


    // 6 - Borrow book
    private static Response InitializeLend()
    {
        Console.Write(Constants.RequestUserId);
        var id = Console.ReadLine();


        if (!int.TryParse(id, out int userId))
        {
            return new Response()
            {
                Message = string.Format(Constants.NumberInput),
                Status = 400
            };
        }

        if (!Library.users.Exists(u => u.UserID.Equals(userId)))
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

        if (!Library.books.Exists(b => b.Title == bookTitle))
        {
            return new Response() 
            { 
                Message = string.Format(Constants.BookNotFound, bookTitle), 
                Status = 400 
            };
        }

        Loan? lastRecord = Library.loans.FindLast(b => b.BookTitle == bookTitle && b.UserId == userId);

        if (lastRecord != null && lastRecord.ReturnDate == null)
        {
            return new Response()
            {
                Status = 400,
                Message = "Not available"
            };
        }

        return new Response
        {
            Loan = new Loan() { BookTitle = bookTitle, UserId = userId },
            Book = new Book() { Title = bookTitle },
            User = new User() { UserID = userId },
            Status = 200
        };
    }

    private static void Lend()
    {
        Response res = InitializeLend();

        if (res.Status == 200)
        {
            Library.loans.Add(new Loan()
            {
                BookTitle = res.Loan!.BookTitle,
                UserId = res.Loan.UserId,
                LendDate = DateTime.Now,
            });

            Library.RegisterLend(res.Book!, res.User!);
            Console.WriteLine("Correct loan");
        }
        else
        {
            Console.WriteLine(Constants.ErrorMessage, res.Message);
        }
    }

    // 7 - Return book
    static Response ValidateReturnInput()
    {
        Console.Write(Constants.RequestUserId);
        var input = Console.ReadLine();

        if (!int.TryParse(input, out int id))
        {
            Console.WriteLine(Constants.InvalidID);
            return new Response()
            {
                Status = 400,
                Message = Constants.InvalidID
            };
        }

        if (!Library.users.Exists(u => u.UserID.Equals(id)))
        {
            return new Response()
            {
                Status = 400,
                Message = string.Format(Constants.UserNotFound, id)
            };
        }


        Console.Write(Constants.RequestTitle);
        var title = Console.ReadLine();

        if (string.IsNullOrEmpty(title))
        {
            return new Response()
            {
                Status = 400,
                Message = string.Format(Constants.EmptyInput, "Book title")
            };
        }

        if (!Library.books.Exists(b => b.Title!.Equals(title)))
        {
            return new Response()
            {
                Status = 400,
                Message = string.Format(Constants.BookNotFound, title)
            };
        }

        Loan? lendBook = Library.loans.FindLast(l => l.UserId == id && l.BookTitle == title);

        if (lendBook == null)
        {
            return new Response()
            {
                Message = Constants.LoanNotFound,
                Status = 400
            };
        }

        return new Response()
        {
            Book = new Book { Title = title },
            User = new User { UserID = id },
            Status = 200,
        };
    }

    private static void Return()
    {
        Response res = ValidateReturnInput();

        if (res.Status == 200)
        {
            string? title = res.Book!.Title;
            int id = res.User!.UserID;

            Loan? book = Library.loans.FindLast(l => l.UserId == id && l.BookTitle.Equals(title) && l.ReturnDate == null);

            if (book != null)
            {
                book.ReturnDate = DateTime.Now;
                Library.RegisterReturn(res.Book, res.User);
                Console.WriteLine("Correct return");
            };
        }
        else
        {
            Console.WriteLine(Constants.ErrorMessage, res.Message);
        }
    }
}