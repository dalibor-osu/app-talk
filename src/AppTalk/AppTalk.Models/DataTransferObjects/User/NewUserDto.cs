using AppTalk.Core.Interfaces.Model;

namespace AppTalk.Models.DataTransferObjects.User;

public class NewUserDto : IUsername, IPasswordCarrier, IEmail
{
    /// <inheritdoc cref="IUsername.Username"/>
    public string Username { get; set; }
    
    /// <inheritdoc cref="IPasswordCarrier.Password"/>
    public string Password { get; set; }
    
    /// <inheritdoc cref="IEmail.Email"/>
    public string Email { get; set; }
}