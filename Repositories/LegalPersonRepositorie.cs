using CustomerProcessManagement.Data;
using CustomerProcessManagement.Models;
using CustomerProcessManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace CustomerProcessManagement.Repositories
{
    public class LegalPersonRepositorie : ILegalPersonRepositorie
    {
        private readonly SystemProcessManagementcsDBContext _dbContext;

        public LegalPersonRepositorie(SystemProcessManagementcsDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<LegalPerson>> GetAllAsync()
        {
            var result = await _dbContext.LegalPerson.ToListAsync();
            return result;
        }

        public async Task<LegalPerson> GetAsync(string cnpj)
        {
            var legalPerson = await _dbContext.LegalPerson.FirstOrDefaultAsync(lp => lp.CNPJ == cnpj);
            return legalPerson!;
        }

        public async Task<LegalPerson> AddAsync(LegalPerson legalPerson)
        {
            ValidateLegalPerson(legalPerson);

            var existingLegalPerson = await _dbContext.LegalPerson.FirstOrDefaultAsync(lp => lp.CNPJ == legalPerson.CNPJ);

            if (existingLegalPerson != null)
            {
                throw new Exception($"CNPJ: {legalPerson.CNPJ} Já cadastrado no banco.");
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                await _dbContext.AddAsync(legalPerson);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return legalPerson;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<LegalPerson> UpdateAsync(LegalPerson legalPerson, string cnpj)
        {
            var legalPersonForId = await _dbContext.LegalPerson
                .Include(p => p.Contact)
                .FirstOrDefaultAsync(p => p.CNPJ == cnpj);

            if (legalPersonForId == null)
            {
                throw new Exception($"Pessoa jurídica com o CNPJ: {cnpj} não está registrada no banco e não pode ser atualizada.");
            }

            ValidateLegalPerson(legalPerson);

            try
            {
                legalPersonForId.CorporateReason = legalPerson.CorporateReason;
                legalPersonForId.FantasyName = legalPerson.FantasyName;
                legalPersonForId.CNPJ = legalPerson.CNPJ;
                legalPersonForId.Address = legalPerson.Address;
                legalPersonForId.CreatedDate = DateTime.SpecifyKind(legalPerson.CreatedDate, DateTimeKind.Utc);
                legalPersonForId.UpdatedDate = DateTime.SpecifyKind(legalPerson.UpdatedDate, DateTimeKind.Utc);

                legalPersonForId.Contact.Telephone = legalPerson.Contact.Telephone;
                legalPersonForId.Contact.Email = legalPerson.Contact.Email;

                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("A entidade foi modificada ou deletada por outro usuário. Atualize os dados e tente novamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar a pessoa física: {ex.Message}");
                throw;
            }

            return legalPersonForId;
        }

        public async Task<bool> DeleteLegalPersonAsync(string cnpj)
        {
            var legalPersonToDelete = await GetAsync(cnpj);

            if (legalPersonToDelete == null)
            {
                throw new Exception($"Pessoa jurídica com o CNPJ: {cnpj} não foi encontrada.");
            }

            _dbContext.Remove(legalPersonToDelete);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        private void ValidateLegalPerson(LegalPerson legalPerson)
        {
            if (string.IsNullOrWhiteSpace(legalPerson.CorporateReason))
            {
                throw new ArgumentException("A razão social é obrigatória.");
            }

            if (string.IsNullOrWhiteSpace(legalPerson.CNPJ?.ToString()))
            {
                throw new ArgumentException("O CNPJ é obrigatório.");
            }

            if (!IsValidCNPJ(legalPerson.CNPJ.ToString()))
            {
                throw new ArgumentException("O CNPJ é inválido.");
            }

            if (legalPerson.Contact == null)
            {
                throw new ArgumentException("Os contatos são obrigatórios.");
            }

            ValidateContact(legalPerson.Contact);
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

        private bool IsValidCNPJ(string cnpj)
        {
            cnpj = Regex.Replace(cnpj, "[^0-9]", "");

            if (cnpj.Length != 14)
            {
                return false;
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
