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
        builder.Property(e => e.Id)
            .IsRequired();
        builder.HasKey(x => x.Id);

        builder.Property(e => e.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.FullName)
            .HasMaxLength(255);

        builder.Property(e => e.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasAlternateKey(e => e.Email);

        builder.Property(e => e.Password)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.Role)
            .HasConversion(new EnumToStringConverter<UserRoleEnum>())
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.IsVerified)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .IsRequired();

        // Relații adiționale
        builder.HasMany(u => u.JobRequests)
            .WithOne(jr => jr.User)
            .HasForeignKey(jr => jr.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.JobOffers)
            .WithOne(jo => jo.User)
            .HasForeignKey(jo => jo.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Company)
            .WithOne(c => c.User)
            .HasForeignKey<Company>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
