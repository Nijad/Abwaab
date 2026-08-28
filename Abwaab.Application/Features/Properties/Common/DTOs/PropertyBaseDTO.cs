namespace Abwaab.Application.Features.Properties.Common.DTOs;

public class PropertyBaseDTO
{
    public Guid PropertyId { get; set; }
    public string? Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string? Address { get; set; } = string.Empty;
    public decimal? AreaInSquareMeter { get; set; }
    public decimal? Price { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public bool IsStar { get; set; }
    public string PropertyState { get; set; } = string.Empty;
}
