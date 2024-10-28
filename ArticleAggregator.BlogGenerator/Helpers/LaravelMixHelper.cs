using Newtonsoft.Json;

namespace ArticleAggregator.BlogGenerator.Helpers;

public static class LaravelMixHelper
{
    private static Dictionary<string, string> _manifest = [];
    private const string OutputDirectoryRelativePath = "mix";

    public static string Mix(string path)
    {
        if (_manifest.Count > 0)
        {
            return _manifest.TryGetValue(path, out var value) ? value : path;
        }

        var manifestPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            OutputDirectoryRelativePath,
            "mix-manifest.json"
        );

        var json = File.ReadAllText(manifestPath);
        _manifest = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

        return _manifest.TryGetValue(path, out var value1) ? value1 : path;
    }
}