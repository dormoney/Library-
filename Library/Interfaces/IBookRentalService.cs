using LessonApiBiblioteka.Requests;
using Library.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Library.Interfaces
{
    public interface IBookRentalService
    {
        Task<IActionResult> RentBook([FromQuery] RentBookRequest request);
        Task<IActionResult> ReturnBook([FromQuery] ReturnBookRequest request);
        Task<IActionResult> GetRentalHistoryByUser(int id_reader);
        Task<IActionResult> GetCurrentRentals();
        Task<IActionResult> GetRentalHistoryByBook(int id_book);
    }
}
