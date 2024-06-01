using System.ComponentModel.DataAnnotations;

namespace MinimalAPIAssignment.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        public IEnumerable<Book>? Books { get; set; }
    }
}
