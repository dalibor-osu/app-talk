using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppTalk.Core.Interfaces.Model;
using static AppTalk.Models.Database.Constants;

namespace AppTalk.Models.Models;

[Table(RoomsTable.TableName)]
public sealed record Room : IBasicDatabaseItem, IServerIdentifiable, IName
{
    /// <inheritdoc cref="IIdentifiable.Id"/>
    [Column(IIdentifiable.ColumnName)]
    [Key]
    public Guid Id { get; set; }

    /// <inheritdoc cref="IServerIdentifiable.ServerId"/>
    [Column(IServerIdentifiable.ColumnName)]
    [Required]
    public Guid ServerId { get; set; }

    public Server Server { get; set; }

    /// <inheritdoc cref="IName.Name"/>
    [Column(IName.ColumnName)]
    [Required]
    [MaxLength(IName.MaxLength)]
    public string Name { get; set; } = string.Empty;

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