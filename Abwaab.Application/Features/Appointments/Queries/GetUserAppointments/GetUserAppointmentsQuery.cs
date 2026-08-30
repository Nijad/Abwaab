using MediatR;

namespace Abwaab.Application.Features.Appointments.Queries.GetUserAppointments;

public class GetUserAppointmentsQuery : IRequest<List<GetUserAppointmentsResponse>>
{
}
