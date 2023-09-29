namespace webapi.Dto
{
    public class BookDto
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public IFormFile File { get; set; }

    }
}
