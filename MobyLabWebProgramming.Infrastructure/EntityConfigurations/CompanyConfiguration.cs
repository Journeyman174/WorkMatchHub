using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.Database.EntityConfigurations;
public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        // Specifica numele tabelului din baza de date
        builder.ToTable("Companies");

        // Definire cheia primara
        builder.HasKey(c => c.Id);

        // Campul Name este obligatoriu
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Campul Description este optional
        builder.Property(c => c.Description)
            .HasMaxLength(500);

        // Campul Location este optional
        builder.Property(c => c.Location)
            .HasMaxLength(200);

        // Relatie Many-to-One: fiecare companie este asociata unui utilizator
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Nu permite stergerea utilizatorului daca are companie
    }
}
