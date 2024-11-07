using System.Text.RegularExpressions;

namespace AppTalk.Core.Interfaces.Model;

public partial interface IUsername
{
    /// <summary>
    /// Gets or sets username name value
    /// </summary>
    public string Username { get; set; }

    public const string ColumnName = "username";
    
    public static bool Validate(IUsername value)
    {
        return ValidationRegex().IsMatch(value.Username);
    }

    [GeneratedRegex("^[a-z0-9_]{3,16}$")]
    private static partial Regex ValidationRegex();
}