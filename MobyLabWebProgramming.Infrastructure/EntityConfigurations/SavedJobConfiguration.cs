using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.Entities;

public class SavedJobConfiguration : IEntityTypeConfiguration<SavedJob>
{
    public void Configure(EntityTypeBuilder<SavedJob> builder)
    {
        builder.HasKey(sj => sj.Id); // Id din BaseEntity

        builder.HasIndex(sj => new { sj.UserId, sj.JobOfferId }).IsUnique(); // unicitate

        builder.Property(sj => sj.CreatedAt).IsRequired();
        builder.Property(sj => sj.UpdatedAt).IsRequired();

        builder.HasOne(sj => sj.User)
               .WithMany(u => u.SavedJobs)
               .HasForeignKey(sj => sj.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sj => sj.JobOffer)
               .WithMany(j => j.SavedByUsers)
               .HasForeignKey(sj => sj.JobOfferId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
