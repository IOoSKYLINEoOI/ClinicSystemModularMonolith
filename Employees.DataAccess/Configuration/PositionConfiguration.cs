using Employees.Core.Enums;
using Employees.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Employees.DataAccess.Configuration;

public class PositionConfiguration : IEntityTypeConfiguration<PositionEntity>
{
    public void Configure(EntityTypeBuilder<PositionEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(x => x.Name).IsRequired();

        var positions = Enum.GetValues<PositionEnum>()
            .Select(p => new PositionEntity
            {
                Id = (int)p,
                Name = p.ToString(),
            });
        
        builder.HasData(positions);
    }
}