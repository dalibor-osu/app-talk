using System.Data;

namespace AppTalk.Core.Base;

public abstract class DatabaseServiceBase(Func<IDbConnection> connectionFactory)
{
    protected Func<IDbConnection> ConnectionFactory { get; } = connectionFactory;
}