using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Exceptions.Plans;
using Abwaab.Application.Common.Exceptions.Properties;
using Abwaab.Application.Contracts;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;

namespace Abwaab.Application.Features.Properties.Star
{
    public class StarPropertyCommandHandler : IRequestHandler<StarPropertyCommand, StarPropertyResponse>
    {
        private readonly IUserService _userService;
        private readonly IPropertyService _propertyService;
        private readonly string errorTitle = ErrorTitle.StarProperty;

        public StarPropertyCommandHandler(
            IUserService userService,
            IPropertyService propertyService)
        {
            _userService = userService;
            _propertyService = propertyService;
        }

        public async Task<StarPropertyResponse> Handle(StarPropertyCommand request, CancellationToken cancellationToken)
        {
            //check if property exist
            Property property = await _propertyService.FindPropertyByIdAsync(request.PropertyId, errorTitle);


            //check if property belong to user
            string username = _userService.FindUserNameByContext(errorTitle);
            ApplicationUser? user = await _userService.FindUserByNameAsync(username);

            if (user == null)
                throw new UserNotFoundException(username, errorTitle);

            //check if property belong to user
            if (property.UserPlan.UserId != user.Id)
                throw new ObjectNotBelongToUserException("العقار", errorTitle);

            //check if property already has stared
            if (property.IsStard)
                throw new PropertyAlreadyStaredExcption(errorTitle);

            //check if allowed to star
            int starCount = await _propertyService.GetStaredPropertyCountInPlanAsync(property.UserPlandId);

            if (starCount >= property.UserPlan.Plan.MaxStardPropertiesCountAtSameTime)
                throw new ExceededAllowedStarNumberException(property.UserPlan.Plan, errorTitle);

            //update property
            property.IsStard = true;
            await _propertyService.UpdatePropertyAsync(property);

            return new StarPropertyResponse() { Success = true, Message = "تم تمييز العقار بنجاح" };
        }
    }
}
