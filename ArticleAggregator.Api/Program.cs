var builder = WebApplication.CreateSlimBuilder(args);

var environment = builder.Environment.EnvironmentName;
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{environment}.json", true)
    .AddJsonFile($"secrets.{environment}.json", true)
    .AddEnvironmentVariables()
    .Build();

var app = builder.Build();

var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/api/articles", () => { return Results.Ok(); });

app.Run();