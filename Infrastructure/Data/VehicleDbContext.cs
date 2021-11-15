using System;
using System.Reflection;
using Core.Entities;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class VehicleDbContext : IdentityDbContext<User, AppRole, Guid,
        IdentityUserClaim<Guid>, AppUserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>>
    {
        public VehicleDbContext(DbContextOptions<VehicleDbContext> options) : base(options)
        {

        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Location> Locations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}