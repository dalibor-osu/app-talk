using AppTalk.API.DatabaseService.Interfaces;
using AppTalk.Core.Enums;
using AppTalk.Core.Extensions;
using AppTalk.Core.Models;
using AppTalk.Models.Models;

namespace AppTalk.API.Managers;

public class ServerMemberManager(IServerMemberDatabaseService serverMemberDatabaseService, IServerDatabaseService serverDatabaseService)
{
    public async Task<OptionalResponse<Server>> JoinAsync(Guid userId, Guid serverId)
    {
        serverId.EmptyCheck(nameof(serverId));

        var server = await serverDatabaseService.GetAsync(serverId);
        if (server == null)
        {
            return new OptionalResponse<Server>(OptionalErrorType.NotFound, "Server was not found");
        }
        
        bool alreadyJoined = await serverMemberDatabaseService.ExistsAsync(userId, serverId);
        if (alreadyJoined)
        {
            return new OptionalResponse<Server>(OptionalErrorType.AlreadyExists, "User already joined this server");
        }

        var serverMember = new ServerMember
        {
            UserId = userId,
            ServerId = server.Id
        };
        
        var result = await serverMemberDatabaseService.AddAsync(serverMember);
        if (result == null)
        {
            return new OptionalResponse<Server>(OptionalErrorType.ServiceError, "Could not add server member");
        }

        return new OptionalResponse<Server>(server);
    }
}