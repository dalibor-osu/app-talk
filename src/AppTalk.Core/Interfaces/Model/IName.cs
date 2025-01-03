using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace AppTalk.Core.Interfaces.Model;

public interface IName
{
    /// <summary>
    /// Gets or sets username name value
    /// </summary>
    public string Name { get; set; }

    public const string ColumnName = "name";
    public const int MaxLength = 32;
    public const int MinLength = 2;
    
    public static bool Validate(IName value)
    {
        return value.Name?.Length is <= MaxLength and >= MinLength;
    }
}