using Microsoft.AspNetCore.Cryptography.KeyDerivation;
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
    public class AdminService : IAdminService
    {
        private readonly ApplicationContext _context;
        private readonly IConfiguration _configuration;
        public AdminService(ApplicationContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public Admin Login(AuthenticateDto authDto)
        {
            string hashedPassword = hashPassword(authDto.Password);
            var admin = _context.Admins.FirstOrDefault(x => x.Name == authDto.Name);
            if (admin == null) return null;
            if (admin.PasswordHash == hashedPassword) return admin;
            return null;
        }
        public string CreateToken(Admin admin)
        {
            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier,admin.Id.ToString()),
                new Claim(ClaimTypes.Name,admin.Name)
            };
            if (admin.isSuperAdmin) claims.Add(new Claim(ClaimTypes.Role, "SuperAdmin"));
            else claims.Add(new Claim(ClaimTypes.Role, "Admin"));
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
        
        public Admin CreateAdmin(AuthenticateDto admin)
        {
            string hashedPassword = hashPassword(admin.Password);
            Admin newAdmin = new Admin()
            {
                Name = admin.Name,
                PasswordHash = hashedPassword,
            };
            _context.Admins.Add(newAdmin);
            _context.SaveChanges();

            return newAdmin;
        }
    }
}
