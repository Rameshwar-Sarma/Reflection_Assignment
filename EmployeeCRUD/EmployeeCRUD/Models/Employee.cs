using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace EmployeeCRUD.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [JsonInclude]
        [XmlElement(ElementName = "FirstName")]
        public string? FirstName { get; set; }

        [JsonInclude]
        [XmlElement(ElementName = "LastName")]
        public string? LastName { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public string? Email { get; set; }
    }
}
