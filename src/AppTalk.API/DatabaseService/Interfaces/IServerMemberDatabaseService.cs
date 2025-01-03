using AppTalk.Core.Interfaces.Database.Support;
using AppTalk.Models.Models;

namespace AppTalk.API.DatabaseService.Interfaces;

public interface IServerMemberDatabaseService : IAddSupport<ServerMember>
{
    public Task<bool> DeleteAsync(Guid userId, Guid serverId);
    public Task<bool> ExistsAsync(Guid userId, Guid serverId);
}