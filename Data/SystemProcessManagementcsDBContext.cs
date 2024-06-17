using CustomerProcessManagement.Data.Mapping;
using CustomerProcessManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerProcessManagement.Data
{
    public class SystemProcessManagementcsDBContext : DbContext
    {
        public SystemProcessManagementcsDBContext(DbContextOptions<SystemProcessManagementcsDBContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contact { get; set; }
        public DbSet<LegalPerson> LegalPerson { get; set; }
        public DbSet<PhysicalPerson> PhysicalPerson { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PhysicalPersonMap());
            modelBuilder.ApplyConfiguration(new LegalPersonMap());
            modelBuilder.ApplyConfiguration(new ContactMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
