namespace Abwaab.Application.Features.Properties.Queries.GetPendingProperties;

public class PendingPropertiesResponse
{
    public Guid PropertyId { get; set; }
    public string PropertyTitle { get; set; } = string.Empty;
    public string CoverPath { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Area { get; set; }
    public decimal Price { get; set; }
}
