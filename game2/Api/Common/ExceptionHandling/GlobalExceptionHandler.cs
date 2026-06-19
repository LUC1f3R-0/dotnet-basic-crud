using Microsoft.AspNetCore.Diagnostics;
using TestCrudApplication.Api.Common.Responses;
using TestCrudApplication.Application.Common.Exceptions;

public sealed class GlobalExceptionHandler: IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        int statusCode;
        string message;

        if (exception is AppException appException)
        {
            statusCode = appException.StatusCode;
            message = appException.Message;

            _logger.LogWarning(exception, "Application error occurred. StatusCode: {StatusCode}, TraceId: {TraceId}", statusCode, httpContext.TraceIdentifier);
        }
        else
        {
            statusCode = StatusCodes.Status500InternalServerError;
            message = "An unexpected server error occurred.";

            _logger.LogError(exception, "Unexpected server error occurred. TraceId: {TraceId}", httpContext.TraceIdentifier);
        }

        var response = new ErrorResponse
        {
            Message = message,
            TraceId = httpContext.TraceIdentifier
        };

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true;
    }
}