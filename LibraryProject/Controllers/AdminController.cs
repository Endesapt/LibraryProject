using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using webapi.Dto;
using webapi.Models;
using webapi.Services.Interfaces;

namespace webapi.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,SuperAdmin")]

    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IBookService _bookService;
        public AdminController(IAdminService adminService, IBookService bookService)
        {
            _adminService = adminService;
            _bookService = bookService;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login(AuthenticateDto request)
        {
            if (request == null) return BadRequest("null");
            if (request.Name == "" && request.Password == null) return BadRequest("No password or login");
            var admin = _adminService.Login(request);
            if (admin == null) return BadRequest("Incorrect username or password");
            string token = _adminService.CreateToken(admin);
            return Ok(token);
        }
        [HttpPost("bookCreate")]
        public async Task<IActionResult> CreateBook([FromForm] BookDto book)
        {
            var file = book.File;
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Book newBook = await _bookService.CreateBook(book, file);
            if (newBook == null) return BadRequest("Unexcpected Error");
            return Ok(newBook);
        }
        [HttpDelete("bookDelete")]
        public IActionResult DeleteBook(string id)
        {
            if (id.IsNullOrEmpty()) return BadRequest("No ID provided");
            Book book = _bookService.DeleteBookById(id);
            if (book == null) return BadRequest("Book with this ID does not exist");
            return Ok(book);
        }

        [HttpPost("adminCreate")]
        //[Authorize(Roles ="SuperAdmin")]
        //Anyone can create Admins for development purposes
        [AllowAnonymous]
        public IActionResult CreateAdmin(AuthenticateDto admin)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Admin newAdmin = _adminService.CreateAdmin(admin);
            if (newAdmin == null) return BadRequest("Admin was not created");
            return Ok(newAdmin);

        }

    }
}
