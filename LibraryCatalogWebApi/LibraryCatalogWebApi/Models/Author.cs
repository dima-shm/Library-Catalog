using System.ComponentModel.DataAnnotations;

namespace LibraryCatalogWebApi.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        
        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public override string ToString()
        {
            return $"{Surname} {Name} {Patronymic}";
        }
    }
}