namespace AppTalk.Core.Extensions;

public static class GenericExtensions
{
    public static object ArgumentNullCheck(this object value, string paramName)
    {
        return value ?? throw new ArgumentNullException(paramName);
    }
}