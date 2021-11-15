using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class VehicleDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<User> userManager, RoleManager<AppRole> roleManager)
        {
            if (userManager.Users.Any()) return;

            var path = Path.GetDirectoryName(Environment.CurrentDirectory);
            var userData = await File.ReadAllTextAsync(path + @"/Infrastructure/Data/UserData.json");

            var users = JsonSerializer.Deserialize<List<User>>(userData);
            // var users = GetPreConfiguredUsers();

            var roles = new List<AppRole>
            {
                new AppRole{ Name = Role.Admin},
                new AppRole{ Name = Role.User}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }


            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, Role.User);
            }

            var adminUser = new User
            {
                Email = "admin@test.com",
                UserName = "admin"
            };

            await userManager.CreateAsync(adminUser, "Pa$$w0rd");
            await userManager.AddToRolesAsync(adminUser, new[] { Role.Admin });

        }

        //use this for docker. The retries helps so the host can be ready
        public static async Task SeedAsync(VehicleDbContext vehicleDbContext, UserManager<User> userManager, RoleManager<AppRole> roleManager, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryforAvailability = retry.Value;

            try
            {
                await vehicleDbContext.Database.MigrateAsync();

                if (userManager.Users.Any()) return;

                var users = GetPreConfiguredUsers();

                var roles = new List<AppRole>
                {
                    new AppRole{ Name = Role.Admin},
                    new AppRole{ Name = Role.User}
                };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }


                foreach (var user in users)
                {
                    user.UserName = user.UserName.ToLower();
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                    await userManager.AddToRoleAsync(user, Role.User);
                }

                var adminUser = new User
                {
                    Email = "admin@test.com",
                    UserName = "admin"
                };

                await userManager.CreateAsync(adminUser, "Pa$$w0rd");
                await userManager.AddToRolesAsync(adminUser, new[] { Role.Admin });

            }
            catch (Exception ex)
            {
                if (retryforAvailability < 3)
                {
                    retryforAvailability++;
                    var log = loggerFactory.CreateLogger<VehicleDbContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(vehicleDbContext, userManager, roleManager, loggerFactory, retryforAvailability);
                }
            }

        }

        private static IEnumerable<User> GetPreConfiguredUsers()
        {
            return new List<User>()
            {
                new User() { UserName = "gsmith_walter", Email = "gsmith@test.com"},
                new User() { UserName = "peakstest", Email = "seven_peaks@test.com"},
                new User() { UserName = "james_test", Email = "james@test.com"}

            };
        }
    }
}