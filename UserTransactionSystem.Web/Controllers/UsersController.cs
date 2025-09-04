using Microsoft.AspNetCore.Mvc;
using UserTransactionSystem.Services.DTOs;
using UserTransactionSystem.Services.Interfaces;

namespace UserTransactionSystem.Web.Controllers
{
    public class UsersController : BaseController<UserDto, CreateUserDto, Guid>
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Updates an existing User by its id.
        /// </summary>
        /// <param name="id">The id of the User to be updated.</param>
        /// <param name="updateUserDto">The new data of the User to be updated.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserDto updateUserDto)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, updateUserDto);
            if (updatedUser == null)
                return NotFound();

            return Ok(updatedUser);
        }

        /// <summary>
        /// Deletes a User by its id.
        /// </summary>
        /// <param name="id">The id of the User to be deleted.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        protected override async Task<UserDto> ReadSingleAsync(Guid id)
        {
            return await _userService.GetUserByIdAsync(id);
        }

        protected override async Task<UserDto> CreateAsync(CreateUserDto createDto)
        {
            return await _userService.CreateUserAsync(createDto);
        }

        protected override Guid GetEntityId(UserDto entity)
        {
            return entity.Id;
        }

        protected override async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            return await _userService.GetAllUsersAsync();
        }
    }
}