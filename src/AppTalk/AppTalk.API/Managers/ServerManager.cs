using AppTalk.API.DatabaseService.Interfaces;
using AppTalk.Core.Enums;
using AppTalk.Core.Extensions;
using AppTalk.Core.Models;
using AppTalk.Core.Validation;
using AppTalk.Models.Convertors;
using AppTalk.Models.DataTransferObjects;
using AppTalk.Models.Models;

namespace AppTalk.API.Managers;

public class ServerManager(IServerDatabaseService serverDatabaseService, RoomManager roomManager)
{
    public async Task<OptionalResponse<Server>> AddAsync(Guid userId, ServerDto serverDto)
    {
        userId.EmptyCheck(nameof(userId));
        
        var server = serverDto.ToModel();
        server.UserId = userId;

        var validationResult = Validator.Validate(server);
        if (!validationResult.IsValid)
        {
            return new OptionalResponse<Server>(validationResult);
        }
        
        server = await serverDatabaseService.AddAsync(server);
        if (server == null)
        {
            return new OptionalResponse<Server>(OptionalErrorType.ServiceError, "Couldn't create server");
        }

        var addDefaultRoomResult = await roomManager.AddDefaultRoom(server);
        if (!addDefaultRoomResult.IsSuccess)
        {
            return new OptionalResponse<Server>(OptionalErrorType.ServiceError, addDefaultRoomResult.ErrorMessage);
        }
        
        return server;
    }
}