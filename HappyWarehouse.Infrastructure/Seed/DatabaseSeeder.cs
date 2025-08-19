using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HappyWarehouse.Infrastructure.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            if (!await db.Roles.AnyAsync())
            {
                db.Roles.AddRange(
                    new Role { Name = "Admin" },
                    new Role { Name = "Management" },
                    new Role { Name = "Auditor" }
                );
                await db.SaveChangesAsync();
            }

            if (!await db.Countries.AnyAsync())
            {
                db.Countries.AddRange(
                    new Country { Name = "Jordan", Code = "JO" },
                    new Country { Name = "Saudi Arabia", Code = "SA" },
                    new Country { Name = "United Arab Emirates", Code = "AE" }
                );
                await db.SaveChangesAsync();
            }

            var adminRole = await db.Roles.FirstAsync(r => r.Name == "Admin");
            if (!await db.Users.AnyAsync(u => u.Email == "admin@happywarehouse.com"))
            {
                db.Users.Add(new User
                {
                    Email = "admin@happywarehouse.com",
                    FullName = "System Administrator",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("P@ssw0rd"),
                    RoleId = adminRole.Id,
                    IsActive = true
                });
                await db.SaveChangesAsync();
            }
        }
    }
}
