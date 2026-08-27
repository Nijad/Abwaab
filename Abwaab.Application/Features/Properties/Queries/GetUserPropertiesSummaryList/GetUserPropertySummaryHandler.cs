using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Contracts;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Domain.Entities.AppointmentEntities;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;

namespace Abwaab.Application.Features.Properties.Queries.GetUserPropertiesSummaryList
{
    public class GetUserPropertySummaryHandler : IRequestHandler<GetUserPropertySummaryQuery, List<GetUserPropertySummaryResponse>>
    {
        private readonly IUserService _userService;
        private readonly IPropertyService _propertyService;
        private readonly IAppointmentService _appointmentService;
        private readonly string errorTitle = ErrorTitle.PropertiesQuery;

        public GetUserPropertySummaryHandler(
            IUserService userService,
            IPropertyService propertyService,
            IAppointmentService appointmentService)
        {
            _userService = userService;
            _propertyService = propertyService;
            _appointmentService = appointmentService;
        }

        public async Task<List<GetUserPropertySummaryResponse>> Handle(GetUserPropertySummaryQuery request, CancellationToken cancellationToken)
        {
            string username = _userService.FindUserNameByContext(errorTitle);
            ApplicationUser? user = await _userService.FindUserByNameAsync(username);
            if (user == null)
                throw new UserNotFoundException(username, errorTitle);

            List<GetUserPropertySummaryResponse> properties = await _propertyService.GetUserPropertiesSummaryAsync(user.Id);

            //get pending appointment for each property
            AppointmentState pendingAppointmentState = await _appointmentService.GetPendingAppointmentStateAsync(errorTitle);
            foreach (var property in properties)
                property.VisitRequest = await _appointmentService.GetAppointmentsCountByPropertyAndStateAsync(property.propertyId, pendingAppointmentState.Id);

            return properties;
        }
    }
}
