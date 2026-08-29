namespace Abwaab.Application.Features.Properties.Queries.AvailableTimeSlots;

public class AvailableTimeSlotsResponse
{
    public int DayNumber { get; set; }
    public string DayName { get; set; } = string.Empty;
    public DateOnly DayDate { get; set; }
    public List<TimeDTO>? DayTimes { get; set; }
}
