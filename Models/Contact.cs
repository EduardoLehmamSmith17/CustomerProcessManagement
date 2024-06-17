using System.ComponentModel.DataAnnotations;

namespace CustomerProcessManagement.Models
{
    public class Contact
    {
        public int IdContact { get; set; }
        public string Telephone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public int? IdLegalPersonContact { get; set; }
        public int? IdPhysicalPersonContact { get; set; }
    }
}
