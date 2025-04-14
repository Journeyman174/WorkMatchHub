using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.Database.EntityConfigurations;
public class SavedJobConfiguration : IEntityTypeConfiguration<SavedJob>
{
    public void Configure(EntityTypeBuilder<SavedJob> builder)
    {
        // Definire cheia primara pe baza Id-ului din BaseEntity
        builder.HasKey(sj => sj.Id);

        // Index compus unic pentru a nu permite salvarea aceluiasi job de doua ori de catre acelasi user
        builder.HasIndex(sj => new { sj.UserId, sj.JobOfferId }).IsUnique();

        // Campuri de audit obligatorii
        builder.Property(sj => sj.CreatedAt).IsRequired();
        builder.Property(sj => sj.UpdatedAt).IsRequired();

        // Relatie Many-to-One: un utilizator poate salva mai multe joburi
        builder.HasOne(sj => sj.User)
               .WithMany(u => u.SavedJobs)
               .HasForeignKey(sj => sj.UserId)
               .OnDelete(DeleteBehavior.Cascade); // Daca se sterge userul, se sterg si joburile salvate

        // Relatie Many-to-One: un job poate fi salvat de mai multi utilizatori
        builder.HasOne(sj => sj.JobOffer)
               .WithMany(j => j.SavedByUsers)
               .HasForeignKey(sj => sj.JobOfferId)
               .OnDelete(DeleteBehavior.Cascade); // Daca se sterge jobul, se sterg si intrarile salvate aferente
    }
}
