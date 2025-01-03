using AppTalk.Core.Interfaces.Model;

namespace AppTalk.Core.Interfaces.Database.Support;

public interface ICrudSupport<TModel, in TId> : 
    IAddSupport<TModel>,
    IGetSupport<TModel,TId>,
    IUpdateSupport<TModel>,
    IDeleteSupport<TId> where TModel : class, IIdentifiable<TId>;
    
public interface ICrudSupport<TModel> : 
    IAddSupport<TModel>,
    IGetSupport<TModel, Guid>,
    IUpdateSupport<TModel>,
    IDeleteSupport<Guid> where TModel : class, IIdentifiable<Guid>;