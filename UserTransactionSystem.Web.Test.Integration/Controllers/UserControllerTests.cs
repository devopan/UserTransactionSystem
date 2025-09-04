using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using UserTransactionSystem.Services.DTOs;

namespace UserTransactionSystem.Web.Test.Integration.Controllers
{
    public class UsersControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public UsersControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        private void Setup()
        {
            _factory.ResetDatabase();
        }

        [Fact]
        public async Task GetAll_ReturnsSuccessAndAllUsers()
        {
            // Arrange
            Setup();

            // Act
            var response = await _client.GetAsync("/api/users");

            // Assert
            response.EnsureSuccessStatusCode();
            var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
            Assert.NotNull(users);
            Assert.Equal(2, users.Count);
        }

        [Fact]
        public async Task GetById_WithValidId_ReturnsSuccessAndUser()
        {
            // Arrange
            var validId = "11111111-1111-1111-1111-111111111111";

            // Act
            var response = await _client.GetAsync($"/api/users/{validId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var user = await response.Content.ReadFromJsonAsync<UserDto>();
            Assert.NotNull(user);
            Assert.Equal(Guid.Parse(validId), user.Id);
        }

        [Fact]
        public async Task GetById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var invalidId = "99999999-9999-9999-9999-999999999999";

            // Act
            var response = await _client.GetAsync($"/api/users/{invalidId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_ReturnsSuccessAndCreatedUser()
        {
            // Arrange
            var createUserDto = new CreateUserDto() { Name = "TestUser" };
            var content = new StringContent(
                JsonSerializer.Serialize(createUserDto),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/api/users", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var user = await response.Content.ReadFromJsonAsync<UserDto>();
            Assert.NotNull(user);
            Assert.NotEqual(Guid.Empty, user.Id);
        }

        [Fact]
        public async Task Update_WithValidId_ReturnsSuccessAndUpdatedUser()
        {
            // Arrange
            Setup();
            var validId = "22222222-2222-2222-2222-222222222222";
            var updateUserDto = new UpdateUserDto() { Name = "InvalidUser" };
            var content = new StringContent(
                JsonSerializer.Serialize(updateUserDto),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PutAsync($"/api/users/{validId}", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var user = await response.Content.ReadFromJsonAsync<UserDto>();
            Assert.NotNull(user);
            Assert.Equal(Guid.Parse(validId), user.Id);
        }

        [Fact]
        public async Task Update_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var invalidId = "99999999-9999-9999-9999-999999999999";
            var updateUserDto = new UpdateUserDto() { Name = "InvalidUser" };
            var content = new StringContent(
                JsonSerializer.Serialize(updateUserDto),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PutAsync($"/api/users/{invalidId}", content);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var validId = "22222222-2222-2222-2222-222222222222";

            // Act
            var response = await _client.DeleteAsync($"/api/users/{validId}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Delete_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var invalidId = "99999999-9999-9999-9999-999999999999";

            // Act
            var response = await _client.DeleteAsync($"/api/users/{invalidId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
