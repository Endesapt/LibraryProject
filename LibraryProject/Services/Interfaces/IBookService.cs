using webapi.Models;

namespace webapi.Services.Interfaces
{
    public interface IBookService
    {
        public IEnumerable<Book> GetAllBooks(int limit,int skip);
        public Book GetBookById(string id);

    }
}