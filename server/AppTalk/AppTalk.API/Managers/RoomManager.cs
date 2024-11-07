using AppTalk.API.DatabaseService.Interfaces;
using AppTalk.Core.Enums;
using AppTalk.Core.Extensions;
using AppTalk.Core.Models;
using AppTalk.Models.Models;

namespace AppTalk.API.Managers;

public class RoomManager (IRoomDatabaseService roomDatabaseService)
{
    public async Task<OptionalResponse<Room>> AddDefaultRoom(Server server)
    {
        server.ArgumentNullCheck(nameof(server));
        
        var defaultRoom = CreateDefaultRoom(server);
        var addResult = await roomDatabaseService.AddAsync(defaultRoom);

        if (addResult == null)
        {
            return new OptionalResponse<Room>(OptionalErrorType.ServiceError, "Failed to add a default room");
        }
        
        return addResult;
    }

    private Room CreateDefaultRoom(Server server)
    {
        return new Room
        {
            Name = "General",
            ServerId = server.Id.EmptyCheck(nameof(server.Id))
        };
    }
}