using BeginnerCrud.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeginnerCrud.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}

    public DbSet<Contact> Contacts => Set<Contact>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgcrypto");

        var contact = modelBuilder.Entity<Contact>();

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

        // contact.Property(x => x.PhoneNumber)
        // .HasColumnName("phone_number")
        // .HasMaxLength(20);

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