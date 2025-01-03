using AppTalk.Core.Interfaces.Model;

namespace AppTalk.Core.Validation;

public static class Validator
{
    public static ValidationResult Validate(object obj)
    {
        var result = new ValidationResult();
        if (obj is IUsername username && !IUsername.Validate(username))
        {
            result.Errors.Add(new ValidationError
            {
                PropertyName = nameof(username.Username),
                Message = "Invalid username format"
            });
        }

        if (obj is IPasswordCarrier passwordCarrier && !IPasswordCarrier.Validate(passwordCarrier))
        {
            result.Errors.Add(new ValidationError
            {
                PropertyName = nameof(passwordCarrier.Password),
                Message = "Invalid password format"
            });
        }
        
        if (obj is IEmail email && !IEmail.Validate(email))
        {
            result.Errors.Add(new ValidationError
            {
                PropertyName = nameof(email.Email),
                Message = "Invalid email format"
            });
        }
        
        if (obj is IName name && !IName.Validate(name))
        {
            result.Errors.Add(new ValidationError
            {
                PropertyName = nameof(name.Name),
                Message = "Invalid name format"
            });
        }

        return result;
    }
}