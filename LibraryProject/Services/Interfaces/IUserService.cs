using webapi.Dto;
using webapi.Models;

namespace webapi.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> Register(AuthenticateDto authDto);
        User? Login(AuthenticateDto authDto);
        User? GetById(string id);
        string CreateToken(User user);
        User? GetByName(string name);
        Book? AddBookToLibrary(string userId, string Id);
        Book? DeleteBookFromLibrary(string userId, string Id);
        IEnumerable<Book> GetUsersBooks(string userId);




    }
}
