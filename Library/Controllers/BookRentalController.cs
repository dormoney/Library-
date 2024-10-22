using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Library.Requests;
using Library.DataBaseContext;
using Library.Model;
using LessonApiBiblioteka.Requests;
using Library.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class BookRentalController : ControllerBase
{
    private readonly IBookRentalService _bookRentalService;

    public BookRentalController(IBookRentalService bookRentalService)
    {
        _bookRentalService = bookRentalService;
    }

    [HttpPost("rent")]
    public async Task<IActionResult> RentBook([FromQuery] RentBookRequest request)
    {
        return await _bookRentalService.RentBook(request);
    }

    [HttpPost("return")]
    public async Task<IActionResult> ReturnBook([FromQuery] ReturnBookRequest request)
    {
        return await _bookRentalService.ReturnBook(request);
    }

    [HttpGet("user/{id_reader}/history")]
    public async Task<IActionResult> GetRentalHistoryByUser(int id_reader)
    {
        return await _bookRentalService.GetRentalHistoryByUser(id_reader);
    }

    [HttpGet("GetAllRents")]
    public async Task<IActionResult> GetCurrentRentals()
    {
        return await _bookRentalService.GetCurrentRentals();
    }

    [HttpGet("book/{id_book}/history")]
    public async Task<IActionResult> GetRentalHistoryByBook(int id_book)
    {
        return await _bookRentalService.GetRentalHistoryByBook(id_book);
    }
}