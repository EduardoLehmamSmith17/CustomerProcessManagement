using CustomerProcessManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerProcessManagement.Data.Mapping
{
    public class LegalPersonMap : IEntityTypeConfiguration<LegalPerson>
    {
        public void Configure(EntityTypeBuilder<LegalPerson> builder)
        {
            builder.ToTable("LegalPersons");

            builder.HasKey(p => p.IdLegalPerson);

            builder.Property(p => p.IdLegalPerson)
                .HasColumnName("IdLegalPerson")
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(p => p.CorporateReason)
                .HasColumnName("CorporateReason")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.FantasyName)
                .HasColumnName("FantasyName")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.CNPJ)
                .HasColumnName("CNPJ")
                .HasMaxLength(14)
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
                .HasForeignKey<Contact>(c => c.IdLegalPersonContact)
                .IsRequired();
        }
    }
}
