using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MORENT.Domain.Entities.Dbo;
using MORENT.Domain.Entities.Security;
using MORENT.Domain.Enums;
using MORENT.Infrastructure.Persistence;

namespace MORENT.Infrastructure.Persistence
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // 1. Apply any pending migrations automatically
            if (context.Database.IsSqlServer())
            {
                await context.Database.MigrateAsync();
            }

            // 2. Seed Roles
            if (!await context.Roles.AnyAsync())
            {
                await context.Roles.AddRangeAsync(
                    Enum.GetValues<SystemRoleEnum>().Select(e => new Role { Id = (int)e, Name = e.ToString() }));
                await context.SaveChangesAsync();
            }

            // 3. Seed Enums dynamically into Lookup Tables
            if (!await context.RentalStatuses.AnyAsync())
            {
                await context.RentalStatuses.AddRangeAsync(
                    Enum.GetValues<RentalStatusEnum>().Select(e => new RentalStatus { Id = (int)e, Name = e.ToString() }));
                await context.SaveChangesAsync();
            }

            if (!await context.SteeringTypes.AnyAsync())
            {
                await context.SteeringTypes.AddRangeAsync(
                    Enum.GetValues<SteeringTypeEnum>().Select(e => new SteeringType { Id = (int)e, Name = e.ToString() }));
                await context.SaveChangesAsync();
            }

            if (!await context.CarTypes.AnyAsync())
            {
                await context.CarTypes.AddRangeAsync(
                    Enum.GetValues<CarTypeEnum>().Select(e => new CarType { Id = (int)e, Name = e.ToString() }));
                await context.SaveChangesAsync();
            }

            if (!await context.FuelTypes.AnyAsync())
            {
                await context.FuelTypes.AddRangeAsync(
                    Enum.GetValues<FuelTypeEnum>().Select(e => new FuelType { Id = (int)e, Name = e.ToString() }));
                await context.SaveChangesAsync();
            }

            if (!await context.PaymentMethods.AnyAsync())
            {
                await context.PaymentMethods.AddRangeAsync(
                    Enum.GetValues<PaymentMethodEnum>().Select(e => new PaymentMethod { Id = (int)e, Name = e.ToString() }));
                await context.SaveChangesAsync();
            }

            // 4. Seed Suggested Locations
            if (!await context.Locations.AnyAsync())
            {
                await context.Locations.AddRangeAsync(
                    new Location { Id = 1, Name = "New Cairo" },
                    new Location { Id = 2, Name = "Giza" },
                    new Location { Id = 3, Name = "Ramses" },
                    new Location { Id = 4, Name = "Maadi" },
                    new Location { Id = 5, Name = "Helwan" },
                    new Location { Id = 6, Name = "Nasr City" },
                    new Location { Id = 7, Name = "6th of October" },
                    new Location { Id = 8, Name = "Shubra El Kheima" },
                    new Location { Id = 9, Name = "Haram" },
                    new Location { Id = 10, Name = "Dokki" },
                    new Location { Id = 11, Name = "Mohandessin" },
                    new Location { Id = 12, Name = "Zamalek" },
                    new Location { Id = 13, Name = "Garden City" },
                    new Location { Id = 14, Name = "Agouza" },
                    new Location { Id = 15, Name = "Faisal" }
                );
                await context.SaveChangesAsync();
            }

            // 5. Seed Admin Account
            if (!await context.Users.AnyAsync(u => u.Username == "admin"))
            {
                var admin = new User
                {
                    Id = Guid.NewGuid(),
                    Username = "admin",
                    FirstName = "Admin",
                    LastName = string.Empty,
                    Email = string.Empty,
                    PhoneNumber = string.Empty,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("@123"),
                    IsActive = true,
                    RoleId = 1
                };

                await context.Users.AddAsync(admin);
                await context.SaveChangesAsync();
            }
        }
    }
}