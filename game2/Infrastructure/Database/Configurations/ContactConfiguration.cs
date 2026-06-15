using TestCrudApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestCrudApplication.Infrastructure.Database.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> contact)
    {
        contact.ToTable("contacts");

        contact.HasKey(x => x.Id);
        contact.Property(x => x.Id)
        .HasColumnName("id");

        contact.Property(x => x.Uuid)
        .HasColumnName("uuid")
        .HasColumnType("uuid")
        .HasDefaultValueSql("gen_random_uuid()")
        .IsRequired();
        contact.HasIndex(x => x.Uuid)
        .IsUnique();

        contact.Property(x => x.Email)
        .HasColumnName("email")
        .HasMaxLength(255)
        .IsRequired();
        contact.HasIndex(x => x.Email)
        .IsUnique();

        contact.Property(x => x.Age)
        .HasColumnName("age")
        .IsRequired();

        contact.Property(x => x.CreatedAt)
        .HasColumnName("created_at")
        .HasDefaultValueSql("now()")
        .IsRequired();

        contact.Property(x => x.UpdatedAt)
        .HasColumnName("updated_at")
        .HasDefaultValueSql("now()")
        .IsRequired();
    }
}