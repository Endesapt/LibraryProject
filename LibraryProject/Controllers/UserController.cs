using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using webapi.Models;
using webapi.Services.Interfaces;

namespace webapi.Controllers
{
    [Route("/api")]
    [ApiController]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("getUserBooks")]
        public IActionResult GetUserBooks()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var books = _userService.GetUsersBooks(userId);
            return Ok(books);
        }
        [HttpPost("addBookToLibrary")]
        public IActionResult AddBookToLibrary(string bookId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            Book? book = _userService.AddBookToLibrary(userId, bookId);
            if (book == null) return BadRequest("No book founded or this book is in ypur library");
            return Ok(book);
        }
        [HttpDelete("deleteBookFromLibrary")]
        public IActionResult DeleteBookFromLibrary(string bookId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            Book? book = _userService.DeleteBookFromLibrary(userId, bookId);
            if (book == null) return BadRequest("User dont have this book");
            return Ok(book);
        }

    }
}
