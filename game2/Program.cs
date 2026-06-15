using TestCrudApplication.Api.HostedServices;
using TestCrudApplication.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Npgsql;
using TestCrudApplication.Infrastructure.Database;
using TestCrudApplication.Application.Services;
using TestCrudApplication.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using TestCrudApplication.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();

builder.Services.Configure<SmtpOptions>(
    builder.Configuration.GetSection("SMTP")
);

builder.Services.Configure<DatabaseOptions>(
    builder.Configuration.GetSection("Database")
);

builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
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
