var builder = WebApplication.CreateSlimBuilder(args);

var app = builder.Build();

var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/api/articles", () => { return Results.Ok(); });

app.Run();