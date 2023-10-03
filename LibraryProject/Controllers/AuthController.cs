using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using webapi.Dto;
using webapi.Services.Interfaces;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        public AuthController( IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(AuthenticateDto request)
        {
            if(!ModelState.IsValid)return BadRequest(ModelState);
            if (_userService.GetByName(request.Name) != null) return BadRequest($"User with name {request.Name} already exists");
            var user = await _userService.Register(request);
            string token = _userService.CreateToken(user);
            return Ok(token);
        }

        [HttpPost("login")]
        public IActionResult Login(AuthenticateDto request)
        {
            if (request == null) return BadRequest("null");
            if (request.Name.IsNullOrEmpty() || request.Password.IsNullOrEmpty()) return BadRequest("No password or login");
            var user = _userService.Login(request);
            if (user == null) return BadRequest("Incorrect username or password");
            string token = _userService.CreateToken(user);
            return Ok(token);
        }  

    }
}
