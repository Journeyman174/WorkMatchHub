using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Infrastructure.Database.EntityConfigurations;

public class JobAssignmentConfiguration : IEntityTypeConfiguration<JobAssignment>
{
    public void Configure(EntityTypeBuilder<JobAssignment> builder)
    {
        builder.ToTable("JobAssignments");

        builder.HasKey(ja => ja.Id);

        builder.Property(ja => ja.AssignedAt)
            .IsRequired();

        builder.HasOne(ja => ja.JobRequest)
            .WithOne(jr => jr.JobAssignment)
            .HasForeignKey<JobAssignment>(ja => ja.JobRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ja => ja.JobOffer)
            .WithMany(jo => jo.JobAssignments)
            .HasForeignKey(ja => ja.JobOfferId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(ja => ja.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
