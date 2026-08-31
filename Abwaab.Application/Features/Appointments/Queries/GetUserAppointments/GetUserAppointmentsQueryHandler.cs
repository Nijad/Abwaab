using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Contracts;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;

namespace Abwaab.Application.Features.Appointments.Queries.GetUserAppointments;

public class GetUserAppointmentsQueryHandler : IRequestHandler<GetUserAppointmentsQuery, List<GetUserAppointmentsResponse>>
{
    private readonly IUserService _userService;
    private readonly IAppointmentService _appointmentService;

    private readonly string errorTitle = ErrorTitle.UserAppointments;

    public GetUserAppointmentsQueryHandler(IUserService userService, IAppointmentService appointmentService)
    {
        _userService = userService;
        _appointmentService = appointmentService;
    }

    public async Task<List<GetUserAppointmentsResponse>> Handle(GetUserAppointmentsQuery request, CancellationToken cancellationToken)
    {
        //get user from context
        string username = _userService.FindUserNameByContext(errorTitle);
        ApplicationUser? user = await _userService.FindUserByNameAsync(username);
        if (user == null)
            throw new UserNotFoundException(username, errorTitle);

        List<GetUserAppointmentsResponse> responses = await _appointmentService.GetUserAppointmentsByUserIdAsync(user.Id, errorTitle);
        
        return responses;
    }
}