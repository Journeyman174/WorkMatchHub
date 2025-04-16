using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.Database.EntityConfigurations;
public class JobRequestConfiguration : IEntityTypeConfiguration<JobRequest>
{
    public void Configure(EntityTypeBuilder<JobRequest> builder)
    {
        // Specifica numele tabelului din baza de date
        builder.ToTable("JobRequests");

        // Definire cheia primara
        builder.HasKey(jr => jr.Id);

        // CoverLetter este optional
        builder.Property(jr => jr.CoverLetter)
            .HasMaxLength(2000);

        // Relatie Many-to-One: o cerere apartine unei singure oferte de job
        // O oferta poate avea mai multe cereri
        builder.HasOne(jr => jr.JobOffer)
            .WithMany(jo => jo.JobRequests)
            .HasForeignKey(jr => jr.JobOfferId)
            .OnDelete(DeleteBehavior.Cascade); // Daca se sterge oferta, se sterg si cererile

        // Relatie Many-to-One: o cerere este trimisa de un utilizator
        // Un utilizator poate trimite mai multe cereri
        builder.HasOne(jr => jr.User)
            .WithMany(u => u.JobRequests)
            .HasForeignKey(jr => jr.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Nu permite stergerea utilizatorului daca are cereri

        // Relatie One-to-One: o cerere poate fi asociata cu un assignment
        builder.HasOne(jr => jr.JobAssignment)
            .WithOne(ja => ja.JobRequest)
            .HasForeignKey<JobAssignment>(ja => ja.JobRequestId)
            .OnDelete(DeleteBehavior.Cascade); // Daca se sterge cererea, se sterge si assignment-ul aferent
    }
}