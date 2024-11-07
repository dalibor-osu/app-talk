namespace AppTalk.Models.DataTransferObjects.User;

public class NewUserDto
{
    /// <summary>
    /// Username of a new user
    /// </summary>
    public string Username { get; set; }
    
    /// <summary>
    /// Password of a new user
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// Email of a new user
    /// </summary>
    public string Email { get; set; }
}