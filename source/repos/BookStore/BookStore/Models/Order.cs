using System;

namespace BookStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int ReaderId { get; set; }
        public Reader Reader { get; set; }

        public DateTime DateOfOrder { get; set; }

        public bool IsBookReturned { get; set; }
    }
}
