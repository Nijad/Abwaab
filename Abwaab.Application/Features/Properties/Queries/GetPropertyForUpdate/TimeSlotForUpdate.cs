namespace Abwaab.Application.Features.Properties.Queries.GetPropertyForUpdate
{
    public class TimeSlotForUpdate
    {
        public Guid TimeSlotId { get; set; }
        public int Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Notes { get; set; }
    }
}
