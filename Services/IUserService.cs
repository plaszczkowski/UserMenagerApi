public interface IUserService
{
    Task<List<User>> GetUsersAsync(int page, int pageSize);
    Task<User> GetUserAsync(int id);
    Task<User> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(int id);
}
