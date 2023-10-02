using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Models;
using webapi.Services.Interfaces;

namespace webapi.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IWebHostEnvironment _appEnvironment;
        public BookController(IBookService bookService, IWebHostEnvironment appEnvironment)
        {
            _bookService = bookService;
            _appEnvironment = appEnvironment;

        }

        [HttpGet("getAllBooks")]
        public IActionResult GetAllBooks(int page=0)
        {
            var books = _bookService.GetAllBooks(page);
            return Ok(books);
        }

        [HttpGet("getBookById")]
        public IActionResult GetBookById(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest("No Id provided");
            Book book = _bookService.GetBookById(id);
            if (book == null) return BadRequest("No book with this Id");
            return Ok(book);
        }
        [HttpGet("getBookFile")]
        public IActionResult GetBookFile(string filePath)
        {
            string fullPath = Path.Combine(_appEnvironment.ContentRootPath, "Books", filePath);
            if (!System.IO.File.Exists(fullPath)) return BadRequest("File does not exists");
            return PhysicalFile(fullPath, "text/plain");
        }

    }
}
