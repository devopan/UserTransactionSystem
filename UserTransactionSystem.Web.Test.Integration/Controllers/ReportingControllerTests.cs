using System.Net.Http.Json;
using UserTransactionSystem.Services.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;

namespace UserTransactionSystem.Web.Test.Integration.Controllers
{
    public class ReportingControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public ReportingControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task GetTotalTransactionsAmountByUser_ReturnsSuccessAndReport()
        {
            // Act
            var response = await _client.GetAsync("/api/reporting/users/transactionAmountTotal");

            // Assert
            response.EnsureSuccessStatusCode();
            var report = await response.Content.ReadFromJsonAsync<List<UserTotalAmountReportDto>>();
            Assert.NotNull(report);
            Assert.True(report.Count > 0);
        }

        [Fact]
        public async Task GetTotalTransactionsAmountByType_ReturnsSuccessAndReport()
        {
            // Act
            var response = await _client.GetAsync("/api/reporting/transactionTypes/transactionAmountTotal");

            // Assert
            response.EnsureSuccessStatusCode();
            var report = await response.Content.ReadFromJsonAsync<List<TransactionTypeTotalAmountReportDto>>();
            Assert.NotNull(report);
            Assert.True(report.Count > 0);
        }

        [Fact]
        public async Task GetHighVolumeTransactions_WithValidParameters_ReturnsSuccessAndReport()
        {
            // Arrange
            var today = DateTime.UtcNow;
            var fromDate = today.AddDays(-30).ToString("dd/MM/yyyy");
            var toDate = today.ToString("dd/MM/yyyy");

            // Act
            var response = await _client.GetAsync($"/api/reporting/high-volume-transactions?from={fromDate}&to={toDate}&limit=0");

            // Assert
            response.EnsureSuccessStatusCode();
            var report = await response.Content.ReadFromJsonAsync<HighVolumeTransactionReportDto>();
            Assert.NotNull(report);
            Assert.NotNull(report.Transactions);
        }

        [Fact]
        public async Task GetHighVolumeTransactions_WithInvalidDateFormat_ReturnsBadRequest()
        {
            // Act
            var response = await _client.GetAsync("/api/reporting/high-volume-transactions?from=2023-01-01&to=2023-12-31&limit=5");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
