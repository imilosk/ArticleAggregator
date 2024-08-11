using System.Text.RegularExpressions;

namespace Common.WebParsingUtils;

public static partial class StripHtml
{
    [GeneratedRegex("<.*?>")]
    public static partial Regex StripHtmlTagsRegex();
}