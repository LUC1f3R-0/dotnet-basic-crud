using TestCrudApplication.Api.HostedServices;
using TestCrudApplication.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Npgsql;
using TestCrudApplication.Infrastructure.Database;
using TestCrudApplication.Application.Services;
using TestCrudApplication.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using TestCrudApplication.Infrastructure.Repositories;
using TestCrudApplication.Api.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using TestCrudApplication.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(modelState =>
                    modelState.Value?.Errors.Count > 0)
                .ToDictionary(
                    modelState => modelState.Key,
                    modelState => modelState.Value!.Errors
                        .Select(error => string.IsNullOrWhiteSpace(error.ErrorMessage) ? "The supplied value is invalid." : error.ErrorMessage)
                        .ToArray()
                );

            var errorResponse = new ErrorResponse
            {
                Message = "Request validation failed.",
                Errors = errors,
                TraceId = context.HttpContext.TraceIdentifier
            };

            return new BadRequestObjectResult(errorResponse);
        };
    });

// -------------------------------------------------------
// 2. GLOBAL EXCEPTION HANDLER REGISTRATION
// -------------------------------------------------------


builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddApplicationCors(builder.Configuration);
// -------------------------------------------------------
// 3. APPLICATION DEPENDENCY INJECTION
// -------------------------------------------------------

builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();

// -------------------------------------------------------
// 4. OPTIONS CONFIGURATION
// -------------------------------------------------------

builder.Services.Configure<SmtpOptions>(
    builder.Configuration.GetSection("SMTP")
);

builder.Services.Configure<DatabaseOptions>(
    builder.Configuration.GetSection("Database")
);

// -------------------------------------------------------
// 5. DATABASE CONTEXT
// -------------------------------------------------------

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

// -------------------------------------------------------
// 6. STARTUP SERVICE
// -------------------------------------------------------

builder.Services.AddHostedService<StartupConnectionCheckService>();

var app = builder.Build();

// -------------------------------------------------------
// 7. EXCEPTION-HANDLING MIDDLEWARE
// -------------------------------------------------------

app.UseExceptionHandler();

app.UseApplicationCors();
// -------------------------------------------------------
// 8. MAP CONTROLLER ENDPOINTS
// -------------------------------------------------------

app.MapControllers();

app.Run();
