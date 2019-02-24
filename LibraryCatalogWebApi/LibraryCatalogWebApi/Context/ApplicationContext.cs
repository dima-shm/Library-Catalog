using LibraryCatalogWebApi.Models;
using System.Data.Entity;

namespace LibraryCatalogWebApi.Context
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