using Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(ur => ur.UserRoles)
                    .WithOne(u => u.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
        }
    }
}