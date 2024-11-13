using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppTalk.Core.Interfaces.Model;
using static AppTalk.Models.Database.Constants;

namespace AppTalk.Models.Models;

[Table(MessagesTable.TableName)]
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
    [Column(MessagesTable.RoomId)]
    [Required]
    public Guid RoomId { get; set; }

    public Room Room { get; set; }

    /// <summary>
    /// Text content of the message
    /// </summary>
    [Column(MessagesTable.Content)]
    [Required]
    [MaxLength(MessagesTable.MaxContentLength)]
    public string Content { get; set; }

    /// <inheritdoc cref="ICreated.Created"/>
    [Column(ICreated.ColumnName)]
    public DateTimeOffset Created { get; set; }

    /// <inheritdoc cref="IUpdated.Updated"/>
    [Column(IUpdated.ColumnName)]
    public DateTimeOffset? Updated { get; set; }

    /// <inheritdoc cref="IDeletable.Deleted"/>
    [Column(IDeletable.ColumnName)]
    public bool Deleted { get; set; }
}