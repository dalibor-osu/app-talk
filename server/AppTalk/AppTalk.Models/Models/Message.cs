using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppTalk.Utils.Interfaces;
using static AppTalk.Models.Database.Constants;

namespace AppTalk.Models.Models;

[Table(MessageTable.TableName)]
public sealed record Message : IBasicDatabaseItem, IUserIdentifiable
{
    /// <inheritdoc cref="IIdentifiable.Id"/>
    [Column(IIdentifiable.ColumnName)]
    [Key]
    public Guid Id { get; set; }

    /// <inheritdoc cref="IUserIdentifiable.UserId"/>
    [Column(IUserIdentifiable.ColumnName)]
    [Required]
    public Guid UserId { get; set; }

    public User User { get; set; }

    /// <summary>
    /// ID of the parent room
    /// </summary>
    [Column(MessageTable.RoomId)]
    [Required]
    public Guid RoomId { get; set; }

    public Room Room { get; set; }

    /// <summary>
    /// Text content of the message
    /// </summary>
    [Column(MessageTable.Content)]
    [Required]
    [MaxLength(2048)]
    public string Content { get; set; }

    /// <inheritdoc cref="ICreated.Created"/>
    [Column(ICreated.ColumnName)]
    public DateTimeOffset Created { get; set; }

    /// <inheritdoc cref="IUpdated.Updated"/>
    [Column(IUpdated.ColumnName)]
    public DateTimeOffset Updated { get; set; }

    /// <inheritdoc cref="IDeletable.Deleted"/>
    [Column(IDeletable.ColumnName)]
    public bool Deleted { get; set; }
}