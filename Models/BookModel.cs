namespace LibrarySystem.Models
{
    public class BookModel
    {
        
        public int Id { get; set; }

        public string Author { get; set; }
        public string Title { get; set; }

        public string Publisher { get; set; }

        public int Date { get; set; }

        public string User { get; set; }
        public string Reserved { get; set; }
        public string Leased { get; set; }

        
    }

    public class CreateBookList
    {
        public List<BookModel> Books;

        
        public CreateBookList(List<BookModel> books)
        {
            Books = books;
        }
    }
}
