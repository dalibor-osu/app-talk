using System.Data;
using AppTalk.API.DatabaseService.Interfaces;
using AppTalk.Core.Base;
using AppTalk.Core.Interfaces.Model;
using AppTalk.Models.Models;
using Dapper;
using static AppTalk.Models.Database.Constants;

namespace AppTalk.API.DatabaseService;

public class UserDatabaseService(Func<IDbConnection> connectionFactory) : DatabaseServiceBase(connectionFactory), IUserDatabaseService
{
    public async Task<User> CreateUserAsync(User user)
    {
        const string query =
            $"""
                 INSERT INTO app_talk.{UsersTable.TableName} (
                     {UsersTable.Username},
                     {UsersTable.PasswordHash},
                     {UsersTable.Email},
                     {ICreated.ColumnName},
                     {IUpdated.ColumnName},
                     {IDeletable.ColumnName}
                 ) VALUES (
                     @username,
                     @passwordHash,
                     @email,
                     @created,
                     @updated,
                     @deleted
                 ) RETURNING *;
             """;

        user.Created = user.Updated = DateTimeOffset.UtcNow;

        using var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<User>(query, user);
    }

    public async Task<User> GetUserAsync(Guid id)
    {
        const string query =
            $"""
                 SELECT * FROM app_talk.{UsersTable.TableName}
                     WHERE {IIdentifiable.ColumnName} = @id;
             """;

        using var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<User>(query, new { id });
    }

    public async Task<User> GetUserAsync(string username)
    {
        const string query =
            $"""
                 SELECT * FROM app_talk.{UsersTable.TableName}
                     WHERE {UsersTable.Username} = @username;
             """;

        using var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<User>(query, new { username });
    }

    public async Task<bool> UsernameOrEmailExistsAsync(string username, string email)
    {
        const string query =
            $"""
                 SELECT EXISTS (
                     SELECT 1 FROM app_talk.{UsersTable.TableName}
                         WHERE {UsersTable.Username} = @username
                             OR "{UsersTable.Email}" = @email
                 );
             """;

        var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<bool>(query, new { username, email });
    }

    public async Task<User> UpdateAsync(User user)
    {
        const string query =
            $"""
                 UPDATE app_talk.{UsersTable.TableName}
                     SET {UsersTable.Username} = @username
                     WHERE {IIdentifiable.ColumnName} = @id
                 RETURNING *;
             """;

        user.Updated = DateTimeOffset.UtcNow;

        var connection = ConnectionFactory();
        return await connection.QuerySingleOrDefaultAsync<User>(query, user);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        const string query =
            $"""
                 DELETE FROM app_talk.{UsersTable.TableName}
                     WHERE {IIdentifiable.ColumnName} = @id;
             """;

        var connection = ConnectionFactory();
        return await connection.ExecuteAsync(query, new { id }) > 0;
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        const string query =
            $"""
                 SELECT EXISTS (
                     SELECT 1 FROM app_talk.{UsersTable.TableName}
                         WHERE {IIdentifiable.ColumnName} = @id
                 );
             """;

        var connection = ConnectionFactory();
        return connection.QuerySingleAsync<bool>(query, new { id });
    }

    public async Task<string> GetPasswordHashByUsernameAsync(string username)
    {
        const string query =
            $"""
                 SELECT {UsersTable.PasswordHash} FROM app_talk.{UsersTable.TableName}
                     WHERE {UsersTable.Username} = @username;
             """;

        var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<string>(query, new { username });
    }
}