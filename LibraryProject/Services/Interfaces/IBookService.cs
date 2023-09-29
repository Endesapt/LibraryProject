using webapi.Models;

namespace webapi.Services.Interfaces
{
    public interface IBookService
    {
        public IEnumerable<Book> GetAllBooks();
        public Book GetBookById(string id);

    }
}