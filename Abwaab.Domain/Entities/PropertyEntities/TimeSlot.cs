namespace Abwaab.Domain.Entities.PropertyEntities
{
    public class TimeSlot : BaseEntity
    {
        public DayOfWeek Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? Notes { get; set; }
        public Property Property { get; set; } = null!;
        public Guid PropertyId { get; set; }
    }
}
