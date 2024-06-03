using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MinimalAPIAssignment.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Title { get; set; }
        [Required]
        public required string ISBN { get; set; }
        public DateTime PublicationDate { get; set; }
        public int? AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}
