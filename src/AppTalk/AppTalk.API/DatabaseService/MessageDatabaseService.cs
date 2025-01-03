using System.Data;
using AppTalk.API.DatabaseService.Interfaces;
using AppTalk.Core.Base;
using AppTalk.Core.Interfaces.Model;
using AppTalk.Models.Models;
using static AppTalk.Models.Database.Constants;

namespace AppTalk.API.DatabaseService;

public class MessageDatabaseService(Func<IDbConnection> connectionFactory) : DatabaseServiceBase<Message>(connectionFactory, MessagesTable.TableName), IMessageDatabaseService
{
    public new async Task<Message> GetAsync(Guid id) => await base.GetAsync(id);
    public new async Task<bool> DeleteAsync(Guid id) => await base.DeleteAsync(id);
    
    public async Task<Message> AddAsync(Message model)
    {
        const string query =
            $"""
                INSERT INTO app_talk.{MessagesTable.TableName} (
                    {IUserIdentifiable.ColumnName},
                    {MessagesTable.RoomId},
                    {MessagesTable.Content}
                ) VALUES (
                    @userId,
                    @roomId,
                    @content
                ) RETURNING *;
             """;

        return await base.AddAsync(query, model);
    }

    public async Task<bool> UpdateAsync(Message model)
    {
        const string query =
            $"""
                 UPDATE app_talk.{MessagesTable.TableName}
                     SET 
                         {MessagesTable.Content} = @content,
                         {IUpdated.ColumnName} = @updated
                     WHERE {IIdentifiable.ColumnName} = @id
                        AND {IDeletable.ColumnName} = false
                 RETURNING *;
             """;

        return await base.UpdateAsync(query, model);
    }
}