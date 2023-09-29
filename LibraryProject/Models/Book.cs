using System.Text.Json.Serialization;

namespace webapi.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string FilePathString { get; set; }
        [JsonIgnore]
        public List<User> Users { get; set; } = new();
    }
}
