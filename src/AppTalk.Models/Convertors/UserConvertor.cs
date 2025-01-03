using AppTalk.Models.DataTransferObjects;
using AppTalk.Models.DataTransferObjects.User;
using AppTalk.Models.Models;

namespace AppTalk.Models.Convertors;

public static class UserConvertor
{
    public static UserDto ToDto(this User source)
    {
        return new UserDto
        {
            Id = source.Id,
            Username = source.Username,
        };
    }

    public static FullUserDto ToFullDto(this User source)
    {
        return new FullUserDto
        {
            Id = source.Id,
            Username = source.Username,
            Created = source.Created,
            Updated = source.Updated,
        };
    }
}