using System.Text.RegularExpressions;

namespace AppTalk.Core.Interfaces.Model;

public partial interface IUsername
{
    /// <summary>
    /// Gets or sets username name value
    /// </summary>
    public string Username { get; set; }

    public const string ColumnName = "username";
    public const int MaxLength = 32;
    public const int MinLength = 3;
    
    public static bool Validate(IUsername value)
    {
        return ValidationRegex().IsMatch(value.Username);
    }

    [GeneratedRegex("^[a-z0-9_]{3,32}$")]
    private static partial Regex ValidationRegex();
}