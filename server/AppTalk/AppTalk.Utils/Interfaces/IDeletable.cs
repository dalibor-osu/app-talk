namespace AppTalk.Utils.Interfaces;

public interface IDeletable
{
    /// <summary>
    /// Information whether the item is deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Name of the column containing deleted info in database
    /// </summary>
    public const string ColumnName = "deleted";
}