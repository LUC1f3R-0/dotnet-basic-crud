using TestCrudApplication.Api.Services.Startup;
using TestCrudApplication.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Npgsql;
using TestCrudApplication.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<SmtpOptions>(
    builder.Configuration.GetSection("SMTP")
);

builder.Services.Configure<DatabaseOptions>(
    builder.Configuration.GetSection("Database")
);

builder.Services.AddDbContext<AppDBContext>((serviceProvider, options) =>
{
    var databaseOptions = serviceProvider
        .GetRequiredService<IOptions<DatabaseOptions>>()
        .Value;

    var connectionStringBuilder = new NpgsqlConnectionStringBuilder
    {
        Host = databaseOptions.Host,
        Port = databaseOptions.Port,
        Database = databaseOptions.Name,
        Username = databaseOptions.Username,
        Password = databaseOptions.Password
    };

    options.UseNpgsql(connectionStringBuilder.ConnectionString);
});

builder.Services.AddHostedService<StartupConnectionCheckService>();

var app = builder.Build();

app.MapControllers();

app.Run();
