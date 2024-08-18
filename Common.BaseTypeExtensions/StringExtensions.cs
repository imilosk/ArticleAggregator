namespace Common.BaseTypeExtensions;

public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string? input)
    {
        return string.IsNullOrEmpty(input);
    }

    public static bool IsNullOrWhiteSpace(this string? input)
    {
        return string.IsNullOrWhiteSpace(input);
    }
}