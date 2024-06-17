using CustomerProcessManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerProcessManagement.Data.Mapping
{
    public class ContactMap : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contacts");

            builder.HasKey(c => c.IdContact); // Define a chave primária

            builder.Property(c => c.IdContact)
                .HasColumnName("IdContact")
                .IsRequired();

            builder.Property(c => c.Telephone)
                .HasColumnName("Telephone")
                .IsRequired();

            builder.Property(c => c.Email)
                .HasColumnName("Email")
                .IsRequired();

        }
    }
}
