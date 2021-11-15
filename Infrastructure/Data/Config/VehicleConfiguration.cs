using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasKey(p => p.Id);
            // builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.VehicleName).IsRequired();

            builder.HasOne(p => p.Device)
                    .WithOne(p => p.Vehicle)
                    .HasForeignKey<Device>(p => p.VehicleId);
        }
    }
}