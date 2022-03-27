namespace BooksAPI.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; } = "";
        public int Pages { get; set; }
        public int BorrowCount { get; set; } = 0;
    }
}
