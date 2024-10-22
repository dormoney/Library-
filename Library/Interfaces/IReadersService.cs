using Library.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Library.Interfaces
{
    public interface IReadersService
    {
        Task<IActionResult> CreateNewReader([FromQuery] CreateNewReader newReader);
        Task<IActionResult> GetAllReaders(int page, int pageSize);
        Task<IActionResult> FindById(int id_reader);
        Task<IActionResult> UpdateReader(int id_reader, [FromQuery] UpdateReaders updreader);
        Task<IActionResult> DeleteReader(int id_reader);
        Task<IActionResult> GetReadersF(DateTime? registrationDate);
    }
}
