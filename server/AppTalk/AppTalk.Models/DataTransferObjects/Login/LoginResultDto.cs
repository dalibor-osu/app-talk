using AppTalk.Core.Enums;
using AppTalk.Models.DataTransferObjects.User;

namespace AppTalk.Models.DataTransferObjects.Login;

public class LoginResultDto
{
    public string Token { get; set; }
    public LoginResultType ResultType { get; set; }
    
    public FullUserDto User { get; set; }
}