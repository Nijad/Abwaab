namespace Abwaab.Application.Features.Visitors.DTOs;

public class PropertyInfo
{
    public Guid PropertyId { get; set; }
    public string PropertyType { get; set; } = string.Empty;
    public string PropertyFinishing { get; set; } = string.Empty;
    public string CoverImage { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Price { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public List<string> ViewSidesList { get; set; } = new List<string>();
}
