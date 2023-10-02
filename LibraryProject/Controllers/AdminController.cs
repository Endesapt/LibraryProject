using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
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
            Book newBook = await _adminService.CreateBook(book, file);
            if (newBook == null) return BadRequest("Unexcpected Error");
            return Ok(newBook);
        }

        [HttpPost("adminCreate")]
        [Authorize(Roles ="SuperAdmin")]
        public IActionResult CreateAdmin(AuthenticateDto admin)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Admin newAdmin = _adminService.CreateAdmin(admin);
            if (newAdmin == null) return BadRequest("Admin was not created");
            return Ok(newAdmin);

        }

    }
}
