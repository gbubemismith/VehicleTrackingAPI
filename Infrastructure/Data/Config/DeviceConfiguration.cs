using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.HasKey(p => p.Id);
            // builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.DeviceName).IsRequired();
        }
    }
}