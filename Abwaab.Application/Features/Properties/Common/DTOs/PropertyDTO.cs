namespace Abwaab.Application.Features.Properties.Common.DTOs;
public class PropertyDTO : PropertyBaseDTO
{

    public Guid? PropertyTypeId { get; set; }
    public Guid? PropertyFinishingId { get; set; }
    public List<TimeSlotDTO>? TimeSlots { get; set; } = new();
    public List<PropertyAttributeDTO>? PropertyAttributesList { get; set; }
    public List<MediaDTO>? PropertyMediaList { get; set; }
}
