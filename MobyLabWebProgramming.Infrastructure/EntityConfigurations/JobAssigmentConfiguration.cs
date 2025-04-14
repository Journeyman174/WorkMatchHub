using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.Database.EntityConfigurations;
public class JobAssignmentConfiguration : IEntityTypeConfiguration<JobAssignment>
{
    public void Configure(EntityTypeBuilder<JobAssignment> builder)
    {
        // Specifica numele tabelului in baza de date
        builder.ToTable("JobAssignments");

        // Definire cheia primara
        builder.HasKey(ja => ja.Id);

        // Campul AssignedAt este obligatoriu (data atribuirii jobului)
        builder.Property(ja => ja.AssignedAt)
            .IsRequired();

        // Relatie One-to-One cu JobRequest
        // Fiecare atribuire este legata de o cerere specifica
        builder.HasOne(ja => ja.JobRequest)
            .WithOne(jr => jr.JobAssignment)
            .HasForeignKey<JobAssignment>(ja => ja.JobRequestId)
            .OnDelete(DeleteBehavior.Cascade); // Daca se sterge cererea, se sterg si atribuirile aferente

        // Relatie Many-to-One cu JobOffer
        // O oferta poate avea mai multe atribuiri
        builder.HasOne(ja => ja.JobOffer)
            .WithMany(jo => jo.JobAssignments)
            .HasForeignKey(ja => ja.JobOfferId)
            .OnDelete(DeleteBehavior.Cascade); // Daca se sterge oferta, se sterg si atribuirile

        // Relatie Many-to-One cu User
        // Legare cu utilizatorul caruia i s-a atribuit jobul
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(ja => ja.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Nu permite stergerea utilizatorului daca are joburi atribuite
    }
}