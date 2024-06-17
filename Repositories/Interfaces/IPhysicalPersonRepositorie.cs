using CustomerProcessManagement.Models;

namespace CustomerProcessManagement.Repositories.Interfaces
{
    public interface IPhysicalPersonRepositorie
    {
        Task<List<PhysicalPerson>> GetAllAsync();
        Task<PhysicalPerson?> GetAsync(string id);
        Task<PhysicalPerson> AddAsync(PhysicalPerson physicalPerson);
        Task<PhysicalPerson> UpdateAsync(PhysicalPerson physicalPerson, string id);
        Task<bool> DeletePhysicalPersonAsync(string id);
    }
}
