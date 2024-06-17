using System.ComponentModel.DataAnnotations;

namespace CustomerProcessManagement.Models
{
    public class LegalPerson
    {
        public int IdLegalPerson { get; set; } = 0;
        public string CorporateReason { get; set; } = string.Empty;
        public string? FantasyName { get; set; }
        public string? CNPJ { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Contact Contact { get; set; } = new Contact();
    }
}
