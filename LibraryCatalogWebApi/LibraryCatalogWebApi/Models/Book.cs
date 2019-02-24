using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace LibraryCatalogWebApi.Models
{
    [KnownType(typeof(Author))]
    public class Book
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        [Required, ForeignKey("Author")]
        public int AuthorId { get; set; }

        public string Description { get; set; }

        public int Year { get; set; }

        public int NumPages { get; set; }

        public Author Author { get; set; }
    }
}