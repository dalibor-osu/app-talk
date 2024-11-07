namespace AppTalk.Models.DataTransferObjects.User;

public class UserDto
{
    /// <summary>
    /// ID of a user
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User's username
    /// </summary>
    public string Username { get; set; }
}