using CustomerProcessManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerProcessManagement.Data.Mapping
{
    public class PhysicalPersonMap : IEntityTypeConfiguration<PhysicalPerson>
    {
        public void Configure(EntityTypeBuilder<PhysicalPerson> builder)
        {
            builder.ToTable("PhysicalPersons");

            builder.HasKey(p => p.IdPhysicalPerson);

            builder.Property(p => p.IdPhysicalPerson)
                .HasColumnName("IdPhysicalPerson")
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(p => p.FullName)
                .HasColumnName("FullName")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.CPF)
                .HasColumnName("CPF")
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(p => p.DateOfBirth)
                .HasColumnName("DateOfBirth")
                .IsRequired();

            builder.Property(p => p.Address)
                .HasColumnName("Address")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(p => p.CreatedDate)
                .HasColumnName("CreatedDate")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(p => p.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(p => p.Contact)
                .WithOne()
                .HasForeignKey<Contact>(c => c.IdPhysicalPersonContact)
                .IsRequired();
        }
    }
}
