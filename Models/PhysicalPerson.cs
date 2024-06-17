using System.ComponentModel.DataAnnotations;

namespace CustomerProcessManagement.Models
{
    public class PhysicalPerson
    {
        public int IdPhysicalPerson { get; set; } = 0;
        public string? FullName { get; set; } = string.Empty;
        public string? CPF { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Contact Contact { get; set; } = new Contact();
    }
}
