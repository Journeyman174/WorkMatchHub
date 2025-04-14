using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.Database.EntityConfigurations;
public class JobOfferConfiguration : IEntityTypeConfiguration<JobOffer>
{
    public void Configure(EntityTypeBuilder<JobOffer> builder)
    {
        // Specifica numele tabelului in baza de date
        builder.ToTable("JobOffers");

        // Definire cheia primara
        builder.HasKey(j => j.Id);

        // Titlul jobului este obligatoriu
        builder.Property(j => j.Title)
            .IsRequired()
            .HasMaxLength(100);

        // Descrierea este obligatorie
        builder.Property(j => j.Description)
            .IsRequired()
            .HasMaxLength(1000);

        // Salariul este un camp decimal cu doua zecimale
        builder.Property(j => j.Salary)
            .HasColumnType("decimal(18,2)");

        // Relatie Many-to-One: fiecare oferta este asociata unei companii
        // O companie poate avea mai multe oferte
        builder.HasOne(j => j.Company)
            .WithMany(c => c.JobOffers)
            .HasForeignKey(j => j.CompanyId)
            .OnDelete(DeleteBehavior.Cascade); // Daca se sterge compania, se sterg si ofertele

        // Relatie Many-to-One: fiecare oferta este asociata unui recruiter
        builder.HasOne(j => j.User)
            .WithMany(u => u.JobOffers) // Utilizatorul poate crea mai multe oferte
            .HasForeignKey(j => j.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Nu permite stergerea utilizatorului daca are oferte active
    }
}
