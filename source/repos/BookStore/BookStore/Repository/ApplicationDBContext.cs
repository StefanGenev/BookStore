using BookStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository
{
    public class ApplicationDBContext : IdentityDbContext<Reader>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> contextOptions)
            : base(contextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>().ToTable("BOOKS");
            modelBuilder.Entity<Author>().ToTable("AUTHORS");
            modelBuilder.Entity<Order>().ToTable("ORDERS");
            modelBuilder.Entity<Reader>().ToTable("READERS");
            modelBuilder.Entity<Publisher>().ToTable("PUBLISHERS");

            modelBuilder.Entity<Book>()
                .HasOne(book => book.Author)
                .WithMany()
                .HasForeignKey(book => book.AuthorId);

            modelBuilder.Entity<Book>()
                .HasOne(book => book.Publisher)
                .WithMany()
                .HasForeignKey(book => book.PublisherId);

            modelBuilder.Entity<Order>()
                .HasOne(order => order.Book)
                .WithMany()
                .HasForeignKey(order => order.BookId);

            modelBuilder.Entity<Order>()
                .HasOne(order => order.Reader)
                .WithMany()
                .HasForeignKey(order => order.ReaderId);
        }
    }
}
