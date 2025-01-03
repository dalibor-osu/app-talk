namespace AppTalk.Core.Interfaces.Database.Support;

public interface IDeleteSupport<in TId>
{
    public Task<bool> DeleteAsync(TId id);
}

public interface IDeleteSupport : IDeleteSupport<Guid>;