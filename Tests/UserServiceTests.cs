using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class UserServiceTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _userService = new UserService(new AppDbContext(new DbContextOptions<AppDbContext>()));
    }

    [Fact]
    public async Task GetUsersAsync_ReturnsCorrectNumberOfUsers()
    {
        var users = new List<User> { new User { Id = 1, Name = "John Doe", Email = "john@example.com" } };
        _userServiceMock.Setup(service => service.GetUsersAsync(1, 10)).ReturnsAsync(users);
        var result = await _userService.GetUsersAsync(1, 10);
        Assert.Single(result);
    }

    [Fact]
    public async Task CreateUserAsync_AddsUserSuccessfully()
    {
        var user = new User { Id = 2, Name = "Jane Doe", Email = "jane@example.com" };
        _userServiceMock.Setup(service => service.CreateUserAsync(user)).ReturnsAsync(user);
        var createdUser = await _userService.CreateUserAsync(user);
        Assert.Equal("Jane Doe", createdUser.Name);
    }
}
