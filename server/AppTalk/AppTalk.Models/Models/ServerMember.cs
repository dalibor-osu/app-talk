using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppTalk.Utils.Interfaces;
using static AppTalk.Models.Database.Constants;

namespace AppTalk.Models.Models;

[Table(ServerMemberTable.TableName)]
public sealed record ServerMember : IIdentifiable, IServerIdentifiable, IUserIdentifiable, ICreated
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

    /// <inheritdoc cref="IServerIdentifiable.ServerId"/>
    [Column(IServerIdentifiable.ColumnName)]
    [Required]
    public Guid ServerId { get; set; }

    public Server Server { get; set; }

    /// <inheritdoc cref="ICreated.Created"/>
    [Column(ICreated.ColumnName)]
    public DateTimeOffset Created { get; set; }
}