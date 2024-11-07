using System.Data;
using AppTalk.API.DatabaseService.Interfaces;
using AppTalk.Core.Base;
using AppTalk.Core.Interfaces.Model;
using AppTalk.Models.Models;
using Dapper;
using static AppTalk.Models.Database.Constants;

namespace AppTalk.API.DatabaseService;

public class ServerMemberDatabaseService(Func<IDbConnection> connectionFactory)
    : DatabaseServiceBase<ServerMember>(connectionFactory, ServerMembersTable.TableName), IServerMemberDatabaseService
{
    public async Task<ServerMember> AddAsync(ServerMember model)
    {
        const string query =
            $"""
                 INSERT INTO app_talk.{ServerMembersTable.TableName} (
                     {IUserIdentifiable.ColumnName},
                     {IServerIdentifiable.ColumnName}
                 ) VALUES (
                     @userId,
                     @serverId
                 ) RETURNING *;
             """;

        return await base.AddAsync(query, model);
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid serverId)
    {
        string query = 
            $"""
                DELETE FROM app_talk.{ServerMembersTable.TableName}
                    WHERE {IUserIdentifiable.ColumnName} = @userId
                        AND {IServerIdentifiable.ColumnName} = @serverId;
            """;

        using var connection = ConnectionFactory();
        return await connection.ExecuteAsync(query, new { userId, serverId }) > 0;
    }

    public async Task<bool> ExistsAsync(Guid userId, Guid serverId)
    {
        string query =
            $"""
                 SELECT EXISTS (
                     SELECT 1 FROM app_talk.{ServerMembersTable.TableName}
                         WHERE {IUserIdentifiable.ColumnName} = @userId
                            AND {IServerIdentifiable.ColumnName} = @serverId
                 );
             """;

        using var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<bool>(query, new { userId, serverId });
    }
}