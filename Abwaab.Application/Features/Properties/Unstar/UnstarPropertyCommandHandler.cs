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

namespace Abwaab.Application.Features.Properties.Unstar
{
    public class UnstarPropertyCommandHandler : IRequestHandler<UnstarPropertyCommand, UnstarPropertyResponse>
    {
        private readonly IUserService _userService;
        private readonly IPropertyService _propertyService;
        private readonly string errorTitle = ErrorTitle.UnstarProperty;

        public UnstarPropertyCommandHandler(
            IUserService userService, 
            IPropertyService propertyService)
        {
            _userService = userService;
            _propertyService = propertyService;
        }

        public async Task<UnstarPropertyResponse> Handle(UnstarPropertyCommand request, CancellationToken cancellationToken)
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
            
            //check if property already has unstared
            if (!property.IsStard)
                throw new PropertyAlreadyUnstaredExcption(errorTitle);

            //update property
            property.IsStard = false;
            await _propertyService.UpdatePropertyAsync(property);

            return new UnstarPropertyResponse() { Success = true , Message = "تم إلغاء تمييز العقار بنجاح"};
        }
    }
}
