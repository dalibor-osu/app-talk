using System.Data;
using AppTalk.API.DatabaseService.Interfaces;
using AppTalk.Core.Base;
using AppTalk.Core.Interfaces.Model;
using AppTalk.Models.Models;
using Dapper;
using static AppTalk.Models.Database.Constants;

namespace AppTalk.API.DatabaseService;

public class UserDatabaseService(Func<IDbConnection> connectionFactory) : DatabaseServiceBase<User>(connectionFactory, UsersTable.TableName), IUserDatabaseService
{
    public new async Task<bool> DeleteAsync(Guid id) => await base.DeleteAsync(id);
    public new async Task<bool> ExistsAsync(Guid id) => await base.ExistsAsync(id);
    public new async Task<User> GetAsync(Guid id) => await base.GetAsync(id);
    
    public async Task<User> AddAsync(User user)
    {
        const string query =
            $"""
                 INSERT INTO app_talk.{UsersTable.TableName} (
                     {IUsername.ColumnName},
                     {UsersTable.PasswordHash},
                     {UsersTable.Email}
                 ) VALUES (
                     @username,
                     @passwordHash,
                     @email
                 ) RETURNING *;
             """;

        return await base.AddAsync(query, user);
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        const string query =
            $"""
                 SELECT * FROM app_talk.{UsersTable.TableName}
                     WHERE {IUsername.ColumnName} = @username
                        AND {IDeletable.ColumnName} = false;
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
                         WHERE {IUsername.ColumnName} = @username
                             OR "{UsersTable.Email}" = @email
                 );
             """;

        var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<bool>(query, new { username, email });
    }

    public async Task<bool> UpdateAsync(User user)
    {
        const string query =
            $"""
                 UPDATE app_talk.{UsersTable.TableName}
                     SET
                        {IUsername.ColumnName} = @username,
                        {IUpdated.ColumnName} = @updated
                     WHERE {IIdentifiable.ColumnName} = @id
                        AND {IDeletable.ColumnName} = false
                 RETURNING *;
             """;

        return await base.UpdateAsync(query, user);
    }

    public async Task<string> GetPasswordHashByUsernameAsync(string username)
    {
        const string query =
            $"""
                 SELECT {UsersTable.PasswordHash} FROM app_talk.{UsersTable.TableName}
                     WHERE {IUsername.ColumnName} = @username
                        AND {IDeletable.ColumnName} = false;
             """;

        var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<string>(query, new { username });
    }
}