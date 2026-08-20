namespace Abwaab.Application.Features.Properties.Common
{
    public class TimeSlotDTO
    {
        public Guid TimeSlotId { get; set; }
        public int Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Notes { get; set; }
    }
}
