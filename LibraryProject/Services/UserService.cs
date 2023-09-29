using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using webapi.Data;
using webapi.Dto;
using webapi.Models;
using webapi.Services.Interfaces;

namespace webapi.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext _context;
        private readonly IConfiguration _configuration;
        public UserService(ApplicationContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        public async Task<User> Register(AuthenticateDto authDto)
        {
            string hashedPassword = hashPassword(authDto.Password);
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Name = authDto.Name,
                PasswordHash = hashedPassword
            };
            await _context.AddAsync<User>(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public User? Login(AuthenticateDto authDto)
        {
            string hashedPassword = hashPassword(authDto.Password);
            var user = _context.Users.FirstOrDefault(x => x.Name == authDto.Name);
            if (user == null) return null;
            if (user.PasswordHash == hashedPassword) return user;
            return null;
        }
        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.Role,"User"),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Name)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["JWT_SECRET"]!
                ));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public User? GetById(string id)
        {
            var user = _context.Users.Find(Guid.Parse(id));
            if (user == null) return null;
            return user;
        }
        public User? GetByName(string name)
        {
            var user = _context.Users.FirstOrDefault(x => x.Name == name);
            if (user == null) return null;
            return user;
        }
        private string hashPassword(string password)
        {
            byte[] salt = Encoding.UTF8.GetBytes(_configuration["PASSWORD_SALT"]!);
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8)
            );
            return hashedPassword;
        }
        public Book? AddBookToLibrary(string userId, string Id)
        {
            User? findUser = _context.Users.Include(user => user.Books).FirstOrDefault(x => x.Id.ToString() == userId);
            if (findUser == null) return null;
            Book? book = findUser.Books.FirstOrDefault(x => x.Id.ToString() == Id);
            if (book != null) return null;
            book = _context.Books.Find(Guid.Parse(Id));
            if (book == null) return null;
            findUser.Books.Add(book);
            _context.SaveChanges();
            return book;
        }
        public Book? DeleteBookFromLibrary(string userId, string Id)
        {
            User? findUser = _context.Users.Include(user => user.Books).FirstOrDefault(x => x.Id.ToString() == userId);
            if (findUser == null) return null;
            Book? book = _context.Books.Find(Guid.Parse(Id));
            if (book == null) return null;
            findUser.Books.Remove(book);
            _context.SaveChanges();
            return book;
        }
        public IEnumerable<Book> GetUsersBooks(string userId)
        {
            User? findUser = _context.Users.Include(user => user.Books).FirstOrDefault(x => x.Id.ToString() == userId);
            if (findUser == null) return null;
            return findUser.Books;
        }

    }
}
