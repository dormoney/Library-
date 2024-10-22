using Library.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Library.Interfaces
{
    public interface IGenresService
    {
        Task<IActionResult> CreateNewGenre([FromQuery] CreateNewGenre newGenre);
        Task<IActionResult> GetAllGenres();
        Task<IActionResult> UpdateGenre(int id_genre, [FromQuery] UpdateGenres updateGenres);
        Task<IActionResult> DeleteGenre(int id_genre);
    }
}
