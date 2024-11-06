namespace AppTalk.Utils.Interfaces;

public interface IIdentifiable
{
    /// <summary>
    /// ID of the entity
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name of the column containing ID in database
    /// </summary>
    public const string ColumnName = "id";
}