using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Core.Enums;
using Users.Domain.Entities;

namespace Users.Domain.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(x => x.Description)
            .HasMaxLength(255);

        builder.HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity<RolePermissionEntity>(
                p => p.HasOne<PermissionEntity>().WithMany().HasForeignKey(rp => rp.PermissionId),
                r => r.HasOne<RoleEntity>().WithMany().HasForeignKey(rp => rp.RoleId)
            );
    }
}