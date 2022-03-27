using BooksAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>().HasData(new Book { BookId = 1, Title = "Babička", Pages = 100 });
            modelBuilder.Entity<Book>().HasData(new Book { BookId = 2, Title = "Guliverovy cesty", Pages = 200 });
        }
    }
}
