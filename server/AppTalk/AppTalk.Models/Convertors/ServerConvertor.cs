using AppTalk.Core.Extensions;
using AppTalk.Models.DataTransferObjects;
using AppTalk.Models.Models;

namespace AppTalk.Models.Convertors;

public static class ServerConvertor
{
    public static Server ToModel(this ServerDto source, Guid? id = null)
    {
        source.ArgumentNullCheck(nameof(source));

        var model = new Server
        {
            Name = source.Name,
            UserId = source.UserId,
        };

        if (id != null)
        {
            model.Id = id.Value.EmptyCheck(nameof(id));
        }

        return model;
    }
}