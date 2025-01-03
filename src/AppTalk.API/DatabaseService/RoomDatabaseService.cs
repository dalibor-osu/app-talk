using System.Data;
using AppTalk.API.DatabaseService.Interfaces;
using AppTalk.Core.Base;
using AppTalk.Core.Interfaces.Model;
using AppTalk.Models.Models;
using static AppTalk.Models.Database.Constants;

namespace AppTalk.API.DatabaseService;

public class RoomDatabaseService(Func<IDbConnection> connectionFactory)
    : DatabaseServiceBase<Room>(connectionFactory, RoomsTable.TableName), IRoomDatabaseService
{
    public new async Task<bool> DeleteAsync(Guid id) => await base.DeleteAsync(id);
    public new async Task<bool> ExistsAsync(Guid id) => await base.ExistsAsync(id);
    public async Task<bool> ExistsAsync(string id) => await ExistsByNameAsync(id);
    public new async Task<Room> GetAsync(Guid id) => await base.GetAsync(id);

    public async Task<Room> AddAsync(Room model)
    {
        const string query =
            $"""
                INSERT INTO app_talk.{RoomsTable.TableName} (
                    {IServerIdentifiable.ColumnName},
                    {IName.ColumnName}
                ) VALUES (
                    @serverId,
                    @name
                ) RETURNING *;
            """;

        return await base.AddAsync(query, model);
    }

    public async Task<bool> UpdateAsync(Room model)
    {
        const string query =
            $"""
                UPDATE app_talk.{RoomsTable.TableName}
                    SET 
                        {IName.ColumnName} = @name,
                        {IUpdated.ColumnName} = @updated
                    WHERE {IIdentifiable.ColumnName} = @id
                        AND {IDeletable.ColumnName} = false
                RETURNING *;
            """;

        return await base.UpdateAsync(query, model);
    }
}