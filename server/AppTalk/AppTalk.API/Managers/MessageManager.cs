using AppTalk.API.DatabaseService.Interfaces;
using AppTalk.Core.Enums;
using AppTalk.Core.Extensions;
using AppTalk.Core.Models;
using AppTalk.Models.Models;

namespace AppTalk.API.Managers;

public class MessageManager(IMessageDatabaseService messageDatabaseService, IRoomDatabaseService roomDatabaseService)
{
    public async Task<OptionalResponse<Message>> AddMessage(Guid userId, Message message)
    {
        message.ArgumentNullCheck(nameof(message));

        bool roomExists = await roomDatabaseService.ExistsAsync(message.RoomId);
        if (!roomExists)
        {
            return new OptionalResponse<Message>(OptionalErrorType.NotFound, "Room does not exist");
        }
        
        var addResult = await messageDatabaseService.AddAsync(message);
        if (addResult == null)
        {
            return new OptionalResponse<Message>(OptionalErrorType.ServiceError, "Could not add message");
        }

        return message;
    }
}