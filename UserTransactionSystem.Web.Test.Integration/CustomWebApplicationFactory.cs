using UserTransactionSystem.Domain.Entities;
using UserTransactionSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UserTransactionSystem.Web.Test.Integration
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the app's ApplicationDbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add ApplicationDbContext using an in-memory database for testing
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                // Build the service provider
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database context
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();

                    // Ensure the database is created
                    db.Database.EnsureCreated();

                    try
                    {
                        // Seed the database with test data
                        InitializeDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the database. Error: {Message}", ex.Message);
                    }
                }
            });
        }

        private void InitializeDbForTests(ApplicationDbContext db)
        {
            // Add test data
            db.Users.AddRange(
                new User
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "UserOne",
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new User
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "UserTwo",
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                }
            );

            db.Transactions.AddRange(
                new Domain.Entities.Transaction
                {
                    Id = 1,
                    Amount = 100.50m,
                    TransactionType = Domain.Enums.TransactionTypeEnum.Debit,
                    CreatedAt = DateTime.UtcNow.AddDays(-8)
                },
                new Domain.Entities.Transaction
                {
                    Id = 2,
                    Amount = 50.25m,
                    TransactionType = Domain.Enums.TransactionTypeEnum.Credit,
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                }
            );

            db.SaveChanges();
        }

        public void ResetDatabase()
        {
            using (var scope = Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                InitializeDbForTests(db);
            }
        }
    }
}
