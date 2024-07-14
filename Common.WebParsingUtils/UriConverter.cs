namespace Common.WebParsingUtils;

public static class UriConverter
{
    public static Uri ConvertToAbsoluteUrl(Uri baseUrl, string relativeUrl)
    {
        if (Uri.TryCreate(baseUrl, relativeUrl, out var absoluteUri))
        {
            return absoluteUri;
        }

        throw new Exception("Cannot convert to absolute URL");
    }
}