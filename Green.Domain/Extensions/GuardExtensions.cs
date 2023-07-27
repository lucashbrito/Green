namespace Green.Domain.Extensions;

public static class GuardExtensions
{
    public static void NullGuard(this object obj, string errorMessage, string paramName)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(errorMessage, paramName);
        }
    }
}
