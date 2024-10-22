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
    public class ReadersController : Controller
    {
        private readonly IReadersService _readersService;

        public ReadersController(IReadersService readersService)
        {
            _readersService = readersService;
        }

        [HttpPost]
        [Route("createNewReader")]
        public async Task<IActionResult> CreateNewReader([FromQuery] CreateNewReader newReader)
        {
            return await _readersService.CreateNewReader(newReader);
        }

        [HttpGet]
        [Route("getAllReadersWithFilter")]
        public async Task<IActionResult> GetReadersF([FromQuery] DateTime? registrationDate = null)
        {
            return await _readersService.GetReadersF((DateTime)registrationDate);
        }

        [HttpGet]
        [Route("getAllReaders")]
        public async Task<IActionResult> GetAllReaders(int page, int pageSize)
        {
            return await _readersService.GetAllReaders(page, pageSize);
        }

        [HttpGet("FindById/{id_reader}")]
        public async Task<IActionResult> FindById(int id_reader)
        {
            return await _readersService.FindById(id_reader);
        }

        [HttpPut("UpdateReader/{id_reader}")]
        public async Task<IActionResult> UpdateReader(int id_reader, [FromQuery] UpdateReaders updreader)
        {
            return await _readersService.UpdateReader(id_reader, updreader);
        }

        [HttpDelete("DeleteReader/{id_reader}")]
        public async Task<IActionResult> DeleteReader(int id_reader)
        {
            return await _readersService.DeleteReader(id_reader);
        }
    }
}