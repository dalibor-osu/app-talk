namespace AppTalk.Core.Interfaces.Database.Support;

public interface IAddSupport<TModel> where TModel : class
{
    public Task<TModel> AddAsync(TModel model);
}