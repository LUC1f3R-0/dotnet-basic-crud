namespace TestCrudApplication.Api.Common.Responses;

public sealed class ErrorResponse
{
    public bool Success { get; init; } = false;

    public string Message { get; init; } = string.Empty;

    public Dictionary<string, string[]>? Errors { get; init; }

    public string TraceId { get; init; } = string.Empty;
}