using Abwaab.Application.Features.Properties.Common;

namespace Abwaab.Application.Features.Properties.Queries.GetPropertyForUpdate
{
    public class PropertyForUpdateResponse : PropertyDTO
    {
        public List<PropertyTypeDTO> PropertyTypesList { get; set; } = new();
        public List<PropertyFinishingDTO> PropertyFinishingsList { get; set; } = new();
        public List<WeekDay> WeekDaysList { get; set; } = WeekDay.GetWeekDaysList();

        public List<AttributeDTO> Attributes { get; set; } = new();
    }
}
