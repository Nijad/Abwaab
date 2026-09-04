using Abwaab.Application.Features.Properties.Common.DTOs;
namespace Abwaab.Application.Features.Visitors.SearchForm;

public class SearchFormResponse
{
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public decimal MinArea { get; set; }
    public decimal MaxArea { get; set; }
    public List<PropertyTypeDTO> PropertyTypes { get; set; } = new List<PropertyTypeDTO>();
    public List<PropertyFinishingDTO> PropertyFinishings { get; set; } = new List<PropertyFinishingDTO>();
    public List<PropertyViewSideDTO> PropertyViewSides { get; set; } = new List<PropertyViewSideDTO>();
}
