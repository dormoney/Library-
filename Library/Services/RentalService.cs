using LessonApiBiblioteka.Requests;
using Library.DataBaseContext;
using Library.Interfaces;
using Library.Model;
using Library.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class RentalService : IBookRentalService
    {
        private readonly LibraryDB _context;

        public RentalService(LibraryDB context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetCurrentRentals()
        {
            try
            {
                var rentals = await _context.Rent_story.Where(r => r.ReturnDate == null).Include(r => r.Books)
                    .Include(r => r.Readers)
                    .Select(r => new
                    {
                        Title = r.Books.Title,
                        Reader = r.Readers.First_name + " " + r.Readers.Last_name,
                        RentalDate = r.RentalDate,
                        DueDate = r.DueDate
                    })
                    .ToListAsync();

                return new OkObjectResult(rentals);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> GetRentalHistoryByBook(int id_book)
        {
            try
            {
                var rentals = await _context.Rent_story
                    .Where(r => r.Id_book == id_book)
                    .Include(r => r.Books)
                    .Include(r => r.Readers)
                    .Select(r => new
                    {
                        Title = r.Books.Title,
                        Reader = r.Readers.First_name + " " + r.Readers.Last_name,
                        RentalDate = r.RentalDate,
                        DueDate = r.DueDate,
                        Return = r.ReturnDate
                    })
                    .ToListAsync();

                if (rentals == null || rentals.Count == 0)
                {
                    return new NotFoundObjectResult("История аренды не найдена.");
                }

                return new OkObjectResult(rentals);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> GetRentalHistoryByUser(int id_reader)
        {
            try
            {
                var rentals = await _context.Rent_story
                    .Where(r => r.Id_reader == id_reader)
                    .Include(r => r.Books)
                    .Include(r => r.Readers)
                    .Select(r => new
                    {
                        Title = r.Books.Title,
                        Reader = r.Readers.First_name + " " + r.Readers.Last_name,
                        RentalDate = r.RentalDate,
                        DueDate = r.DueDate,
                        ReturnDate = r.ReturnDate
                    })
                    .ToListAsync();

                if (rentals == null || rentals.Count == 0)
                {
                    return new NotFoundObjectResult("История аренды не найдена.");
                }

                return new OkObjectResult(rentals);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> RentBook([FromQuery] RentBookRequest request)
        {
            if (request == null)
            {
                return new BadRequestObjectResult("Некорректные данные для аренды книги.");
            }

            try
            {
                var book = await _context.Books.FindAsync(request.Id_book);
                if (book == null)
                {
                    return new NotFoundObjectResult("Книга не найдена.");
                }

                if (book.Copies <= 0)
                {
                    return new BadRequestObjectResult("Нет доступных копий для аренды.");
                }
                var reader = await _context.Readers
                .FirstOrDefaultAsync(g => g.Id_reader == request.Id_reader);
                var rental = new Rent_story
                {
                    Id_book = request.Id_book,
                    Id_reader = reader.Id_reader,
                    RentalDate = DateTime.UtcNow,
                    DueDate = request.DueDate
                };
                book.Copies--;

                _context.Rent_story.Add(rental);
                await _context.SaveChangesAsync();

                return new OkObjectResult("Книга арендована.");
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> ReturnBook([FromQuery] ReturnBookRequest request)
        {
            if (request == null)
            {
                return new BadRequestObjectResult("Некорректные данные для сдачи книги.");
            }

            try
            {
                var rental = await _context.Rent_story.FindAsync(request.RentalId);
                if (rental == null)
                {
                    return new NotFoundObjectResult("Аренда не найдена.");
                }

                var book = await _context.Books.FindAsync(rental.Id_book);
                if (book == null)
                {
                    return new NotFoundObjectResult("Книга не найдена.");
                }

                rental.ReturnDate = DateTime.UtcNow;
                book.Copies++;

                await _context.SaveChangesAsync();

                return new OkObjectResult("Книга возвращена.");
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }
    }
}
