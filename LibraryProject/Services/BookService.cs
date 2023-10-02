using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using webapi.Data;
using webapi.Dto;
using webapi.Models;
using webapi.Services.Interfaces;

namespace webapi.Services
{

    public class BookService : IBookService
    {
        public readonly ApplicationContext _context;
        private const int BOOKS_PER_PAGE = 10;
        public BookService(ApplicationContext context)
        {
            _context = context;
        }
        public IEnumerable<Book> GetAllBooks(int page)
        {
            if(page<0)return Enumerable.Empty<Book>();
            int skip = (page) * BOOKS_PER_PAGE;
            IEnumerable<Book> books = _context.Books.AsNoTracking().Skip(skip).Take(BOOKS_PER_PAGE).ToList();
            return books;
        }
        public Book GetBookById(string id)
        {
            return _context.Books.Find(Guid.Parse(id));
        }
        public async Task<Book> CreateBook(BookDto bookDto, IFormFile file)
        {
            Guid id = Guid.NewGuid();
            string ext = Path.GetExtension(file.FileName);
            string loadPath = $"Books/{id}{ext}";
            using (var fileStream = new FileStream(loadPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            Book book = new Book()
            {
                Author = bookDto.Author,
                Description = bookDto.Description,
                Title = bookDto.Title,
                Id = Guid.NewGuid(),
                FilePathString = $"{id}{ext}",
            };
            _context.Books.Add(book);
            _context.SaveChanges();
            return book;
        }
        public IEnumerable<Book> FindBookByName(string name)
        {
            var books=_context.Books.AsNoTracking().Where(book=>book.Title.StartsWith(name)).Take(10);
            return books.ToList();
        }
        public Book DeleteBookById(string id)
        {
            Book book = _context.Books.Find(Guid.Parse(id));
            if (book == null)
            {
                return null;
            }
            if(File.Exists(Path.Combine("Books", book.FilePathString)))
            {
                File.Delete(Path.Combine("Books", book.FilePathString));
                _context.Books.Remove(book);
                _context.SaveChanges();
                return book;
            }
            else
            {
                return null;
            }
            
            
        }
    }
}
