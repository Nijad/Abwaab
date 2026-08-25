using Abwaab.Application.Features.Properties.Common.DTOs;

namespace Abwaab.Application.Features.Properties.Queries.GetPropertyForUpdate
{
    public class PropertyForUpdateResponse : PropertyDTO
    {
        public List<PropertyTypeDTO> PropertyTypesList { get; set; } = new();
        public List<PropertyFinishingDTO> PropertyFinishingsList { get; set; } = new();
        public List<WeekDay> WeekDaysList { get; set; } = WeekDay.GetWeekDaysList();
        public List<AttributeDTO> Attributes { get; set; } = new();
        public List<MediaTypeDTO> MediaTypes { get; set; }
        public int RemainingStarsAllowed { get; set; }
        public int RemainingImagesAllowed { get; set; }
        public int RemainingVideosAllowed { get; set; }
    }
}
