using UserTransactionSystem.Domain.Entities;
using UserTransactionSystem.Domain.Enums;
using UserTransactionSystem.Infrastructure.Data;
using UserTransactionSystem.Services.Services;
using Microsoft.EntityFrameworkCore;

namespace UserTransactionSystem.UnitTests.Services
{
    public class ReportingServiceTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public ReportingServiceTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Seed the database
            using (var context = new ApplicationDbContext(_options))
            {
                SeedDatabase(context);
            }
        }

        private void SeedDatabase(ApplicationDbContext context)
        {
            // Add users
            var user1 = new User { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "UserOne", CreatedAt = DateTime.UtcNow.AddDays(-30) };
            var user2 = new User { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "UserTwo", CreatedAt = DateTime.UtcNow.AddDays(-20) };
            context.Users.AddRange(user1, user2);

            // Add transactions
            var transactions = new List<Domain.Entities.Transaction>
            {
                new Domain.Entities.Transaction { Id = 1, Amount = 100, TransactionType = TransactionTypeEnum.Debit, CreatedAt = DateTime.UtcNow.AddDays(-15) },
                new Domain.Entities.Transaction { Id = 2, Amount = 200, TransactionType = TransactionTypeEnum.Debit, CreatedAt = DateTime.UtcNow.AddDays(-14) },
                new Domain.Entities.Transaction { Id = 3, Amount = 300, TransactionType = TransactionTypeEnum.Debit, CreatedAt = DateTime.UtcNow.AddDays(-13) },
                new Domain.Entities.Transaction { Id = 4, Amount = 400, TransactionType = TransactionTypeEnum.Credit, CreatedAt = DateTime.UtcNow.AddDays(-12) },
                new Domain.Entities.Transaction { Id = 5, Amount = 500, TransactionType = TransactionTypeEnum.Credit, CreatedAt = DateTime.UtcNow.AddDays(-11) }
            };
            context.Transactions.AddRange(transactions);

            context.SaveChanges();
        }

        [Fact]
        public async Task GetHighVolumeTransactionsAsync_DateFiltering_ReturnsCorrectTransactionIds()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            var service = new ReportingService(context);
            var from = DateTime.UtcNow.AddDays(-14); // Only include transactions from day -14 and newer
            var to = DateTime.UtcNow.AddDays(-11);   // Only include transactions up to day -11
            var thresholdAmount = 200; // Include all transactions in the date range above this amount

            // Act
            var result = await service.GetHighVolumeTransactionsAsync(from, to, thresholdAmount);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Transactions.Count); // Should return transactions 3, 4, and 5
            Assert.True(result.Transactions.FirstOrDefault(x => x.Id == 3) != null);
            Assert.True(result.Transactions.FirstOrDefault(x => x.Id == 4) != null);
            Assert.True(result.Transactions.FirstOrDefault(x => x.Id == 5) != null);
        }
    }
}
