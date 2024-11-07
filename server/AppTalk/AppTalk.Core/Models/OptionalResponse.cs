using AppTalk.Core.Enums;
using AppTalk.Core.Validation;

namespace AppTalk.Core.Models;

public class OptionalResponse<T>
{
    public T Value { get; private set; }
    public OptionalErrorType Type { get; private set; } = OptionalErrorType.Invalid;
    public string ErrorMessage { get; private set; } = string.Empty;
    
    public bool IsSuccess => Type == OptionalErrorType.Invalid;
    public bool IsValidationError => Type == OptionalErrorType.ValidationError;
    
    public ValidationResult ValidationResult { get; private set; } = new();

    public OptionalResponse(T value)
    {
        if (Equals(default(T), null) && Equals(value, null))
        {
            throw new ArgumentNullException(nameof(value), $"{nameof(value)} cannot be null when creating a success response.");
        }
        
        Value = value;
    }

    public OptionalResponse(OptionalErrorType type, string errorMessage)
    {
        if (type == OptionalErrorType.Invalid)
        {
            throw new ArgumentNullException(nameof(type), $"{nameof(type)} cannot be Invalid when creating an error response.");
        }
        
        Type = type;
        ErrorMessage = errorMessage;
    }
    
    public OptionalResponse(ValidationResult validationResult)
    {
        if (validationResult.IsValid)
        {
            throw new ArgumentException($"Validation result cannot be valid when creating an error response.", nameof(validationResult));
        }
        
        Type = OptionalErrorType.BadRequest;
    }

    public static implicit operator OptionalResponse<T>(T value) => new(value);
    public static implicit operator OptionalResponse<T>(ValidationResult validationResult) => new(validationResult);

    public T GetValue()
    {
        if (Type != OptionalErrorType.Invalid)
        {
            throw new InvalidOperationException("OptionalResponse was not successful, but GetValue() was called.");
        }
        
        return Value;
    }

    public ErrorMessageWrapper GetErrorMessageWrapper() => new() { ErrorMessage = ErrorMessage };
}