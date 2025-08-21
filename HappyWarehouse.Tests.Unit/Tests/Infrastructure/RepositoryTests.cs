using FluentAssertions;
using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Infrastructure.Data;
using HappyWarehouse.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HappyWarehouse.Tests.Unit.Tests.Infrastructure
{
    public class RepositoryTests
    {


        private static (AppDbContext db, SqliteConnection conn) NewSqliteDb()
        {
            var conn = new SqliteConnection("DataSource=:memory:");
            conn.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(conn)
                .Options;

            var db = new AppDbContext(options);
            db.Database.EnsureCreated();

            return (db, conn);
        }

        [Fact]
        public async Task Repository_CRUD_With_Sqlite_InMemory_Works()
        {

            var (db, conn) = NewSqliteDb();
            try
            {
                var country = new Country { Id = 1, Name = "Jordan", Code = "JO" };
                db.Countries.Add(country);
                await db.SaveChangesAsync();

                // Now add a warehouse with the required CountryId set
                var wh = new Warehouse
                {
                    Name = "Amman Central",
                    Address = "123 King Abdullah II St",
                    City = "Amman",
                    CountryId = 1
                };

                var repo = new Repository<Warehouse>(db);

                // CREATE
                await repo.AddAsync(wh);

                // READ
                var fromDb = await repo.GetByIdAsync(wh.Id);
                Assert.NotNull(fromDb);

                // UPDATE
                fromDb!.Name = "Updated";
                await repo.UpdateAsync(fromDb);

                // DELETE
                await repo.DeleteAsync(fromDb.Id);

                var shouldBeNull = await repo.GetByIdAsync(fromDb.Id);
                Assert.Null(shouldBeNull);
            }
            finally
            {
                await conn.DisposeAsync();
                await db.DisposeAsync();
            }
        }
    }
}
