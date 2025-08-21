using HappyWarehouse.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace HappyWarehouse.Tests.Integration
{
    public class ApiFactory : WebApplicationFactory<Program>
    {
        private SqliteConnection? _conn;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {

                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor != null) services.Remove(descriptor);


                _conn = new SqliteConnection("DataSource=:memory:");
                _conn.Open();

                services.AddDbContext<AppDbContext>(o => o.UseSqlite(_conn));


                using var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();

                if (!db.Roles.Any()) db.Roles.Add(new HappyWarehouse.Domain.Entities.Role { Name = "Admin" });
                db.SaveChanges();
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _conn?.Dispose();
        }
    }
}
