namespace Abwaab.Application.Features.Properties.Common.DTOs
{
    public class TimeSlotDTO
    {
        public Guid? TimeSlotId { get; set; }
        public int DayNumber { get; set; }
        public string? DayName { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Notes { get; set; }
    }
}
