namespace AppTalk.Core.Interfaces.Database.Support;

public interface IUpdateSupport<in TModel> where TModel : class
{
    public Task<bool> UpdateAsync(TModel model);
}