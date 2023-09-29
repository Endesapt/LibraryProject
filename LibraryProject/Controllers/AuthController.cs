using Microsoft.AspNetCore.Mvc;
using webapi.Dto;
using webapi.Services.Interfaces;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public AuthController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(AuthenticateDto request)
        {
            if (_userService.GetByName(request.Name) != null) return BadRequest($"User with name {request.Name} already exists");
            var user = await _userService.Register(request);
            string token = _userService.CreateToken(user);
            return Ok(token);
        }

        [HttpPost("login")]
        public IActionResult Login(AuthenticateDto request)
        {
            if (request == null) return BadRequest("null");
            if (request.Name == "" && request.Password == null) return BadRequest("No password or login");
            var user = _userService.Login(request);
            if (user == null) return BadRequest("Incorrect username or password");
            string token = _userService.CreateToken(user);
            return Ok(token);
        }

    }
}
