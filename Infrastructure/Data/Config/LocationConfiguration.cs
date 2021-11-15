using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasKey(p => p.Id);
            // builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Latitude).HasColumnType("decimal(10, 8)").IsRequired();
            builder.Property(p => p.Longitude).HasColumnType("decimal(11, 8)").IsRequired();
        }
    }
}