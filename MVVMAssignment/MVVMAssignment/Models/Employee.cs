
using System.ComponentModel.DataAnnotations;

namespace MVVMAssignment.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string? Email { get; set; }
    }
}

