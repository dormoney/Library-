using Library.DataBaseContext;
using Library.Interfaces;
using Library.Model;
using Library.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Library.Requests.Find;

namespace Library.Services
{
    public class ReadersService : IReadersService
    {
        private readonly LibraryDB _context;
        public ReadersService(LibraryDB context)
        {
            _context = context;
        }
        public async Task<IActionResult> CreateNewReader([FromQuery] CreateNewReader newReader)
        {
            try
            {
                var readers = new Readers()
                {
                    First_name = newReader.First_name,
                    Last_name = newReader.Last_name,
                    Birth_year = newReader.Birth_year,
                    Contact_info = newReader.Contact_info,
                    RegistrationDate = newReader.RegistrationDate
                };
                await _context.Readers.AddAsync(readers);
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }     
              
        public async Task<IActionResult> DeleteReader(int id_reader)
        {
            if (id_reader <= 0)
            {
                return new BadRequestObjectResult("Некорректный идентификатор читателя.");
            }

            try
            {
                var readers = await _context.Readers.FindAsync(id_reader);
                if (readers == null)
                {
                    return new NotFoundObjectResult("Читатель с указанным идентификатором не найдена.");
                }

                _context.Readers.Remove(readers);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> FindById(int id_reader)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> GetAllReaders(int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return new BadRequestObjectResult("Параметры страницы и размера страницы должны быть положительными.");
            }

            try
            {
                var totalReaders = await _context.Readers.CountAsync();
                var totalPages = (int)Math.Ceiling(totalReaders / (double)pageSize);

                var readers = await _context.Readers
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var readersDto = readers.Select(r => new Find.GetAllReaders
                {
                    Id_reader = r.Id_reader,
                    First_name = r.First_name,
                    Last_name = r.Last_name,
                    Birth_year = r.Birth_year,
                    Contact_info = r.Contact_info,
                    RegistrationDate = r.RegistrationDate
                });

                var result = new
                {
                    CurrentPage = page,
                    TotalPages = totalPages,
                    TotalReaders = totalReaders,
                    Readers = readersDto
                };

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }     
              
        public async Task<IActionResult> UpdateReader(int id_reader, [FromQuery] UpdateReaders updreader)
        {
            if (id_reader <= 0 || updreader == null)
            {
                return new BadRequestObjectResult("Некорректные данные для обновления читателя.");
            }
            try
            {
                var readers = await _context.Readers.FindAsync(id_reader);
                if (readers == null)
                {
                    return new NotFoundObjectResult("Читатель с указанным идентификатором не найден.");
                }
                readers.First_name = updreader.First_name;
                readers.Last_name = updreader.Last_name;
                readers.Birth_year = updreader.Birth_year;
                readers.Contact_info = updreader.Contact_info;
                readers.RegistrationDate = updreader.RegistrationDate;

                _context.Readers.Update(readers);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> GetReadersF([FromQuery] DateTime? registrationDate = null)
        {
            try
            {
                var query = _context.Readers.AsQueryable();

                if (registrationDate.HasValue)
                {
                    query = query.Where(r => r.RegistrationDate == registrationDate.Value.Date);
                }

                var readers = await query.ToListAsync();

                var readersDto = readers.Select(b => new GetAllReaders
                {
                    Id_reader = b.Id_reader,
                    First_name = b.First_name,
                    Last_name = b.Last_name,
                    Birth_year = b.Birth_year,
                    Contact_info = b.Contact_info,
                    RegistrationDate = b.RegistrationDate
                });

                return new OkObjectResult(readersDto);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }
    }
}
