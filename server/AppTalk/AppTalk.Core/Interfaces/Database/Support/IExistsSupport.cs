namespace AppTalk.Core.Interfaces.Database.Support;

public interface IExistsSupport<in TId>
{
    public Task<bool> ExistsAsync(TId id);
}

public interface IExistsSupport : IExistsSupport<Guid>;