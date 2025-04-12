using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

/// <summary>
/// This is the entity configuration for the User entity, generally the Entity Framework will figure out most of the configuration but,
/// for some specifics such as unique keys, indexes and foreign keys it is better to explicitly specify them.
/// Note that the EntityTypeBuilder implements a Fluent interface, meaning it is a highly declarative interface using method-chaining.
/// </summary>
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

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .IsRequired();

        // Relații adiționale
        builder.HasMany(u => u.JobRequests)
            .WithOne(jr => jr.JobSeeker)
            .HasForeignKey(jr => jr.JobSeekerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.JobOffers)
            .WithOne(jo => jo.User)
            .HasForeignKey(jo => jo.UserId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
