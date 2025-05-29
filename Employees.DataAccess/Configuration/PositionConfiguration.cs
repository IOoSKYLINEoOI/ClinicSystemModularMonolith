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

        var positions = Enum.GetValues<Position>()
            .Select(p => new PositionEntity
            {
                Id = (int)p,
                Name = p.ToString(),
            });
        
        builder.HasData(positions);
    }
}