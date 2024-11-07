namespace AppTalk.Models.DataTransferObjects;

public class ServerDto
{
    /// <summary>
    /// ID of the server
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Creator of the server
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Name of the server
    /// </summary>
    public string Name { get; set; } = string.Empty;
}