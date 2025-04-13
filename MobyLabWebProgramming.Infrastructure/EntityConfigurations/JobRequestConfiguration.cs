using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

public class JobRequestConfiguration : IEntityTypeConfiguration<JobRequest>
{
    public void Configure(EntityTypeBuilder<JobRequest> builder)
    {
        builder.ToTable("JobRequests");

        builder.HasKey(jr => jr.Id);

        builder.Property(jr => jr.CoverLetter)
            .HasMaxLength(2000);

        builder.HasOne(jr => jr.JobOffer)
            .WithMany(jo => jo.JobRequests)
            .HasForeignKey(jr => jr.JobOfferId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(jr => jr.User)
            .WithMany(u => u.JobRequests)
            .HasForeignKey(jr => jr.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(jr => jr.JobAssignment)
            .WithOne(ja => ja.JobRequest)
            .HasForeignKey<JobAssignment>(ja => ja.JobRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
