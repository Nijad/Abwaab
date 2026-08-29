using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Properties.TimeSlots;
using Abwaab.Application.Common.Mappings;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Properties.Common.DTOs;
using Abwaab.Domain.Entities.AppointmentEntities;
using MediatR;

namespace Abwaab.Application.Features.Properties.Queries.AvailableTimeSlots;

public class AvailableTimeSlotsQueryHandler : IRequestHandler<AvailableTimeSlotsQuery, List<AvailableTimeSlotsResponse>>
{
    private readonly IPropertyTimeSlotService _timeSlotService;
    private readonly IAppointmentService _appointmentService;
    private readonly string errorTitle = ErrorTitle.TimeSlotsQuery;

    public AvailableTimeSlotsQueryHandler(IPropertyTimeSlotService timeSlotService, IAppointmentService appointmentService)
    {
        _timeSlotService = timeSlotService;
        _appointmentService = appointmentService;
    }

    public async Task<List<AvailableTimeSlotsResponse>> Handle(AvailableTimeSlotsQuery request, CancellationToken cancellationToken)
    {
        //get property time slots
        List<TimeSlotDTO> timeSlots = await _timeSlotService.GetPropertyTimeSlotsListAsync(request.PropertyId);

        if (timeSlots == null || timeSlots.Count == 0)
            throw new NoTimeSlotsConfiguredException(errorTitle);

        DateOnly startDate = request.StartDate ?? DateOnly.FromDateTime(DateTime.Today);
        DateOnly endDate = startDate.AddDays(request.DaysCount - 1);

        // Generate a list of dates for the range
        List<DateOnly> dates = Enumerable.Range(0, request.DaysCount)
            .Select(offset => startDate.AddDays(offset))
            .ToList();

        List<Appointment> appointments = await _appointmentService.GetCommingAppointments(request.PropertyId, startDate, endDate, errorTitle, cancellationToken);

        // Build the response for each day
        List<AvailableTimeSlotsResponse> response = new();

        foreach (DateOnly date in dates)
        {
            int dayOfWeek = (int)date.DayOfWeek;

            // Find time slots that match this day
            List<TimeSlotDTO> slotsForDay = timeSlots.Where(ts => ts.DayNumber == dayOfWeek).ToList();

            List<TimeDTO> dayTimes = new List<TimeDTO>();
            foreach (var slot in slotsForDay)
            {
                // Check if this slot is blocked by ANY appointment
                bool isBlocked = appointments.Any(app =>
                {
                    // Appointment must be on the same day
                    if (DateOnly.FromDateTime(app.Date) != date)
                        return false;

                    TimeOnly appStart = TimeOnly.FromDateTime(app.Date);
                    TimeOnly appEnd = app.EndTime;

                    return slot.StartTime < appEnd && appStart < slot.EndTime;
                });

                if (!isBlocked)
                {
                    dayTimes.Add(new TimeDTO
                    {
                        StartTime = slot.StartTime,
                        EndTime = slot.EndTime
                    });
                }
            }
            if (dayTimes != null && dayTimes.Count > 0)
                response.Add(new AvailableTimeSlotsResponse
                {
                    DayNumber = dayOfWeek,
                    DayName = DayOfWeekMapping.Map(dayOfWeek),
                    DayDate = date,
                    DayTimes = dayTimes.OrderBy(x => x.StartTime).ToList()
                });
        }

        return response.OrderBy(x => x.DayDate).ToList();
    }
}