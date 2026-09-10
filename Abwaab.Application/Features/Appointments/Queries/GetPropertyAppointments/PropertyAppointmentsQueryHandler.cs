using Abwaab.Application.Common.Constants;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Domain.Entities.AppointmentEntities;
using MediatR;

namespace Abwaab.Application.Features.Appointments.Queries.GetPropertyAppointments;

public class PropertyAppointmentsQueryHandler : IRequestHandler<PropertyAppointmentsQuery, PropertyAppointmentsResponse>
{
    private readonly IAppointmentService _appointmentService;
    private readonly string errorTitle = ErrorTitle.PropertyAppointments;

    public PropertyAppointmentsQueryHandler(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    public async Task<PropertyAppointmentsResponse> Handle(PropertyAppointmentsQuery request, CancellationToken cancellationToken)
    {
        AppointmentState confirmedAppointments = await _appointmentService.GetConfirmedAppointmentStateAsync(errorTitle);
        AppointmentState pendingAppointments = await _appointmentService.GetPendingAppointmentStateAsync(errorTitle);
        List<AppointmentState> states = new() { confirmedAppointments, pendingAppointments };
        

        PropertyAppointmentsResponse response = await _appointmentService.GetPropertyAppointments(request.PropertyId, states, errorTitle);

        return response;
    }
}