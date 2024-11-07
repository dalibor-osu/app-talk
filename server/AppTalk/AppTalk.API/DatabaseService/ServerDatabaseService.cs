using System.Data;
using AppTalk.API.DatabaseService.Interfaces;
using AppTalk.Core.Base;
using AppTalk.Core.Interfaces.Model;
using AppTalk.Models.Models;
using static AppTalk.Models.Database.Constants;

namespace AppTalk.API.DatabaseService;

public class ServerDatabaseService(Func<IDbConnection> connectionFactory)
    : DatabaseServiceBase<Server>(connectionFactory, ServersTable.TableName), IServerDatabaseService
{
    public new async Task<Server> GetAsync(Guid id) => await base.GetAsync(id);
    public new async Task<bool> DeleteAsync(Guid id) => await base.DeleteAsync(id);
    public new async Task<bool> ExistsAsync(Guid id) => await base.ExistsAsync(id);

    public async Task<Server> AddAsync(Server server)
    {
        const string query =
            $"""
                 INSERT INTO app_talk.{ServersTable.TableName} (
                     {IName.ColumnName},
                     {IUserIdentifiable.ColumnName}
                 ) VALUES (
                     @name,
                     @userId
                 ) RETURNING *;
             """;

        return await base.AddAsync(query, server);
    }

    public async Task<bool> UpdateAsync(Server server)
    {
        const string query =
            $"""
                 UPDATE app_talk.{ServersTable.TableName}
                     SET 
                        {IName.ColumnName} = @name,
                        {IUpdated.ColumnName} = @updated
                     WHERE {IIdentifiable.ColumnName} = @id
                        AND {IDeletable.ColumnName} = false
                 RETURNING *;
             """;

        return await base.UpdateAsync(query, server);
    }
}