using Lab_3.Models;
using System.Data.Entity;

namespace Lab_3.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }

        public ApplicationContext() : base("LibraryCatalogDB") { }

        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }
    }
}