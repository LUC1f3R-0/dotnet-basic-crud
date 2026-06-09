using BeginnerCrud.Api.Data;
using BeginnerCrud.Api.Options;
using MailKit.Security;
using Microsoft.Extensions.Options;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace BeginnerCrud.Api.Services.Startup;

public class StartupConnectionCheckService : IHostedService
{
    private readonly ILogger<StartupConnectionCheckService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly SmtpOptions _smtpOptions;

    public StartupConnectionCheckService(ILogger<StartupConnectionCheckService> logger, IServiceScopeFactory scopeFactory, IOptions<SmtpOptions> smtpOptions)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _smtpOptions = smtpOptions.Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await CheckDatabaseConnectionAsync(cancellationToken);
        await CheckSmtpConnectionAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task CheckDatabaseConnectionAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var canConnect = await dbContext.Database.CanConnectAsync(cancellationToken);

            if (canConnect)
            {
                _logger.LogInformation("database connection: true");
            }
            else
            {
                _logger.LogError("database connection: false");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "database connection error: {Message}", ex.Message);
        }
    }

    private async Task CheckSmtpConnectionAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var smtpClient = new SmtpClient();

            var socketOptions = _smtpOptions.UseSsl
                ? SecureSocketOptions.SslOnConnect
                : SecureSocketOptions.StartTls;

            await smtpClient.ConnectAsync(
                _smtpOptions.Host,
                _smtpOptions.Port,
                socketOptions,
                cancellationToken
            );

            if (!string.IsNullOrWhiteSpace(_smtpOptions.Username))
            {
                await smtpClient.AuthenticateAsync(
                    _smtpOptions.Username,
                    _smtpOptions.Password,
                    cancellationToken
                );
            }

            _logger.LogInformation("SMTP connection: true");

            await smtpClient.DisconnectAsync(true, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SMTP connection error: {Message}", ex.Message);
        }
    }
}