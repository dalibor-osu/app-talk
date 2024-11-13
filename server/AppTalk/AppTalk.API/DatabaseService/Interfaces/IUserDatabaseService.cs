using AppTalk.Core.Interfaces.Database.Support;
using AppTalk.Models.Models;

namespace AppTalk.API.DatabaseService.Interfaces;

public interface IUserDatabaseService : ICrudSupport<User>, IExistsSupport
{
    public Task<User> GetByUsernameAsync(string username);

    public Task<bool> UsernameOrEmailExistsAsync(string username, string email);
}