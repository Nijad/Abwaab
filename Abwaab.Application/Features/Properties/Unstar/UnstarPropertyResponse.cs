namespace Abwaab.Application.Features.Properties.Unstar;

public record UnstarPropertyResponse(bool Success)
{
    public string Message { get; set; } = string.Empty;
}
