using CustomerProcessManagement.Models;

namespace CustomerProcessManagement.Repositories.Interfaces
{
    public interface ILegalPersonRepositorie
    {
        Task<List<LegalPerson>> GetAllAsync();
        Task<LegalPerson> GetAsync(string id);
        Task<LegalPerson> AddAsync(LegalPerson legalPerson);
        Task<LegalPerson> UpdateAsync(LegalPerson legalPerson, string id);
        Task<bool> DeleteLegalPersonAsync(string id);
    }
}
