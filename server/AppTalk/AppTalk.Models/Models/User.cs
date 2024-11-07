using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppTalk.Core.Interfaces.Model;
using static AppTalk.Models.Database.Constants;

namespace AppTalk.Models.Models;

[Table(UsersTable.TableName)]
public sealed record User : IBasicDatabaseItem, IUsername
{
    /// <inheritdoc cref="IIdentifiable.Id"/>
    [Column(IIdentifiable.ColumnName)]
    [Key]
    public Guid Id { get; set; }

    /// <inheritdoc cref="IUsername.Username"/>
    [Column(IUsername.ColumnName)]
    [Required]
    [MaxLength(IName.MaxLength)]
    public string Username { get; set; }

    /// <summary>
    /// Hash of the user's password
    /// </summary>
    [Column(UsersTable.PasswordHash)]
    [Required]
    public string PasswordHash { get; set; }

    /// <summary>
    /// User's email
    /// </summary>
    [Column(UsersTable.Email)]
    [Required]
    [MaxLength(320)]
    public string Email { get; set; }

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