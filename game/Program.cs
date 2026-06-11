using BeginnerCrud.Api.Data;
using BeginnerCrud.Infrastructure.Options;
using BeginnerCrud.Api.Services.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using BeginnerCrud.Application.Services.Contacts;
using BeginnerCrud.Application.Repositories.Contacts;
using BeginnerCrud.Infrastructure.Repositories.Contacts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<DatabaseOptions>(
    builder.Configuration.GetSection("Database")
);

builder.Services.Configure<SmtpOptions>(
    builder.Configuration.GetSection("Smtp")
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

builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IContactService, ContactService>();

builder.Services.AddHostedService<StartupConnectionCheckService>();

var app = builder.Build();

app.MapControllers();

app.Run();