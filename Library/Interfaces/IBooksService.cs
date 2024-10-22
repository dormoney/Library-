using Library.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Library.Interfaces
{
    public interface IBooksService
    {
        Task<IActionResult> GetBooks(int page, int pageSize);
        Task<IActionResult> FindById(int id_book);
        Task<IActionResult> FindByIdGenre(int id_genre);
        Task<IActionResult> CreateNewBook([FromQuery] CreateNewBook newBook);
        Task<IActionResult> DeleteBook(int id_book);
        Task<IActionResult> UpdateBook(int id_book, [FromQuery] UpdateBooks Updbook);
        Task<IActionResult> SearchBooksByTitle(string Title);
        Task<IActionResult> SearchBooksByAuthor(string Author);
        Task<IActionResult> SearchBooksByGenre(int id_genre);
        Task<IActionResult> SearchBooksByYearPublic(int YearPublic);
        Task<IActionResult> FindCopies(string Title);
    }
}
