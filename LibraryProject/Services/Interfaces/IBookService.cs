using webapi.Dto;
using webapi.Models;

namespace webapi.Services.Interfaces
{
    public interface IBookService
    {
        
        public IEnumerable<Book> GetAllBooks(int page);
        public Task<Book> CreateBook(BookDto bookDto, IFormFile file);
        public Book GetBookById(string id);
        public IEnumerable<Book> FindBookByName(string name);
        public Book DeleteBookById(string id);

    }
}