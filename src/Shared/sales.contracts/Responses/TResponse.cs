using System.Text.Json.Serialization;

namespace sales.Contracts.Responses;

public class TResponse<TData>(TData? data, string? message = null, int code = 200) : Exception
{
    public TResponse()
        : this(default, null, 200) { }

    public TData? Data { get; init; } = data;
    public string? Message { get; init; } = message ?? string.Empty;
    public int StatusCode { get; init; } = code;

    [JsonIgnore]
    public bool IsSuccess => StatusCode is >= 200 and <= 299;
}
