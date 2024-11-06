using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppTalk.Utils.Interfaces;
using static AppTalk.Models.Database.Constants;

namespace AppTalk.Models.Models;

[Table(ServerTable.TableName)]
public sealed record Server : IBasicDatabaseItem, IUserIdentifiable
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
    /// Name of the server
    /// </summary>
    [Column(UserTable.Username)]
    [Required]
    [MaxLength(32)]
    public string Name { get; set; }

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