using AppTalk.Models.Models;

namespace AppTalk.API.DatabaseService.Interfaces;

public interface IUserDatabaseService
{
    public Task<User> CreateUserAsync(User user);
    
    public Task<User> GetUserAsync(Guid id);
    
    public Task<User> GetUserAsync(string username);

    public Task<bool> UsernameOrEmailExistsAsync(string username, string email);

    public Task<User> UpdateAsync(User user);
    public Task<bool> DeleteAsync(Guid id);
    public Task<bool> ExistsAsync(Guid id);
}