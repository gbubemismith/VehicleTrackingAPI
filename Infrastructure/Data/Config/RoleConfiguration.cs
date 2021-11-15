using Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class RoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.HasMany(ur => ur.UserRoles)
                    .WithOne(u => u.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
        }
    }
}