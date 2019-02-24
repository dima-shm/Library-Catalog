using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryCatalog.Models
{
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

        public virtual Author Author { get; set; }
    }
}