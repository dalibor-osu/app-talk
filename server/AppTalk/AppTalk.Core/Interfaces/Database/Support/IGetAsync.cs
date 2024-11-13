using AppTalk.Core.Interfaces.Model;

namespace AppTalk.Core.Interfaces.Database.Support;

public interface IGetSupport<TModel, in TId> where TModel : class, IIdentifiable<TId>
{
    public Task<TModel> GetAsync(TId id);
}
 
public interface IGetSupport<TModel> where TModel : class, IIdentifiable
{
    public Task<TModel> GetAsync(Guid id);
}