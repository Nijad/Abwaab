namespace Abwaab.Application.Features.Properties.Queries.GetPropertyForUpdate
{
    public class PropertyForUpdateResponse
    {
        public Guid PropertyId { get; set; }
        public string? Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public decimal? AreaInSquareMeter { get; set; }
        public decimal? Price { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public Guid? ProperytTypeId { get; set; }
        public List<PropertyTypeForUpdate> PropertyTypesList { get; set; } = new();

        public Guid? PropertyFinishingId { get; set; }
        public List<PropertyFinishingForUpdate> PropertyFinishingsList { get; set; } = new();

        public List<TimeSlotForUpdate> TimeSlots { get; set; } = new();
        public List<WeekDay> WeekDaysList { get; set; }

        public List<AttributeForUpdate> Attributes { get; set; } = new();

        public List<PropertyAttributeForUpdate>? PropertyAttributesList { get; set; }

        public bool IsStar { get; set; }
    }
}
