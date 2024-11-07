using System.Text.RegularExpressions;

namespace AppTalk.Core.Interfaces.Model;

public partial interface IEmail
{
    /// <summary>
    /// Gets or sets email value
    /// </summary>
    public string Email { get; set; }
    
    public const string ColumnName = "email";
    
    public static bool Validate(IEmail value)
    {
        return ValidationRegex().IsMatch(value.Email);
    }

    [GeneratedRegex(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$")]
    private static partial Regex ValidationRegex();
}