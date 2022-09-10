using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public double Price { get; set; }

        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        public int YearOfPublishing { get; set; }

        public string ImagePath { get; set; }

    }
}
