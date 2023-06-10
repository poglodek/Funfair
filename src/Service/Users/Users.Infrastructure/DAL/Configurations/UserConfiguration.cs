using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Core.Entities;
using Users.Core.ValueObjects;

namespace Users.Infrastructure.DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("U_Users");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Email)
            .HasConversion(c => c.Value, c => new EmailAddress(c))
            .IsRequired();
        
        builder.Property(x => x.FirstName)
            .HasConversion(c => c.Value, c => new Name(c))
            .IsRequired();
        
        builder.Property(x => x.LastName)
            .HasConversion(c => c.Value, c => new Name(c))
            .IsRequired();
        
        builder.Property(x => x.CreatedAt)
            .HasConversion(c => c.Value, c => new Date(c))
            .IsRequired();
        
        builder.Property(x => x.DateOfBirth)
            .HasConversion(c => c.Value, c => new Date(c))
            .IsRequired();

        builder.Property(x => x.Role)
            .HasConversion(c => c.Name, c => new Role(c))
            .IsRequired();
        
        builder.Property(x => x.Password)
            .HasConversion(c => c.Value, c => new Password(c))
            .IsRequired();
    }
}