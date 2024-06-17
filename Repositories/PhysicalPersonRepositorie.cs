using CustomerProcessManagement.Data;
using CustomerProcessManagement.Models;
using CustomerProcessManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace CustomerProcessManagement.Repositories
{
    public class PhysicalPersonRepositorie : IPhysicalPersonRepositorie
    {
        private readonly SystemProcessManagementcsDBContext _dbContext;

        public PhysicalPersonRepositorie(SystemProcessManagementcsDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<PhysicalPerson>> GetAllAsync()
        {
            var result = await _dbContext.PhysicalPerson.ToListAsync();
            return result;
        }

        public async Task<PhysicalPerson?> GetAsync(string cpf)
        {
            var physicalPerson = await _dbContext.PhysicalPerson.FirstOrDefaultAsync(pp => pp.CPF == cpf);
            return physicalPerson;
        }

        public async Task<PhysicalPerson> AddAsync(PhysicalPerson physicalPerson)
        {
            ValidatePhysicalPerson(physicalPerson);

            if (physicalPerson.DateOfBirth.Kind != DateTimeKind.Utc)
            {
                physicalPerson.DateOfBirth = physicalPerson.DateOfBirth.ToUniversalTime();
            }

            var existingPhysicalPerson = await _dbContext.PhysicalPerson.FirstOrDefaultAsync(lp => lp.CPF == physicalPerson.CPF);

            if (existingPhysicalPerson != null)
            {
                throw new Exception($"CPF: {physicalPerson.CPF} Já cadastrado no banco.");
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                await _dbContext.AddAsync(physicalPerson);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return physicalPerson;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<PhysicalPerson> UpdateAsync(PhysicalPerson physicalPerson, string cpf)
        {
            var physicalPersonForId = await _dbContext.PhysicalPerson
                .Include(p => p.Contact)
                .FirstOrDefaultAsync(p => p.CPF == cpf);

            if (physicalPersonForId == null)
            {
                throw new Exception($"Pessoa física com o CPF: {cpf} não está registrada no banco e não pode ser atualizada.");
            }

            ValidatePhysicalPerson(physicalPerson);

            try
            {
                physicalPersonForId.FullName = physicalPerson.FullName;
                physicalPersonForId.CPF = physicalPerson.CPF;
                physicalPersonForId.DateOfBirth = DateTime.SpecifyKind(physicalPerson.DateOfBirth, DateTimeKind.Utc);
                physicalPersonForId.Address = physicalPerson.Address;
                physicalPersonForId.CreatedDate = DateTime.SpecifyKind(physicalPerson.CreatedDate, DateTimeKind.Utc);
                physicalPersonForId.UpdatedDate = DateTime.UtcNow;

                physicalPersonForId.Contact.Telephone = physicalPerson.Contact.Telephone;
                physicalPersonForId.Contact.Email = physicalPerson.Contact.Email;

                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("A entidade foi modificada ou deletada por outro usuário. Atualize os dados e tente novamente.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar a pessoa física: {ex.Message}");
                throw;
            }

            return physicalPersonForId;
        }

        public async Task<bool> DeletePhysicalPersonAsync(string cpf)
        {
            PhysicalPerson? physicalPersonToDelete = await GetAsync(cpf);

            if (physicalPersonToDelete == null)
            {
                throw new Exception($"Pessoa física com o CPF: {cpf} não foi encontrada.");
            }

            _dbContext.PhysicalPerson.Remove(physicalPersonToDelete);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private void ValidatePhysicalPerson(PhysicalPerson physicalPerson)
        {
            if (string.IsNullOrWhiteSpace(physicalPerson.FullName))
            {
                throw new ArgumentException("O nome completo é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(physicalPerson.CPF?.ToString()))
            {
                throw new ArgumentException("O CPF é obrigatório.");
            }

            if (!IsValidCPF(physicalPerson.CPF.ToString()))
            {
                throw new ArgumentException("O CPF é inválido.");
            }

            if (physicalPerson.Contact == null)
            {
                throw new ArgumentException("Os contatos são obrigatórios.");
            }

            ValidateContact(physicalPerson.Contact);
        }

        private void ValidateContact(Contact contact)
        {
            if (string.IsNullOrWhiteSpace(contact.Telephone))
            {
                throw new ArgumentException("O telefone é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(contact.Email) || !IsValidEmail(contact.Email))
            {
                throw new ArgumentException("O email é obrigatório e deve ser válido.");
            }
        }

        private bool IsValidCPF(string cpf)
        {
            cpf = Regex.Replace(cpf, "[^0-9]", "");

            if (cpf.Length != 11)
            {
                return false;
            }

            if (new string(cpf[0], 11) == cpf)
            {
                return false;
            }

            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                sum += (cpf[i] - '0') * (10 - i);
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
