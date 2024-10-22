using Library.DataBaseContext;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Library.Interfaces;
using Library.Model;
using Library.Requests;

namespace Library.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private readonly IBooksService _booksService;
        public BooksController(IBooksService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet]
        [Route("getAllBooks")]
        public async Task<IActionResult> GetBooks(int page, int pageSize)
        {
            return await _booksService.GetBooks(page, pageSize);
        }

        [HttpGet("FindById/{id_book}")]
        public async Task<IActionResult> FindById(int id_book)
        {
            return await _booksService.FindById(id_book);
        }

        [HttpGet("FindByIdGenre/{id_genre}")]
        public async Task<IActionResult> FindByIdGenre(int id_genre)
        {
            return await _booksService.FindByIdGenre(id_genre);
        }

        [HttpPost]
        [Route("createNewBook")]
        public async Task<IActionResult> CreateNewBook([FromQuery] CreateNewBook newBook)
        {
            return await _booksService.CreateNewBook(newBook);
        }

        [HttpDelete("DeleteBook/{id_book}")]
        public async Task<IActionResult> DeleteBook(int id_book)
        {
            return await _booksService.DeleteBook(id_book);
        }

        [HttpPut("UpdateBook/{id_book}")]
        public async Task<IActionResult> UpdateBook(int id_book, [FromQuery] UpdateBooks Updbook)
        {
            return await _booksService.UpdateBook(id_book, Updbook);
        }

        [HttpGet("FindByTitle/{Title}")]
        public async Task<IActionResult> SearchBooksByTitle(string Title)
        {
            return await _booksService.SearchBooksByTitle(Title);
        }

        [HttpGet("FindByAuthor/{Author}")]
        public async Task<IActionResult> SearchBooksByAuthor(string Author)
        {
            return await _booksService.SearchBooksByAuthor(Author);
        }

        [HttpGet("FindByGenre/{id_genre}")]
        public async Task<IActionResult> SearchBooksByGenre(int id_genre)
        {
            return await _booksService.SearchBooksByGenre(id_genre);
        }

        [HttpGet("FindByYearPublic/{YearPublic}")]
        public async Task<IActionResult> SearchBooksByYearPublic(int YearPublic)
        {
            return await _booksService.SearchBooksByYearPublic(YearPublic);
        }

        [HttpGet("FindCopies/{Title}")]
        public async Task<IActionResult> FindCopies(string Title)
        {
            return await _booksService.FindCopies(Title);
        }
    }
}