var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<>

var app = builder.Build();

app.MapControllers();

app.Run();
