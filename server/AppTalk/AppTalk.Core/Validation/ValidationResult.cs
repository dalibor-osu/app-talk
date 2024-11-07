namespace AppTalk.Core.Validation;

public class ValidationResult
{
    public List<ValidationError> Errors { get; set; } = new();
    public bool IsValid => Errors.Count == 0;
}