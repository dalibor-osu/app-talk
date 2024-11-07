using AppTalk.Core.Interfaces.Model;

namespace AppTalk.Core.Validation;

public class Validator
{
    public ValidationResult Validate(object obj)
    {
        var result = new ValidationResult();
        if (obj is IUsername username && !IUsername.Validate(username))
        {
            result.Errors.Add(new ValidationError
            {
                PropertyName = nameof(IUsername.Username),
                Message = "Invalid username format"
            });
        }

        if (obj is IPasswordCarrier passwordCarrier && !IPasswordCarrier.Validate(passwordCarrier))
        {
            result.Errors.Add(new ValidationError
            {
                PropertyName = nameof(IPasswordCarrier.Password),
                Message = "Invalid password format"
            });
        }
        
        if (obj is IEmail email && !IEmail.Validate(email))
        {
            result.Errors.Add(new ValidationError
            {
                PropertyName = nameof(IEmail.Email),
                Message = "Invalid email format"
            });
        }

        return result;
    }
}