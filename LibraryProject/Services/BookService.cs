using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models;
using webapi.Services.Interfaces;

namespace webapi.Services
{

    public class BookService : IBookService
    {
        public readonly ApplicationContext _context;
        public BookService(ApplicationContext context)
        {
            _context = context;
        }
        public IEnumerable<Book> GetAllBooks(int limit,int skip)
        {
            IEnumerable<Book> books = _context.Books.AsNoTracking().Skip(skip).Take(limit).ToList();
            return books;
        }
        public Book GetBookById(string id)
        {
            return _context.Books.Find(Guid.Parse(id));
        }
    }
}
