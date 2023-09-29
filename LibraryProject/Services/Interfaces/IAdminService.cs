using webapi.Dto;
using webapi.Models;

namespace webapi.Services.Interfaces
{
    public interface IAdminService
    {
        public Admin Login(AuthenticateDto authDto);
        public string CreateToken(Admin admin);
        public Task<Book> CreateBook(BookDto bookDto, IFormFile file);
        public Admin CreateAdmin(AuthenticateDto admin);
    }
}
