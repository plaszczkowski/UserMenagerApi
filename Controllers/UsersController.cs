using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUsers(int page = 1, int pageSize = 10)
    {
        _logger.LogInformation("Getting users - Page: {Page}, PageSize: {PageSize}", page, pageSize);

        try
        {
            var users = await _userService.GetUsersAsync(page, pageSize);
            _logger.LogInformation("Successfully retrieved {UserCount} users", users.Count);
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving users.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetUser(int id)
    {
        _logger.LogInformation("Getting user with ID: {UserId}", id);

        try
        {
            var user = await _userService.GetUserAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully retrieved user with ID: {UserId}", id);
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving user with ID: {UserId}.", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateUser(User user)
    {
        _logger.LogInformation("Creating a new user with name: {UserName}", user.Name);

        if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Email))
        {
            _logger.LogWarning("Invalid input provided for user creation.");
            return BadRequest("Invalid input");
        }

        try
        {
            var createdUser = await _userService.CreateUserAsync(user);
            _logger.LogInformation("Successfully created user with ID: {UserId}", createdUser.Id);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating user.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateUser(int id, User user)
    {
        _logger.LogInformation("Updating user with ID: {UserId}", id);

        if (id != user.Id)
        {
            _logger.LogWarning("User ID mismatch during update. Provided ID: {ProvidedId}, Expected ID: {ExpectedId}", id, user.Id);
            return BadRequest("User ID mismatch");
        }

        try
        {
            var updatedUser = await _userService.UpdateUserAsync(user);
            _logger.LogInformation("Successfully updated user with ID: {UserId}", user.Id);
            return Ok(updatedUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating user with ID: {UserId}.", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteUser(int id)
    {
        _logger.LogInformation("Deleting user with ID: {UserId}", id);

        try
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("User with ID {UserId} not found for deletion.", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully deleted user with ID: {UserId}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting user with ID: {UserId}.", id);
            return StatusCode(500, "Internal server error");
        }
    }
}