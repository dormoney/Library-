using Library.DataBaseContext;
using Library.Interfaces;
using Library.Model;
using Library.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class GenresService : IGenresService
    {
        private readonly LibraryDB _context;
        public GenresService(LibraryDB context)
        {
            _context = context;
        }
        public async Task<IActionResult> CreateNewGenre([FromQuery] CreateNewGenre newGenres)
        {
            try
            {
                var genres = new Genres()
                {
                    Name_genre = newGenres.Name_genre,
                };
                await _context.Genres.AddAsync(genres);
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> DeleteGenre(int id_genre)
        {
            if (id_genre <= 0)
            {
                return new BadRequestObjectResult("Некорректный идентификатор жанра.");
            }

            try
            {
                var genres = await _context.Genres.FindAsync(id_genre);
                if (genres == null)
                {
                    return new NotFoundObjectResult("Жанр с указанным идентификатором не найден.");
                }

                _context.Genres.Remove(genres);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> GetAllGenres()
        {
            try
            {
                var genres = await _context.Genres.ToListAsync();
                var genresDto = genres.Select(g => new Find.GetAllGenres
                {
                    Id_Genres = g.Id_genre,
                    Name_genre = g.Name_genre,
                });
                return new OkObjectResult(genresDto);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> UpdateGenre(int id_genre, [FromQuery] UpdateGenres updateGenres)
        {
            if (id_genre <= 0 || updateGenres == null)
            {
                return new BadRequestObjectResult("Некорректные данные для обновления жанра.");
            }
            try
            {
                var genres = await _context.Genres.FindAsync(id_genre);
                if (genres == null)
                {
                    return new NotFoundObjectResult("Жанр с указанным идентификатором не найден.");
                }

                genres.Name_genre = updateGenres.Name_genre;

                _context.Genres.Update(genres);
                await _context.SaveChangesAsync();


                return new NoContentResult();
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }
    }
}
