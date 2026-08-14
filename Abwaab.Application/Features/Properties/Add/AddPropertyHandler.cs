using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Exceptions.Plans;
using Abwaab.Application.Contracts;
using Abwaab.Application.Features.Properties.Update;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;

namespace Abwaab.Application.Features.Properties.Add
{
    public class AddPropertyHandler : IRequestHandler<AddPropertyCommand, AddPropertyResponse>
    {
        private readonly IPropertyService _propertyService;
        private readonly IUserService _userService;
        private readonly IPlanService _planService;
        private readonly IUserPlanStateService _userPlanStateService;
        private readonly string errorTitle = ErrorTitle.AddingProperty;

        public AddPropertyHandler(
            IPropertyService propertyService,
            IUserService userService,
            IPlanService planService,
            IUserPlanStateService userPlanStateService)
        {
            _propertyService = propertyService;
            _userService = userService;
            _planService = planService;
            _userPlanStateService = userPlanStateService;
        }

        public async Task<AddPropertyResponse> Handle(AddPropertyCommand request, CancellationToken cancellationToken)
        {
            // get signed in user
            string username = _userService.FindUserNameByContext();

            ApplicationUser? user = await _userService.FindUserByNameAsync(username);
            if (user == null)
                throw new UserNotFoundException(username, errorTitle);

            //check if user can add new property depend thier balance in plan
            UserPlanStatus activUserPlanState = await _userPlanStateService.GetActiveUserPlanStatus(errorTitle);

            // get user active plan
            UserPlan activeUserPlan = await _planService.FindUserActivePlanAsync(user.Id, activUserPlanState.Id, errorTitle);
            
            // check if properties properties count less than allowed in the plan
            bool isAllowedToAdd = await _propertyService.HasBalanceToAddPropertyAsync(activeUserPlan);

            if (!isAllowedToAdd)
                throw new ExceededAllowedPropertyNumberException(activeUserPlan.Plan, errorTitle);

            Guid createdPropertyId = await _propertyService.CreatePropertyAsync(activeUserPlan, errorTitle);
            return new AddPropertyResponse() { PropertyId = createdPropertyId, Message = "New property created successfully." };
        }
    }
}
