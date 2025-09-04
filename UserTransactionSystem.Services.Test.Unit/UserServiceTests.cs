using UserTransactionSystem.Domain.Entities;
using UserTransactionSystem.Infrastructure.Repositories;
using UserTransactionSystem.Infrastructure.UnitOfWork;
using UserTransactionSystem.Services.DTOs;
using UserTransactionSystem.Services.Services;
using AutoMapper;
using Moq;

namespace UserTransactionSystem.Services.Test.Unit
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockUserRepository = new Mock<IRepository<User>>();
            _mockUnitOfWork.Setup(uow => uow.Users).Returns(_mockUserRepository.Object);
            _userService = new UserService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow },
                new User { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow }
            };

            var userDtos = new List<UserDto>
            {
                new UserDto { Id = users[0].Id, CreatedAt = users[0].CreatedAt },
                new UserDto { Id = users[1].Id, CreatedAt = users[1].CreatedAt }
            };

            _mockUserRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<UserDto>>(users)).Returns(userDtos);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal(users[0].Id, result.First().Id);
            Assert.Equal(users[1].Id, result.Last().Id);
            _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_WithValidId_ShouldReturnUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, CreatedAt = DateTime.UtcNow };
            var userDto = new UserDto { Id = userId, CreatedAt = user.CreatedAt };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);
            _mockMapper.Setup(mapper => mapper.Map<UserDto>(user)).Returns(userDto);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.Null(result);
            _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldCreateAndReturnUser()
        {
            // Arrange
            var createUserDto = new CreateUserDto();
            var userId = Guid.NewGuid();
            var dateCreated = DateTime.UtcNow;

            var user = new User { Id = userId, CreatedAt = dateCreated };
            var userDto = new UserDto { Id = userId, CreatedAt = dateCreated };

            _mockMapper.Setup(mapper => mapper.Map<User>(createUserDto)).Returns(user);
            _mockMapper.Setup(mapper => mapper.Map<UserDto>(user)).Returns(userDto);

            // Act
            var result = await _userService.CreateUserAsync(createUserDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            _mockUserRepository.Verify(repo => repo.AddAsync(user), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_WithValidId_ShouldUpdateAndReturnUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updateUserDto = new UpdateUserDto();
            var user = new User { Id = userId, CreatedAt = DateTime.UtcNow };
            var userDto = new UserDto { Id = userId, CreatedAt = user.CreatedAt };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);
            _mockMapper.Setup(mapper => mapper.Map<UserDto>(user)).Returns(userDto);

            // Act
            var result = await _userService.UpdateUserAsync(userId, updateUserDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            _mockUserRepository.Verify(repo => repo.Update(user), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updateUserDto = new UpdateUserDto();

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.UpdateUserAsync(userId, updateUserDto);

            // Assert
            Assert.Null(result);
            _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
            _mockUserRepository.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteUserAsync_WithValidId_ShouldDeleteAndReturnTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, CreatedAt = DateTime.UtcNow };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.DeleteUserAsync(userId);

            // Assert
            Assert.True(result);
            _mockUserRepository.Verify(repo => repo.Remove(user), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_WithInvalidId_ShouldReturnFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.DeleteUserAsync(userId);

            // Assert
            Assert.False(result);
            _mockUserRepository.Verify(repo => repo.Remove(It.IsAny<User>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Never);
        }
    }
}
