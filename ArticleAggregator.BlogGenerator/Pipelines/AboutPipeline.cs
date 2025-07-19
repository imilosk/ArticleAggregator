using Statiq.Razor;

namespace ArticleAggregator.BlogGenerator.Pipelines;

internal class AboutPipeline : Pipeline
{
    public AboutPipeline()
    {
        InputModules =
        [
            new ReadFiles("About.cshtml"),
        ];

        ProcessModules =
        [
            new RenderRazor(),
            new SetDestination("about.html"),
        ];

        OutputModules =
        [
            new WriteFiles(),
        ];
    }
}