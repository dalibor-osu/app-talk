using AppTalk.Core.Interfaces.Database.Support;
using AppTalk.Models.Models;

namespace AppTalk.API.DatabaseService.Interfaces;

public interface IServerDatabaseService : ICrudSupport<Server>, IExistsSupport;