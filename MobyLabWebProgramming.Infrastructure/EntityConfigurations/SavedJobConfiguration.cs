using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations
{
    public class SavedJobConfiguration : IEntityTypeConfiguration<SavedJob>
    {
        public void Configure(EntityTypeBuilder<SavedJob> builder)
        {
            builder.HasKey(sj => new { sj.UserId, sj.JobOfferId });

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
}
