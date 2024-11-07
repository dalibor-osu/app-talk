namespace AppTalk.Core.Interfaces.Model;

public interface IServerIdentifiable
{
    /// <summary>
    /// ID of the parent server of the entity
    /// </summary>
    public Guid ServerId { get; set; }

    /// <summary>
    /// Name of the column containing ID of the parent server in database
    /// </summary>
    public const string ColumnName = "server_id";
}