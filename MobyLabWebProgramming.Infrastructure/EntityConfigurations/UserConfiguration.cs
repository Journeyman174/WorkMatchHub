using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Configurare pentru proprietatea Id (cheia primara)
        builder.Property(e => e.Id)
            .IsRequired();

        builder.HasKey(x => x.Id); // Definire cheia primara

        // Configurare pentru proprietatea Name
        builder.Property(e => e.Name)
            .HasMaxLength(255)
            .IsRequired();

        // FullName - optional
        builder.Property(e => e.FullName)
            .HasMaxLength(255);

        // Configurare pentru Email
        builder.Property(e => e.Email)
            .HasMaxLength(255)
            .IsRequired();

        // Emailul trebuie sa fie unic
        builder.HasAlternateKey(e => e.Email);

        // Configurare pentru parola
        builder.Property(e => e.Password)
            .HasMaxLength(255)
            .IsRequired();

        // Rolul utilizatorului este salvat ca string in baza de date
        builder.Property(e => e.Role)
            .HasConversion(new EnumToStringConverter<UserRoleEnum>())
            .HasMaxLength(255)
            .IsRequired();

        // IsVerified este obligatoriu si are valoare implicita false
        builder.Property(e => e.IsVerified)
            .IsRequired()
            .HasDefaultValue(false);

        // Timestamps pentru creare si actualizare
        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .IsRequired();

        // Relatie One-to-Many: un utilizator poate trimite mai multe cereri de job
        builder.HasMany(u => u.JobRequests)
            .WithOne(jr => jr.User)
            .HasForeignKey(jr => jr.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Nu sterge utilizatorul daca exista cereri

        // Relatie One-to-Many: un utilizator poate crea mai multe oferte de job
        builder.HasMany(u => u.JobOffers)
            .WithOne(jo => jo.User)
            .HasForeignKey(jo => jo.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Nu sterge daca are oferte active

        // Relatie One-to-One: un utilizator poate avea o companie asociata
        builder.HasOne(u => u.Company)
            .WithOne(c => c.User)
            .HasForeignKey<Company>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Daca sterge utilizatorul, sterge si compania
    }
}
