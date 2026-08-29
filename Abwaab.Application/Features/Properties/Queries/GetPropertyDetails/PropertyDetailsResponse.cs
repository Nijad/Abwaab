using Abwaab.Application.Features.Properties.Common.DTOs;

namespace Abwaab.Application.Features.Properties.Queries.GetPropertyDetails
{
    public class PropertyDetailsResponse : PropertyBaseDTO
    {
        public string PropertyType { get; set; } = string.Empty;
        public string PropertyFinishing { get; set; } = string.Empty;
        public List<MediaBaseDTO> PropertyMediaList { get; set; }
        public List<PropertyAttributeBaseDTO>? PropertyAttributesList { get; set; }
        public int ViewsNumber { get; set; }
    }
}