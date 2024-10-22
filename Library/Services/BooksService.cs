using Library.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Library.Requests;
using Library.Model;
using Library.DataBaseContext;
using static Library.Requests.Find;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Linq;

namespace Library.Services
{
    public class BooksService : IBooksService
    {
        private readonly LibraryDB _context;
        public BooksService(LibraryDB context)
        {
            _context = context;
        }
        public async Task<IActionResult> CreateNewBook([FromQuery] CreateNewBook newBook)
        {
            try
            {
                var books = new Books()
                {
                    Title = newBook.Title,
                    Author = newBook.Author,
                    Id_genre = newBook.Id_genre,
                    Copies = newBook.Copies,
                    Year_public = newBook.Year_public,
                    Description = newBook.Description,
                };
                await _context.Books.AddAsync(books);
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> DeleteBook(int id_book)
        {
            if (id_book <= 0)
            {
                return new BadRequestObjectResult("Некорректный идентификатор книги.");
            }

            try
            {
                var book = await _context.Books.FindAsync(id_book);
                if (book == null)
                {
                    return new NotFoundObjectResult("Книга с указанным идентификатором не найдена.");
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> FindById(int id_book)
        {
            if (id_book <= 0)
            {
                return new BadRequestObjectResult("Некорректный идентификатор книги.");
            }
            try
            {
                var book = await _context.Books.FindAsync(id_book);
                if (book == null)
                {
                    return new NotFoundObjectResult("Книга с указанным идентификатором не найдена.");
                }

                var bookDto = new GetAllBooksId
                {
                    Id_Books = book.Id_book,
                    Title = book.Title,
                    Author = book.Author,
                    Id_genre = book.Id_genre,
                    Copies = book.Copies,
                    Year_public = book.Year_public,
                    Description = book.Description
                };
                return new OkObjectResult(bookDto);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> FindByIdGenre(int id_genre)
        {
            if (id_genre <= 0)
            {
                return new BadRequestObjectResult("Некорректный идентификатор жанра.");
            }
            try
            {
                var books = await _context.Books
                    .Where(b => b.Id_genre == id_genre)
                    .ToListAsync();

                if (books == null || !books.Any())
                {
                    return new NotFoundObjectResult("Книги с указанным жанром не найдены.");
                }

                var booksDto = books.Select(b => new GetAllBooksId
                {
                    Id_Books = b.Id_book,
                    Title = b.Title,
                    Author = b.Author,
                    Id_genre = b.Id_genre,
                    Copies = b.Copies,
                    Year_public = b.Year_public,
                    Description = b.Description
                });

                return new OkObjectResult(booksDto);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> FindCopies(string Title)
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                return new BadRequestObjectResult("Название книги обязательно для поиска.");
            }

            try
            {
                var books = await _context.Books
                    .Where(b => b.Title.Contains(Title))
                    .Select(b => new BookAvailableCopies
                    {
                        Id_Books = b.Id_book,
                        Title = b.Title,
                        AvailableCopies = b.Copies
                    })
                    .ToListAsync();

                if (books == null || books.Count == 0)
                {
                    return new NotFoundObjectResult("Книги с указанным названием не найдены.");
                }

                return new OkObjectResult(books);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> GetBooks(int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return new BadRequestObjectResult("Параметры страницы и размера страницы должны быть положительными.");
            }

            try
            {
                var totalBooks = await _context.Books.CountAsync();
                var totalPages = (int)Math.Ceiling(totalBooks / (double)pageSize);

                var books = await _context.Books
                    .Include(b => b.Genres)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var booksDto = books.Select(b => new GetAllBooksName
                {
                    Id_Books = b.Id_book,
                    Title = b.Title,
                    Author = b.Author,
                    Id_genre = b.Genres.Id_genre,
                    Copies = b.Copies,
                    Year_public = b.Year_public,
                    Description = b.Description
                });

                var result = new
                {
                    CurrentPage = page,
                    TotalPages = totalPages,
                    TotalBooks = totalBooks,
                    Books = booksDto
                };

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> SearchBooksByTitle(string Title)
        {
            if (Title == null)
            {
                return new BadRequestObjectResult("Строка обязательна для поиска.");
            }

            try
            {
                var books = await _context.Books.Include(b => b.Genres)
                .Where(b => b.Title.Contains(Title))
                .ToListAsync();
                if (books == null || !books.Any())
                {
                    return new NotFoundObjectResult("Книги с указанным запросом не найдены.");
                }

                var booksDto = books.Select(b => new GetAllBooksName
                {
                    Id_Books = b.Id_book,
                    Title = b.Title,
                    Author = b.Author,
                    Id_genre = b.Id_genre,
                    Copies = b.Copies,
                    Year_public = b.Year_public,
                    Description = b.Description
                });

                return new OkObjectResult(booksDto);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> SearchBooksByAuthor(string Author)
        {
            if (Author == null)
            {
                return new BadRequestObjectResult("Строка обязательна для поиска.");
            }

            try
            {
                var books = await _context.Books.Include(b => b.Genres)
                .Where(b => b.Author.Contains(Author))
                .ToListAsync();
                if (books == null || !books.Any())
                {
                    return new NotFoundObjectResult("Книги с указанным запросом не найдены.");
                }

                var booksDto = books.Select(b => new GetAllBooksName
                {
                    Id_Books = b.Id_book,
                    Title = b.Title,
                    Author = b.Author,
                    Id_genre = b.Id_genre,
                    Copies = b.Copies,
                    Year_public = b.Year_public,
                    Description = b.Description
                });

                return new OkObjectResult(booksDto);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> SearchBooksByGenre(int id_genre)
        {
            if (id_genre == 0)
            {
                return new BadRequestObjectResult("Строка обязательна для поиска.");
            }

            try
            {
                var books = await _context.Books.Include(b => b.Genres)
                .Where(b => b.Id_genre == id_genre)
                .ToListAsync();
                if (books == null || !books.Any())
                {
                    return new NotFoundObjectResult("Книги с указанным запросом не найдены.");
                }

                var booksDto = books.Select(b => new GetAllBooksName
                {
                    Id_Books = b.Id_book,
                    Title = b.Title,
                    Author = b.Author,
                    Id_genre = b.Id_genre,
                    Copies = b.Copies,
                    Year_public = b.Year_public,
                    Description = b.Description
                });

                return new OkObjectResult(booksDto);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> SearchBooksByYearPublic(int YearPublic)
        {
            if (YearPublic == 0)
            {
                return new BadRequestObjectResult("Строка обязательна для поиска.");
            }

            try
            {
                var books = await _context.Books.Include(b => b.Genres)
                .Where(b => b.Year_public == YearPublic)
                .ToListAsync();
                if (books == null || !books.Any())
                {
                    return new NotFoundObjectResult("Книги с указанным запросом не найдены.");
                }

                var booksDto = books.Select(b => new GetAllBooksName
                {
                    Id_Books = b.Id_book,
                    Title = b.Title,
                    Author = b.Author,
                    Id_genre = b.Id_genre,
                    Copies = b.Copies,
                    Year_public = b.Year_public,
                    Description = b.Description
                });

                return new OkObjectResult(booksDto);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> UpdateBook(int id_book, [FromQuery] UpdateBooks Updbook)
        {
            throw new NotImplementedException();
        }
    }
}
