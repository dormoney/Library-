using Library.DataBaseContext;
using Library.Interfaces;
using Library.Model;
using Library.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : Controller
    {
        private readonly IGenresService _genresService;

        public GenresController(IGenresService genresService) 
        {
            _genresService = genresService;
        }

        [HttpPost]
        [Route("createNewGenre")]
        public async Task<IActionResult> CreateNewGenre([FromQuery] CreateNewGenre newGenre)
        {
            return await _genresService.CreateNewGenre(newGenre);
        }

        [HttpGet]
        [Route("getAllGenres")]
        public async Task<IActionResult> GetAllGenres()
        {
           return await _genresService.GetAllGenres();
        }

        [HttpPut("UpdateGenre/{id_genre}")]
        public async Task<IActionResult> UpdateGenre(int id_genre, [FromQuery] UpdateGenres updateGenres)
        {
            return await _genresService.UpdateGenre(id_genre, updateGenres);
        }

        [HttpDelete("DeleteGenre/{id_genre}")]
        public async Task<IActionResult> DeleteGenre(int id_genre)
        {
            return await _genresService.DeleteGenre(id_genre);
        }
    }
}