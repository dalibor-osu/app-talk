using System.Data;
using AppTalk.Core.Interfaces.Model;
using Dapper;

namespace AppTalk.Core.Base;

public abstract class DatabaseServiceBase<T>(Func<IDbConnection> connectionFactory, string tableName) where T : class
{
    protected Func<IDbConnection> ConnectionFactory { get; } = connectionFactory;

    protected async Task<T> GetAsync(Guid id)
    {
        string query =
            $"""
                 SELECT * FROM app_talk.{tableName}
                     WHERE {IIdentifiable.ColumnName} = @id;
             """;

        using var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<T>(query, new { id });
    }

    protected async Task<bool> DeleteAsync(Guid id)
    {
        string query;
        bool isSoftDeletable = Array.Exists(typeof(T).GetInterfaces(), x =>
            x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDeletable));

        if (isSoftDeletable)
        {
            query =
                $"""
                     UPDATE app_talk.{tableName}
                         SET
                            {IDeletable.ColumnName} = true,
                            {IUpdated.ColumnName} = now()
                         WHERE {IIdentifiable.ColumnName} = @id
                     RETURNING *;
                 """;
        }
        else
        {
            query =
                $"""
                     DELETE FROM app_talk.{tableName}
                         WHERE {IIdentifiable.ColumnName} = @id;
                 """;
        }

        using var connection = ConnectionFactory();
        return await connection.ExecuteAsync(query, new { id }) > 0;
    }

    protected async Task<bool> ExistsAsync(Guid id)
    {
        string query =
            $"""
                 SELECT EXISTS (
                     SELECT 1 FROM app_talk.{tableName}
                         WHERE {IIdentifiable.ColumnName} = @id
                 );
             """;

        using var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<bool>(query, new { id });
    }

    protected async Task<bool> ExistsByNameAsync(string name)
    {
        string query =
            $"""
                 SELECT EXISTS (
                     SELECT 1 FROM app_talk.{tableName}
                         WHERE {IName.ColumnName} = @name
                 );
             """;

        using var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<bool>(query, new { name });
    }

    protected async Task<bool> UpdateAsync(string query, T model)
    {
        if (model is IUpdated updated)
        {
            updated.Updated = DateTimeOffset.UtcNow;
        }

        using var connection = ConnectionFactory();
        return await connection.ExecuteAsync(query, model) > 0;
    }

    protected async Task<T> AddAsync(string query, T model)
    {
        using var connection = ConnectionFactory();
        return await connection.QuerySingleAsync<T>(query, model);
    }
}