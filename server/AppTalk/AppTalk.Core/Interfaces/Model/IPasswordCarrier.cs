using System.Text.RegularExpressions;

namespace AppTalk.Core.Interfaces.Model;

public partial interface IPasswordCarrier
{
    /// <summary>
    /// Gets or sets password value
    /// </summary>
    public string Password { get; set; }
    
    public static bool Validate(IPasswordCarrier value)
    {
        return ValidationRegex().IsMatch(value.Password);
    }

    [GeneratedRegex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$")]
    private static partial Regex ValidationRegex();
}