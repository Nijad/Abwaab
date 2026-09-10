using Abwaab.Application.Common.Constants;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Domain.Entities.AppointmentEntities;
using MediatR;

namespace Abwaab.Application.Features.Appointments.Queries.GetPropertyAppointments;

public class PropertyAppointmentsQueryHandler : IRequestHandler<PropertyAppointmentsQuery, PropertyAppointmentsResponse>
{
    private readonly IAppointmentService _appointmentService;
    private readonly IPropertyService _propertyService;
    private readonly string errorTitle = ErrorTitle.PropertyAppointments;

    public PropertyAppointmentsQueryHandler(IAppointmentService appointmentService, IPropertyService propertyService)
    {
        _appointmentService = appointmentService;
        _propertyService = propertyService;
    }

    public async Task<PropertyAppointmentsResponse> Handle(PropertyAppointmentsQuery request, CancellationToken cancellationToken)
    {
        await _appointmentService.CancelMissedِppointments(errorTitle);

        AppointmentState pendingAppointments = await _appointmentService.GetPendingAppointmentStateAsync(errorTitle);
        AppointmentState confirmedAppointments = await _appointmentService.GetConfirmedAppointmentStateAsync(errorTitle);
        List<AppointmentState> states = new() { confirmedAppointments, pendingAppointments };

        PropertyAppointmentsResponse propertyAppointments = await _propertyService.FindPropertyByIdForAppointmentsAsync(request.PropertyId, errorTitle);

        propertyAppointments.Requests = await _appointmentService.GetPropertyAppointmentsRequests(request.PropertyId, states, errorTitle);

        return propertyAppointments;
    }
}