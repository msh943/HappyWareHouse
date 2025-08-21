using FluentAssertions;
using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Infrastructure.Data;
using HappyWarehouse.Infrastructure.Repositories;
using HappyWarehouse.Service.Service;
using Microsoft.EntityFrameworkCore;
namespace HappyWarehouse.Tests.Unit.Tests.Services
{
    public class AuthServiceTests
    {
        [Fact]
        public async Task Login_Should_Return_Token_For_Valid_Credentials()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .Options;

            using var db = new AppDbContext(options);

            var role = new Role { Id = 1, Name = "Admin" };
            var user = new User
            {
                Id = 1,
                Email = "admin@happywarehouse.com",
                FullName = "System Administrator",
                IsActive = true,
                Password = BCrypt.Net.BCrypt.HashPassword("P@ssw0rd!"),
                RoleId = 1,
                Role = role
            };

            db.Roles.Add(role);
            db.Users.Add(user);
            await db.SaveChangesAsync();


            var repo = new Repository<User>(db);
            var sut = new AuthService(repo);


            var result = await sut.ValidateUserAsync("admin@happywarehouse.com", "P@ssw0rd!");


            result.Should().NotBeNull();
            result!.Email.Should().Be("admin@happywarehouse.com");
            result.Role!.Name.Should().Be("Admin");
        }
    }
}
