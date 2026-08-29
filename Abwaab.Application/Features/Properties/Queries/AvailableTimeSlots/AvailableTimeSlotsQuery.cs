using MediatR;

namespace Abwaab.Application.Features.Properties.Queries.AvailableTimeSlots;

public class AvailableTimeSlotsQuery : IRequest<List<AvailableTimeSlotsResponse>>
{
    public Guid PropertyId { get; set; }
    public DateOnly? StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
    public int DaysCount { get; set; } = 7;
}
