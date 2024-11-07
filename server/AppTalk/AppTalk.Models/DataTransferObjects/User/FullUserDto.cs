namespace AppTalk.Models.DataTransferObjects.User;

public class FullUserDto : UserDto
{
    /// <summary>
    /// DateTime when User was created
    /// </summary>
    public DateTimeOffset Created { get; set; }
    
    /// <summary>
    /// DateTime when User was last updated
    /// </summary>
    public DateTimeOffset Updated { get; set; }
}