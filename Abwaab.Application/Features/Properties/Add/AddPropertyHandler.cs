using Abwaab.Application.Common.Exceptions;
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

        public AddPropertyHandler(
            IPropertyService propertyService,
            IUserService userService,
            IPlanService planService)
        {
            _propertyService = propertyService;
            _userService = userService;
            _planService = planService;
        }

        public async Task<AddPropertyResponse> Handle(AddPropertyCommand request, CancellationToken cancellationToken)
        {
            // get signed in user
            string username = _userService.FindUserNameByContext();

            ApplicationUser? user = await _userService.FindUserByNameAsync(username);
            if (user == null)
                throw new NotFoundException("User", nameof(username), username);

            //check if user can add new property depend thier balance in plan
            // get user active plan
            UserPlan? activePlan = await _planService.FindUserActivePlanAsync(user.Id);
            
            //UserPlan activeUserPlan = await _planService.

            // get properties count belong to active plan

            // check if properties properties count less than allowed in the plan



            Guid createdPropertyId = await _propertyService.CreatePropertyAsync();
            return new AddPropertyResponse() { PropertyId = createdPropertyId, Message = "New property created successfully." };
        }
    }
}
