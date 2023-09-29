using Microsoft.EntityFrameworkCore;


namespace webapi.Models
{
    [Index("Name")]
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }

        public List<Book> Books { get; set; } = new();
    }
}
